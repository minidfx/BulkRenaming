using System.Collections.Generic;
using App.Models.Contracts;

namespace App.Models
{
    public class TreeItemModel : ITreeItemModel
    {
        public string Name { get; set; }
        public IEnumerable<ITreeItemModel> Items { get; set; }
    }
}