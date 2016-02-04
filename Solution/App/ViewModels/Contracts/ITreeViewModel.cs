using System.Collections.Generic;

using App.Models.Contracts;

namespace App.ViewModels.Contracts
{
    public interface ITreeViewModel
    {
        #region Properties, Indexers

        IEnumerable<ITreeItemModel> Items { get; }

        #endregion
    }
}