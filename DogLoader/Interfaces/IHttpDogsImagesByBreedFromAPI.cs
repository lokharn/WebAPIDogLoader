using DogLoader.Models;

namespace DogLoader.Interfaces
{
    /// <summary>
    /// Service for downloading a list of breed images from the API.
    /// </summary>
    public interface IHttpDogsImagesByBreedFromAPI
    {
        /// <summary>
        /// Calls to the API to download the list of images by breed.
        /// </summary>
        /// <param name="breed"><see cref="DogBreed"/> of dog</param>
        /// <returns>List of actual images of dogs by breed.</returns>
        Task<List<DogImage>> GetDogsImagesByBreedList(DogBreed breed);
    }
}
