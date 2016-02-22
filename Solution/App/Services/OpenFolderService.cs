using Windows.Foundation;
using Windows.Storage;
using Windows.Storage.Pickers;

using BulkRenaming.Services.Contracts;

namespace BulkRenaming.Services
{
    public sealed class OpenFolderService : IOpenFolderService
    {
        #region Interface Implementations

        public IAsyncOperation<StorageFolder> PromptAsync()
        {
            var folderPicker = new FolderPicker
                               {
                                   ViewMode = PickerViewMode.List,
                                   SuggestedStartLocation = PickerLocationId.ComputerFolder,
                                   FileTypeFilter = {"*"}
                               };

            return folderPicker.PickSingleFolderAsync();
        }

        #endregion
    }
}