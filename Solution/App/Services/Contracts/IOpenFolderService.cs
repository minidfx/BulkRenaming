using Windows.Foundation;
using Windows.Storage;

namespace BulkRenaming.Services.Contracts
{
    public interface IOpenFolderService
    {
        #region All other members

        IAsyncOperation<StorageFolder> PromptAsync();

        #endregion
    }
}