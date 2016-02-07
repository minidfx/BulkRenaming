using System;
using System.Collections.ObjectModel;
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

        private IOpenFolderService _openFolderService;

        #endregion

        #region Properties, Indexers

        public ReactiveProperty<string> Pattern { get; set; } = new ReactiveProperty<string>().Where(x => x != null)
                                                                                              .Throttle(TimeSpan.FromMilliseconds(500))
                                                                                              .ToReactiveProperty();

        public ReactiveProperty<string> ReplacePattern { get; set; } = new ReactiveProperty<string>().Where(x => x != null)
                                                                                                     .Throttle(TimeSpan.FromMilliseconds(500))
                                                                                                     .ToReactiveProperty();

        public ObservableCollection<IListViewModel> Files { get; } = new ObservableCollection<IListViewModel>();

        public StorageFolder FolderSelected { get; private set; }

        public string PatternFound { get; set; }

        public string ReplacePatternFound { get; set; }

        #endregion

        #region Interface Implementations

        public Task ApplyAsync()
        {
            throw new NotImplementedException();
        }

        public async Task BrowseAsync()
        {
            this.FolderSelected = await this._openFolderService.PromptAsync();

            this.Files.Clear();
            await this.FetchFolderAsync();

            this.NotifyOfPropertyChange(() => this.FolderSelected);
            await Task.Run(() => this.CaculateRegex(this.Pattern.Value, this.ReplacePattern.Value));
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
            if (pattern == null || replacePattern == null)
            {
                return;
            }

            try
            {
                var regex = new Regex(pattern);

                foreach (var file in this.Files)
                {
                    var match = regex.Match(file.Name);
                    if (match.Success)
                    {
                        file.RegexResult.Result = match.Groups.Cast<Group>().Skip(1).Select(x => x.Value);
                        file.RegexResult.FuturResult = match.Result(replacePattern);
                    }
                    else
                    {
                        file.RegexResult.Result = Enumerable.Empty<string>();
                        file.RegexResult.FuturResult = string.Empty;
                    }
                }
            }
            catch (ArgumentException)
            {
                this.Files.ForEach(x =>
                {
                    var regexResult = x.RegexResult;

                    regexResult.Result = Enumerable.Empty<string>();
                    regexResult.FuturResult = string.Empty;
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
            var entries = await this.FolderSelected.GetFilesAsync();

            foreach (var entry in entries.Select(entry => new {entry.Name, entry.Path}))
            {
                this.Files.Add(new ListViewModel(entry.Name, new Uri(entry.Path)));
            }
        }

        #endregion
    }
}