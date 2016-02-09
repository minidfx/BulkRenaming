using System.Collections.Generic;
using System.Threading.Tasks;

using App.Models.Contracts;

using Reactive.Bindings;

namespace App.ViewModels.Contracts
{
    public interface IShellViewModel
    {
        #region Properties, Indexers

        ReactiveProperty<string> Pattern { get; set; }

        ReactiveProperty<string> ReplacePattern { get; set; }

        IEnumerable<IListViewModel> Files { get; }

        string FolderSelected { get; }

        bool IsLoading { get; }

        #endregion

        #region All other members

        Task BrowseAsync();

        Task ApplyAsync();

        #endregion
    }
}