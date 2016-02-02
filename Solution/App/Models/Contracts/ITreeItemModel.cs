using System.Collections.Generic;

namespace App.Models.Contracts
{
    public interface ITreeItemModel
    {
        string Name { get; set; }
        IEnumerable<ITreeItemModel> Items { get; set; }
    }
}