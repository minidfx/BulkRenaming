using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Windows.Storage;
using Windows.UI.ViewManagement;

using BulkRenaming.Services.Contracts;
using BulkRenaming.ViewModels.Contracts;

using Caliburn.Micro;

using Reactive.Bindings;

namespace BulkRenaming.ViewModels
{
    public sealed class ShellViewModel : Screen, IShellViewModel
    {
        #region Fields and Constants

        private static readonly ILog Logger = LogManager.GetLog(typeof (ShellViewModel));

        #endregion

        #region Fields

        private readonly CompositeDisposable _disposables = new CompositeDisposable();

        private readonly IOpenFolderService _openFolderService;

        private StorageFolder _folderSelected;

        private bool _isLoading;

        #endregion

        #region Constructors

        public ShellViewModel(IOpenFolderService openFolderService)
        {
            this._openFolderService = openFolderService;

            this.Files = Enumerable.Empty<IListViewModel>();
            this.FolderSelected = "Select a folder by clicking on the right button";

            this.Pattern = new ReactiveProperty<string>();
            this.ReplacePattern = new ReactiveProperty<string>();
        }

        #endregion

        #region Properties, Indexers

        public ReactiveProperty<string> Pattern { get; }

        public ReactiveProperty<string> ReplacePattern { get; }

        public IEnumerable<IListViewModel> Files { get; private set; }

        public string FolderSelected { get; private set; }

        public bool IsLoading
        {
            get { return this._isLoading; }
            private set
            {
                this._isLoading = value;
                this.NotifyOfPropertyChange(() => this.IsLoading);
            }
        }

        public bool CanApplyAsync { get; private set; }

        #endregion

        #region Interface Implementations

        public async Task ApplyAsync()
        {
            this.IsLoading = true;

            using (Disposable.Create(() => this.IsLoading = false))
            {
                var actualFiles = await this._folderSelected.GetFilesAsync();

                foreach (var file in this.Files.AsParallel()
                                         .Where(x => x.FuturResult != null &&
                                                     actualFiles.Any(f => f.Path == x.StorageFile.Path)
                                                     && x.FileName != x.FuturResult))
                {
                    await file.StorageFile.RenameAsync(file.FuturResult);
                }

                await this.FetchFolderAsync();
                this.CalculateRegex();
            }
        }

        public async Task BrowseAsync()
        {
            using (Disposable.Create(() => this.IsLoading = false))
            {
                this.IsLoading = true;

                if (!IsUnsnapped())
                {
                    throw new NotImplementedException("Cannot browse any folders when the application is snapped.");
                }

                this._folderSelected = await this._openFolderService.PromptAsync();

                if (this._folderSelected == null)
                {
                    return;
                }

                await this.FetchFolderAsync();

                this.FolderSelected = this._folderSelected.Path;
                this.NotifyOfPropertyChange(() => this.FolderSelected);

                this.CalculateRegex();
            }
        }

        #endregion

        #region All other members

        /// <summary>
        ///     FilePicker APIs will not work if the application is in a snapped state.
        ///     If an app wants to show a FilePicker while snapped, it must attempt to unsnap first
        /// </summary>
        /// <returns>
        ///     <see langword="true" /> whether the application is unsnapped otherwise <see langword="false" />.
        /// </returns>
        /// <remarks>
        ///     Copied from https://msdn.microsoft.com/library/windows/apps/br207881?cs-save-lang=1&cs-lang=csharp#code-snippet-2
        /// </remarks>
        private static bool IsUnsnapped()
        {
            // FilePicker APIs will not work if the application is in a snapped state.
            // If an app wants to show a FilePicker while snapped, it must attempt to unsnap first
            return ((ApplicationView.Value != ApplicationViewState.Snapped) || ApplicationView.TryUnsnap());
        }

