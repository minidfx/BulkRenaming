using System.Collections.Generic;
using System.Threading.Tasks;

using Reactive.Bindings;

namespace BulkRenaming.ViewModels.Contracts
{
    public interface IShellViewModel
    {
        #region Properties, Indexers

        ReactiveProperty<string> Pattern { get; }

        ReactiveProperty<string> ReplacePattern { get; }

        IEnumerable<IListViewModel> Files { get; }

        string FolderSelected { get; }

        bool IsLoading { get; }

        bool CanApplyAsync { get; }

        #endregion

        #region All other members

        Task BrowseAsync();

        Task ApplyAsync();

        #endregion
    }
}