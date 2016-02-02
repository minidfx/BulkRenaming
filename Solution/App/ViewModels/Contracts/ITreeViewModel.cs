using System.Collections.Generic;

namespace App.ViewModels.Contracts
{
    public interface ITreeViewModel
    {
        IEnumerable<ITreeViewModel> Items { get; }
    }
}