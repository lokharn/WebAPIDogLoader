using DogLoader.Models;
using System.Net;

namespace DogLoader.Interfaces
{
    /// <summary>
    /// Http request processing service.
    /// </summary>
    public interface IHttpProcessing
    {
        /// <summary>
        /// Makes an API request.
        /// </summary>
        /// <param name="apiLink">API <see cref="string"/> address.</param>
        /// <returns><see cref="HttpWebResponse"/> response for a request.</returns>
        Task<HttpWebResponse> MakeGetRequestToAPI(string apiLink);
    }
}
