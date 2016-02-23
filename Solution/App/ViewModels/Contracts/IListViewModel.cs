using System.Collections.Generic;
using System.Text.RegularExpressions;

using Windows.Storage;

namespace BulkRenaming.ViewModels.Contracts
{
    public interface IListViewModel
    {
        #region Properties, Indexers

        string FuturResult { get; set; }

        Match RegexMatch { get; set; }

        IEnumerable<string> Parts { get; set; }

        string FileName { get; }

        StorageFile StorageFile { get; }

        #endregion
    }
}