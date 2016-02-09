using System.Collections.Generic;

using Windows.Storage;

namespace App.Models.Contracts
{
    public interface IListViewModel
    {
        #region Properties, Indexers

        string FuturResult { get; set; }

        IEnumerable<string> Parts { get; set; }

        string FileName { get; set; }

        bool Success { get; set; }

        StorageFile StorageFile { get; set; }

        #endregion
    }
}