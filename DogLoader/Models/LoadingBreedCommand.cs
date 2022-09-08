namespace DogLoader.Models
{
    /// <summary>
    /// The command to load an image of a specific dog.
    /// </summary>
    public class LoadingBreedCommand : LoadingCommand
    {
        public string Breed { get; set; }
    }
}
