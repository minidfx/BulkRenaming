using System;

using App.Models.Contracts;

namespace App.Models
{
    public class ListViewModel : IListViewModel
    {
        #region Constructors

        public ListViewModel(string name, Uri path)
        {
            this.Name = name;
            this.Path = path;
        }

        #endregion

        #region Properties, Indexers

        public string Name { get; }

        public Uri Path { get; }

        #endregion
    }
}