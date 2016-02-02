using System.Collections.Generic;
using App.ViewModels.Contracts;
using Caliburn.Micro;

namespace App.ViewModels
{
    public sealed class ShellViewModel : Screen, IShellViewModel, ITreeViewModel
    {
        public IEnumerable<ITreeViewModel> Items { get; } = new List<ITreeViewModel>();
    }
}