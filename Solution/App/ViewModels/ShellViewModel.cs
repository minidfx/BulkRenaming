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
        private IOpenFolderService _openFolderService;

        public ObservableCollection<IListViewModel> Files { get; } = new ObservableCollection<IListViewModel>();
        public StorageFolder FolderSelected { get; private set; }

        public void InitShell()
        {
            this._openFolderService = ServiceLocator.Current.GetInstance<IOpenFolderService>();
        }

        public async Task BrowseAsync()
        {
            this.FolderSelected = await this._openFolderService.PromptAsync();

            this.NotifyOfPropertyChange(() => this.FolderSelected);

            this.Files.Clear();
            await this.FetchFolder();
        }

        public Task ApplyAsync()
        {
            throw new NotImplementedException();
        }

        protected override void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);

            this.NotifyOfPropertyChange(() => this.FolderSelected);
        }

        private async Task FetchFolder()
        {
            var entries = await this.FolderSelected.GetFilesAsync();

            foreach (var entry in entries.Select(entry => new {entry.Name, entry.Path}))
            {
                this.Files.Add(new ListViewModel(entry.Name, new Uri(entry.Path)));
            }
        }
    }
}