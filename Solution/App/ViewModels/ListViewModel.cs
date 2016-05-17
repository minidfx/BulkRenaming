using System.Collections.Generic;
using System.Text.RegularExpressions;

using Windows.Storage;

using BulkRenaming.ViewModels.Contracts;

using Caliburn.Micro;

namespace BulkRenaming.ViewModels
{
    public sealed class ListViewModel : ViewAware, IListViewModel
    {
        #region Constructors

        public ListViewModel(string fileName, StorageFile storageFile)
        {
            this.FileName = fileName;
            this.Parts = new[] {fileName};
            this.StorageFile = storageFile;
        }

        #endregion

        #region Properties, Indexers

        public string FileName { get; }

        public StorageFile StorageFile { get; }

        public string FuturResult { get; set; }

        public Match RegexMatch { get; set; }

        public IEnumerable<string> Parts { get; set; }

        #endregion

        #region Interface Implementations

        public void NotifyUi()
        {
            this.NotifyOfPropertyChange(() => this.Parts);
            this.NotifyOfPropertyChange(() => this.FuturResult);
        }

        #endregion
    }
}