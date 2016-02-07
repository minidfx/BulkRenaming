using System.Collections.ObjectModel;
using System.Threading.Tasks;

using Windows.Storage;

using App.Models.Contracts;

using Reactive.Bindings;

namespace App.ViewModels.Contracts
{
    public interface IShellViewModel
    {
        #region Properties, Indexers

        ReactiveProperty<string> Pattern { get; set; }

        ReactiveProperty<string> ReplacePattern { get; set; }

        ObservableCollection<IListViewModel> Files { get; }

        string FolderSelected { get; }

        #endregion

        #region All other members

        Task BrowseAsync();

        Task ApplyAsync();

        #endregion
    }
}