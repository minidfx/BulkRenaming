using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.Storage;
using App.Models.Contracts;

namespace App.ViewModels.Contracts
{
    public interface IShellViewModel
    {
        string Pattern { get; set; }
        string ReplacePattern { get; set; }
        ObservableCollection<IListViewModel> Files { get; }
        StorageFolder FolderSelected { get; }
        void InitShell();
        Task BrowseAsync();
        Task ApplyAsync();
    }
}