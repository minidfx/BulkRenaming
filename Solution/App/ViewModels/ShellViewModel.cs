using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

using Windows.Storage;

using App.Models;
using App.Models.Contracts;
using App.Services.Contracts;
using App.ViewModels.Contracts;

using Caliburn.Micro;

using Microsoft.Practices.ServiceLocation;

namespace App.ViewModels
{
    public sealed class ShellViewModel : Screen, IShellViewModel
    {
        #region Fields

        private IOpenFolderService _openFolderService;

        private string _pattern;

        private string _replacePattern;

        #endregion

        #region Properties, Indexers

        public string Pattern
        {
            get { return this._pattern; }
            set
            {
                UpdatePattern(value);
                this._pattern = value;
            }
        }

        public string ReplacePattern
        {
            get { return this._replacePattern; }
            set
            {
                UpdateReplacePattern(value);
                this._replacePattern = value;
            }
        }

        public ObservableCollection<IListViewModel> Files { get; } = new ObservableCollection<IListViewModel>();

        public StorageFolder FolderSelected { get; private set; }

        #endregion

        #region Interface Implementations

        public void InitShell()
        {
            this._openFolderService = ServiceLocator.Current.GetInstance<IOpenFolderService>();
        }

        public async Task BrowseAsync()
        {
            this.FolderSelected = await this._openFolderService.PromptAsync();

            this.Files.Clear();
            await this.FetchFolderAsync();

            this.NotifyOfPropertyChange(() => this.FolderSelected);
        }

        public Task ApplyAsync()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region All other members

        protected override void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);

            this.NotifyOfPropertyChange(() => this.FolderSelected);
        }

        private static void UpdatePattern(string value)
        {
            throw new NotImplementedException();
        }

        private static void UpdateReplacePattern(string value)
        {
            throw new NotImplementedException();
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