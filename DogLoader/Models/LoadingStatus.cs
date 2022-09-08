using System.ComponentModel.DataAnnotations;

namespace DogLoader.Models
{
    /// <summary>
    /// Process status with the number of images loaded.
    /// </summary>
    public class LoadingStatus : ProcessStatus
    {
        public int? Count { get; set; }
    }
}
