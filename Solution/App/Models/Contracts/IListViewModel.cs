using System;

namespace App.Models.Contracts
{
    public interface IListViewModel
    {
        string Name { get; }
        Uri Path { get; }
    }
}