using System.Collections.Generic;
using App.Models.Contracts;

namespace App.ViewModels.Contracts
{
    public interface ITreeViewModel
    {
        IEnumerable<ITreeItemModel> Items { get; }
    }
}