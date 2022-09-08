namespace DogLoader.Models
{
    /// <summary>
    /// The command to upload images of dogs of all breeds.
    /// </summary>
    public class LoadingCommand
    {
        public string? Command { get; set; }
        public int Count { get; set; }
    }
}
