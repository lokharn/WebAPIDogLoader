using DogLoader.Controllers;
using DogLoader.Interfaces;
using DogLoader.Models;
using Newtonsoft.Json.Linq;
using System.Net;

namespace DogLoader.Services
{
    public class HttpProcessing : IHttpProcessing
    {
        private readonly ILogger<DogLoaderController> _logger;

        public HttpProcessing(ILogger<DogLoaderController> logger)
        {
            _logger = logger;

            _logger.LogInformation("HttpProcessing service is running...");
        }

        public async Task<HttpWebResponse> MakeGetRequestToAPI(string apiLink) //Выполнить GET-запрос к API.
        {
            HttpWebResponse response;
            try
            {
                var httpWebRequest = WebRequest.Create(apiLink); //Создаёт запрос.
                httpWebRequest.ContentType = "text/json"; //Установка параметра для получения результатов в формате JSON.
                httpWebRequest.Method = "GET"; //Установка параметра типа запроса.
                response = (HttpWebResponse)httpWebRequest.GetResponse(); //Получить результат запроса.
            }
            catch (Exception ex)
            {
                throw new Exception($"{DateTime.Now}: Failed request to api: {ex.Message}");
            }
            return response;
        }
    }
}
