using System;
using System.Collections.Generic;
using System.Linq;

using App.Models.Contracts;

namespace App.Models
{
    public sealed class TreeItemModel : ITreeItemModel
    {
        #region Constructors

        public TreeItemModel(string name, string path, IEnumerable<ITreeItemModel> items)
        {
            this.Name = name;
            this.Items = items;
            this.Path = new Uri(path);
        }

        public TreeItemModel(string name, string path) : this(name, path, Enumerable.Empty<ITreeItemModel>())
        {
        }

        #endregion

        #region Properties, Indexers

        public Uri Path { get; set; }

        public string Name { get; set; }

        public IEnumerable<ITreeItemModel> Items { get; set; }

        #endregion
    }
}