using System.Collections.Generic;

using BulkRenaming.Models.Contracts;

namespace BulkRenaming.ViewModels.Contracts
{
    public interface ITreeViewModel
    {
        #region Properties, Indexers

        IEnumerable<ITreeItemModel> Items { get; }

        #endregion
    }
}