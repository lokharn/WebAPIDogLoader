using DogLoader.Models;

namespace DogLoader.Interfaces
{
    /// <summary>
    /// Dog download service via http.
    /// </summary>
    public interface IDogsHttpLoader
    {
        /// <summary>
        /// Uploading Dogs to Local Redis Storage.
        /// </summary>
        /// <param name="loadingCommand">The <see cref="LoadingCommand"/> or <see cref="LoadingBreedCommand"/> command is required to start the download.</param>
        /// <returns>Asynchronous stream that reflects the loading process.</returns>
        IAsyncEnumerable<int> LoadDogsInLocalCacheAsync(LoadingCommand loadingCommand);
    }
}
