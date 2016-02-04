using System;
using System.Collections.Generic;

namespace App.Models.Contracts
{
    public interface ITreeItemModel
    {
        #region Properties, Indexers

        string Name { get; set; }

        IEnumerable<ITreeItemModel> Items { get; set; }

        Uri Path { get; set; }

        #endregion
    }
}