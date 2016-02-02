using System;
using App.ViewModels.Contracts;
using WinRTXamlToolkit.Controls;
using WinRTXamlToolkit.Interactivity;

namespace App.Behaviors
{
    public class TreeViewBehavior : Behavior<TreeView>
    {
        protected override void OnLoaded()
        {
            base.OnLoaded();

            var viewModel = this.DataContext as ITreeViewModel;
            if (viewModel == null)
            {
                throw new ArgumentException("The view model associated must implement the interface ITreeViewModel.");
            }

            foreach (var treeItemModel in viewModel.Items)
            {
                // Read the view model and fill the treeview
            }
        }
    }
}