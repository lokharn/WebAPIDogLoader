namespace DogLoader.Models
{
    /// <summary>
    /// Storing the image of the dog and the name of the image.
    /// </summary>
    public class DogImage
    {
        public string ImagePath { get; set; }
        public string ImageName { get; set; }

        /// <summary>
        /// Constructor to automatically fill in image name data.
        /// </summary>
        /// <param name="path">The full link to fill in the name.</param>
        public DogImage(string path)
        {
            ImagePath = path;
            ImageName = Path.GetFileName(path); //Получить имя изображения из ссылки.
        }
    }
}
