using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Windows.Storage;

using App.Models;
using App.Models.Contracts;
using App.Services.Contracts;
using App.ViewModels.Contracts;

using Caliburn.Micro;

using Microsoft.Practices.ServiceLocation;

using Reactive.Bindings;

using WinRTXamlToolkit.Tools;

namespace App.ViewModels
{
    public sealed class ShellViewModel : Screen, IShellViewModel
    {
        #region Fields

        private readonly CompositeDisposable _disposables = new CompositeDisposable();

        private StorageFolder _folderSelected;

        private bool _isLoading;

        private IOpenFolderService _openFolderService;

        #endregion

        #region Constructors

        public ShellViewModel()
        {
            this.Files = Enumerable.Empty<IListViewModel>();

            this.Pattern = new ReactiveProperty<string>()
                .Throttle(TimeSpan.FromMilliseconds(200))
                .Select(x => string.IsNullOrWhiteSpace(x) ? null : x)
                .ToReactiveProperty();
            this.ReplacePattern = new ReactiveProperty<string>()
                .Throttle(TimeSpan.FromMilliseconds(200))
                .Select(x => string.IsNullOrWhiteSpace(x) ? null : x)
                .ToReactiveProperty();
        }

        #endregion

        #region Properties, Indexers

        public ReactiveProperty<string> Pattern { get; }

        public ReactiveProperty<string> ReplacePattern { get; }

        public IEnumerable<IListViewModel> Files { get; private set; }

        public string FolderSelected => this._folderSelected?.Path;

        public bool IsLoading
        {
            get { return this._isLoading; }
            private set
            {
                this._isLoading = value;
                this.NotifyOfPropertyChange(() => this.IsLoading);
            }
        }

        #endregion

        #region Interface Implementations

        public async Task ApplyAsync()
        {
            this.IsLoading = true;

            using (Disposable.Create(() => this.IsLoading = false))
            {
                foreach (var file in this.Files.Where(x => x.FuturResult != null))
                {
                    await file.StorageFile.RenameAsync(file.FuturResult);
                }

                await this.FetchFolderAsync();
                await Task.Run(() => this.CaculateRegex(this.Pattern.Value, this.ReplacePattern.Value));
            }
        }

        public async Task BrowseAsync()
        {
            this.IsLoading = true;

            using (Disposable.Create(() => this.IsLoading = false))
            {
                this._folderSelected = await this._openFolderService.PromptAsync();

                if (this._folderSelected == null)
                {
                    return;
                }

                await this.FetchFolderAsync();

                this.NotifyOfPropertyChange(() => this.FolderSelected);
                await Task.Run(() => this.CaculateRegex(this.Pattern.Value, this.ReplacePattern.Value));
            }
        }

        #endregion

        #region All other members

        /// <summary>
        ///     Called when initializing.
        /// </summary>
        protected override void OnInitialize()
        {
            base.OnInitialize();

            this._openFolderService = ServiceLocator.Current.GetInstance<IOpenFolderService>();
        }

        /// <summary>
        ///     Called when activating.
        /// </summary>
        protected override void OnActivate()
        {
            base.OnActivate();

            this._disposables.Add(this.Pattern
                                      .Do(pattern => this.CaculateRegex(pattern, this.ReplacePattern.Value))
                                      .Merge(this.ReplacePattern
                                                 .Do(replacePattern => this.CaculateRegex(this.Pattern.Value, replacePattern)))
                                      .SubscribeOn(TaskPoolScheduler.Default)
                                      .ObserveOn(UIDispatcherScheduler.Default)
                                      .Subscribe());
        }

        private void CaculateRegex(string pattern, string replacePattern)
        {
            try
            {
                if (pattern == null)
                {
                    foreach (var file in this.Files)
                    {
                        file.Parts = new[] {file.FileName};
                        file.FuturResult = null;
                        file.Success = false;
                    }

                    return;
                }

                var regex = new Regex(pattern);

                foreach (var file in this.Files)
                {
                    var fileName = file.FileName;
                    var match = regex.Match(fileName);

                    file.Success = match.Success;

                    if (match.Success)
                    {
                        var patternIdentified = match.Value;
                        var indexOf = file.FileName.IndexOf(patternIdentified, StringComparison.Ordinal);

                        file.Parts = new[]
                                     {
                                         fileName.Substring(0, indexOf),
                                         fileName.Substring(indexOf, patternIdentified.Length),
                                         fileName.Substring(indexOf + patternIdentified.Length)
                                     };

                        if (replacePattern == null)
                        {
                            file.FuturResult = null;
                            continue;
                        }

                        var result = match.Result(replacePattern);
                        file.FuturResult = string.IsNullOrWhiteSpace(result) ? match.Result(replacePattern) : result;
                    }
                    else
                    {
                        file.Parts = new[] {fileName};
                        file.FuturResult = null;
                        file.Success = false;
                    }
                }
            }
            catch (ArgumentException)
            {
                this.Files.ForEach(x =>
                {
                    x.Parts = new[] {x.FileName};
                    x.FuturResult = null;
                    x.Success = false;
                });
            }
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