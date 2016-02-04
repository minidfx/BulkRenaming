using System.Threading.Tasks;
using Windows.Storage;

namespace App.Services.Contracts
{
    public interface IOpenFolderService
    {
        Task<StorageFolder> PromptAsync();
    }
}