using DogLoader.Models;

namespace DogLoader.Interfaces
{
    /// <summary>
    /// Load request command processing service.
    /// </summary>
    public interface IDogsCommandProcessing
    {
        /// <summary>
        /// Starts and checks the current status of the download process.
        /// </summary>
        /// <param name="command">The <see cref="LoadingCommand"/> or <see cref="LoadingBreedCommand"/> command is required to start the download.</param>
        /// <returns><see cref="ProcessStatus"/> or <see cref="LoadingStatus"/> download status depending on the result of processing.</returns>
        Task<ProcessStatus> ProcessStatusManager(LoadingCommand command);
    }
}
