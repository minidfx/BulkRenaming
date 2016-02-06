using System.Collections.Generic;
using System.ComponentModel;

namespace App.ViewModels.Contracts
{
    public interface IResultViewModel : INotifyPropertyChanged
    {
        #region Properties, Indexers

        IEnumerable<string> Result { get; set; }

        string FuturResult { get; set; }

        #endregion
    }
}