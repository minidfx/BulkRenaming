using System.Collections.Generic;
using System.Text.RegularExpressions;

using Windows.Storage;

using BulkRenaming.ViewModels.Contracts;

using Caliburn.Micro;

namespace BulkRenaming.ViewModels
{
    public sealed class ListViewModel : ViewAware, IListViewModel
    {
        #region Fields

        private string _futurResult;

        private IEnumerable<string> _parts;

        #endregion

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

        public string FuturResult
        {
            get { return this._futurResult; }
            set
            {
                this._futurResult = value;
                this.NotifyOfPropertyChange(() => this.FuturResult);
            }
        }

        public Match RegexMatch { get; set; }

        public IEnumerable<string> Parts
        {
            get { return this._parts; }
            set
            {
                this._parts = value;
                this.NotifyOfPropertyChange(() => this.Parts);
            }
        }

        #endregion
    }
}