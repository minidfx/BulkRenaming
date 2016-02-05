using System;

using App.ViewModels.Contracts;

namespace App.Models.Contracts
{
    public interface IListViewModel
    {
        #region Properties, Indexers

        string Name { get; }

        Uri Path { get; }

        IResultViewModel RegexResult { get; set; }

        #endregion
    }
}