using System.Collections.ObjectModel;
using System.Threading.Tasks;

using Windows.Storage;

using App.Models.Contracts;

namespace App.ViewModels.Contracts
{
    public interface IShellViewModel
    {
        #region Properties, Indexers

        string Pattern { get; set; }

        string ReplacePattern { get; set; }

        ObservableCollection<IListViewModel> Files { get; }

        StorageFolder FolderSelected { get; }

        #endregion

        #region All other members

        void InitShell();

        Task BrowseAsync();

        Task ApplyAsync();

        #endregion
    }
}