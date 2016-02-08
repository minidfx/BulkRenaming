using App.ViewModels.Contracts;

using Caliburn.Micro;

namespace App.ViewModels
{
    public class ResultViewViewModel : ViewAware, IResultViewModel
    {
        #region Fields

        private string _futurResult;

        private string _result;

        #endregion

        #region Properties, Indexers

        public string Result
        {
            get { return this._result; }
            set
            {
                this._result = value;
                this.NotifyOfPropertyChange(() => this.Result);
            }
        }

        public string FuturResult
        {
            get { return this._futurResult; }
            set
            {
                this._futurResult = value;
                this.NotifyOfPropertyChange(() => this.FuturResult);
            }
        }

        #endregion
    }
}