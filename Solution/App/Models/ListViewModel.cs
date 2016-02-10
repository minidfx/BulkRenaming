using System.Collections.Generic;

using Windows.Storage;

using BulkRenaming.Models.Contracts;

using Caliburn.Micro;

namespace BulkRenaming.Models
{
    public sealed class ListViewModel : ViewAware, IListViewModel
    {
        #region Fields

        private string _fileName;

        private string _futurResult;

        private IEnumerable<string> _parts;

        private bool _success;

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

        public string FileName
        {
            get { return this._fileName; }
            set
            {
                this._fileName = value;
                this.NotifyOfPropertyChange(() => this.FileName);
            }
        }

        public StorageFile StorageFile { get; set; }

        public string FuturResult
        {
            get { return this._futurResult; }
            set
            {
                this._futurResult = value;
                this.NotifyOfPropertyChange(() => this.FuturResult);
            }
        }

        public IEnumerable<string> Parts
        {
            get { return this._parts; }
            set
            {
                this._parts = value;
                this.NotifyOfPropertyChange(() => this.Parts);
            }
        }

        public bool Success
        {
            get { return this._success; }
            set
            {
                this._success = value;
                this.NotifyOfPropertyChange(() => this.Success);
            }
        }

        #endregion
    }
}