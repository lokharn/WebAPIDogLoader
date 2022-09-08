using DogLoader.Models;

namespace DogLoader.Interfaces
{
    /// <summary>
    /// Service for downloading a list of breeds from the API.
    /// </summary>
    public interface IHttpBreedsFromAPI
    {
        /// <summary>
        /// Calls to the API to download the list of breeds.
        /// </summary>
        /// <returns>List of actual breeds of dogs.</returns>
        Task<List<DogBreed>> GetBreedsList();
    }
}
