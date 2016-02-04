using System;

namespace App.Models.Contracts
{
    public interface IListViewModel
    {
        #region Properties, Indexers

        string Name { get; }

        Uri Path { get; }

        #endregion
    }
}