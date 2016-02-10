using System;
using System.Threading.Tasks;

using Windows.Storage;
using Windows.Storage.Pickers;

using BulkRenaming.Services.Contracts;

namespace BulkRenaming.Services
{
    public class OpenFolderService : IOpenFolderService
    {
        #region Interface Implementations

        public async Task<StorageFolder> PromptAsync()
        {
            var folderPicker = new FolderPicker
                               {
                                   ViewMode = PickerViewMode.Thumbnail,
                                   SuggestedStartLocation = PickerLocationId.Desktop,
                                   FileTypeFilter = {"."}
                               };

            return await folderPicker.PickSingleFolderAsync();
        }

        #endregion
    }
}