using System.Collections.ObjectModel;
using System.Threading.Tasks;
using App.Models.Contracts;

namespace App.ViewModels.Contracts
{
    public interface IShellViewModel
    {
        ObservableCollection<IListViewModel> Files { get; }

        string PathSelected { get; }

        void InitShell();
        Task BrowseAsync();
        Task ApplyAsync();
    }
}