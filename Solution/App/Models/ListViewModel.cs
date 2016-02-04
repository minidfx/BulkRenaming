using System;
using App.Models.Contracts;

namespace App.Models
{
    public class ListViewModel : IListViewModel
    {
        public ListViewModel(string name, Uri path)
        {
            this.Name = name;
            this.Path = path;
        }

        public string Name { get; }
        public Uri Path { get; }
    }
}