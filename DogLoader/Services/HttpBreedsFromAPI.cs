using DogLoader.Controllers;
using DogLoader.Interfaces;
using DogLoader.Models;
using Newtonsoft.Json.Linq;
using System.Net;

namespace DogLoader.Services
{
    public class HttpBreedsFromAPI : IHttpBreedsFromAPI
    {
        private readonly ILogger<DogLoaderController> _logger;
        private readonly IHttpProcessing _httpProcessing; //Сервис обработки веб-запросов

        public HttpBreedsFromAPI(ILogger<DogLoaderController> logger, IHttpProcessing httpProcessing)
        {
            _logger = logger;
            _httpProcessing = httpProcessing;

            _logger.LogInformation("HttpBreedsFromAPI service is running...");            
        }

        public async Task<List<DogBreed>> GetBreedsList()
        {
            var httpResponse = await _httpProcessing.MakeGetRequestToAPI("https://dog.ceo/api/breeds/list/all"); //Выполнить обращение к API для получения всех пород.
            var resultListWithBreeds = ParseJSONBreedsToList(httpResponse); //Преобразовать полученный ответ о породах в список.

            if (!resultListWithBreeds.Any())
            {
                _logger.LogError($"{DateTime.Now}: HttpBreedsFromAPI service finished with empty list.");
                throw new Exception("Unable to load breeds. API http-request return a null value.");
            }
            return resultListWithBreeds;
        }

        /// <summary>
        /// Parsing JSON breed response into a List.
        /// </summary>
        /// <param name="httpResponse"><see cref="HttpWebResponse"/> result.</param>
        /// <returns>The <see cref="List{T}"/> of <see cref="DogBreed"/> converted from JSON.</returns>
        static List<DogBreed> ParseJSONBreedsToList(HttpWebResponse httpResponse) //Преобразовать результат запроса в список пород.
        {
            var resultList = new List<DogBreed>();

            using (StreamReader r = new StreamReader(httpResponse.GetResponseStream())) //Создаёт чтение из потока.
            {
                var json = r.ReadToEnd(); //Получить сразу весь текст запроса.
                JObject breedResponse = JObject.Parse(json); //Преобразование текста в набор JSON объектов.

                //Получить по токену message, в котором хранятся результаты запроса, потомков, содержащих в себе породы.
                //После этого производится преобразование в JSON-объект "Свойство" и в список.
                var breedResponseCollection = breedResponse.SelectToken("message").Children().OfType<JProperty>().ToList();
                foreach (var breedProperty in breedResponseCollection) //Получая каждое свойство в отдельности, преообразует его в породу собаки.
                {
                    resultList.Add(new DogBreed { Name = breedProperty.Name });
                }
            }
            return resultList;
        }
    }
}
