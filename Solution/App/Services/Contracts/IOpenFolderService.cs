using System.Threading.Tasks;

using Windows.Storage;

namespace BulkRenaming.Services.Contracts
{
    public interface IOpenFolderService
    {
        #region All other members

        Task<StorageFolder> PromptAsync();

        #endregion
    }
}