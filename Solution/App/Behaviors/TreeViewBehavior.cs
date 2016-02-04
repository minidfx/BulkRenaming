using System;

using App.Models.Contracts;
using App.ViewModels.Contracts;

using WinRTXamlToolkit.Controls;
using WinRTXamlToolkit.Interactivity;

namespace App.Behaviors
{
    public class TreeViewBehavior : Behavior<TreeView>
    {
        #region All other members

        protected override void OnLoaded()
        {
            base.OnLoaded();

            var viewModel = this.DataContext as ITreeViewModel;
            if (viewModel == null)
            {
                throw new ArgumentException("The view model associated must implement the interface ITreeViewModel.");
            }

            foreach (var item in viewModel.Items)
            {
                this.AssociatedObject.Items?.Add(this.BuildTreeViewItem(item));
            }
        }

        private TreeViewItem BuildTreeViewItem(ITreeItemModel treeItemModel)
        {
            var newTreeViewItem = new TreeViewItem {Header = treeItemModel.Name};

            foreach (var item in treeItemModel.Items)
            {
                newTreeViewItem.Items?.Add(this.BuildTreeViewItem(item));
            }

            return newTreeViewItem;
        }

        #endregion
    }
}