        /// <summary>
        ///     Called when activating.
        /// </summary>
        protected override void OnActivate()
        {
            base.OnActivate();

            this._disposables.Add(this.Pattern
                                      .Throttle(TimeSpan.FromMilliseconds(200))
                                      .Select(x => string.IsNullOrWhiteSpace(x) ? null : x)
                                      .Do(this.CalculateRegex)
                                      .Merge(this.ReplacePattern
                                                 .Throttle(TimeSpan.FromMilliseconds(200))
                                                 .Select(x => string.IsNullOrWhiteSpace(x) ? null : x)
                                                 .Do(this.CalculateReplacePattern))
                                      .SubscribeOn(TaskPoolScheduler.Default)
                                      .ObserveOn(UIDispatcherScheduler.Default)
                                      .Subscribe(x =>
                                      {
                                          foreach (var file in this.Files)
                                          {
                                              file.NotifyUi();
                                          }

                                          this.NotifyOfPropertyChange(() => this.CanApplyAsync);
                                      },
                                                 e => { Logger.Error(e); }));
        }

        private void CalculateReplacePattern()
        {
            this.CalculateReplacePattern(this.ReplacePattern.Value);
        }

        private void CalculateReplacePattern(string replacePattern)
        {
            // When the replace pattern is specified
            if (!string.IsNullOrWhiteSpace(replacePattern))
            {
                var increment = 1;

                foreach (var item in this.Files.Where(x => x.RegexMatch == null || !x.RegexMatch.Success))
                {
                    item.FuturResult = null;
                }

                foreach (var item in this.Files.Where(x => x.RegexMatch != null && x.RegexMatch.Success))
                {
                    try
                    {
                        var match = item.RegexMatch;
                        item.FuturResult = match.Result(replacePattern).Replace("%i", increment++.ToString());
                    }
                    catch (ArgumentException)
                    {
                    }
                }

                this.CanApplyAsync = this.Files.Any(x => x.RegexMatch != null && x.RegexMatch.Success) && this.ReplacePattern.Value != null;
            }
            else
            {
                foreach (var item in this.Files)
                {
                    item.FuturResult = null;
                }

                this.CanApplyAsync = false;
            }
        }

        private void CalculateRegex()
        {
            this.CalculateRegex(this.Pattern.Value);
        }

        private void CalculateRegex(string pattern)
        {
            if (pattern == null)
            {
                foreach (var file in this.Files)
                {
                    file.Parts = new[] {file.FileName};
                    file.FuturResult = null;
                }

                return;
            }

            Regex regex;

            try
            {
                regex = new Regex(pattern, RegexOptions.IgnoreCase);
            }
            catch (ArgumentException)
            {
                foreach (var item in this.Files)
                {
                    item.Parts = new[] {item.FileName};
                    item.FuturResult = null;
                }

                return;
            }

            // Items with a match
            foreach (var item in this.Files)
            {
                var match = item.RegexMatch = regex.Match(item.FileName);
                var patternIdentified = match.Value;
                var fileName = item.FileName;
                var indexOf = fileName.IndexOf(patternIdentified, StringComparison.Ordinal);

                item.Parts = match.Success
                                 ? new[]
                                   {
                                       fileName.Substring(0, indexOf),
                                       fileName.Substring(indexOf, patternIdentified.Length),
                                       fileName.Substring(indexOf + patternIdentified.Length)
                                   }
                                 : new[] {item.FileName};
            }

            this.CalculateReplacePattern();
        }

        /// <summary>
        ///     Called when deactivating.
        /// </summary>
        /// <param name="close">Inidicates whether this instance will be closed.</param>
        protected override void OnDeactivate(bool close)
        {
            base.OnDeactivate(close);

            this._disposables.Dispose();

            if (close)
            {
                this.Pattern.Dispose();
                this.ReplacePattern.Dispose();
            }
        }

        private async Task FetchFolderAsync()
        {
            var entries = await this._folderSelected.GetFilesAsync();
            var files = new LinkedList<IListViewModel>();

            foreach (var entry in entries)
            {
                files.AddLast(new ListViewModel(entry.Name, entry));
            }

            this.Files = files;
            this.NotifyOfPropertyChange(() => this.Files);
        }

        #endregion
    }
}