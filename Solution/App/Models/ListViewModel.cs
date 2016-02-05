using System;

using App.Models.Contracts;
using App.ViewModels;
using App.ViewModels.Contracts;

using Caliburn.Micro;

namespace App.Models
{
    public class ListViewModel : ViewAware, IListViewModel
    {
        #region Constructors

        public ListViewModel(string name, Uri path)
        {
            this.Name = name;
            this.Path = path;

            this.RegexResult = new ResultViewViewModel {Result = name};
        }

        #endregion

        #region Properties, Indexers

        public string Name { get; }

        public Uri Path { get; }

        public IResultViewModel RegexResult { get; set; }

        #endregion
    }
}