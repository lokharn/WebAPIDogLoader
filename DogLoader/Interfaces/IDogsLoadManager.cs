using DogLoader.Models;

namespace DogLoader.Interfaces
{
    /// <summary>
    /// Image upload service.
    /// </summary>
    public interface IDogsLoadManager
    {
        /// <summary>
        /// Get the number of uploaded images so far.
        /// </summary>
        /// <returns>The number of <see cref="int"/> loaded images.</returns>
        int GetCountOfLoadingImages();
        /// <summary>
        /// Gets the current download status.
        /// </summary>
        /// <returns>True if the download is in progress, False if the download is not active.</returns>
        bool GetOnLoadingStatus();
        /// <summary>
        /// Manager of the active image upload process.
        /// </summary>
        /// <param name="command"></param>
        /// <returns><see cref="Task"/> that handles the loading process.</returns>
        Task LoadingManager(LoadingCommand command);
    }
}
