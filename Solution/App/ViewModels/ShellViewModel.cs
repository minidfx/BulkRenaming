using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
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
        public string PathSelected { get; private set; }

        public void InitShell()
        {
            this._openFolderService = ServiceLocator.Current.GetInstance<IOpenFolderService>();
        }

        public async Task BrowseAsync()
        {
            var folder = await this._openFolderService.PromptAsync();
        }

        public Task ApplyAsync()
        {
            throw new NotImplementedException();
        }

        protected override void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);

            this.PathSelected = Directory.GetCurrentDirectory();
            this.NotifyOfPropertyChange(() => this.PathSelected);

            this.FetchFolder();
        }

        private void FetchFolder()
        {
            var entries = Directory.GetFiles(this.PathSelected);

            foreach (var entry in entries.Select(entry => new {FileName = Path.GetFileName(entry), Path = entry}))
            {
                this.Files.Add(new ListViewModel(entry.FileName, new Uri(entry.Path)));
            }
        }
    }
}