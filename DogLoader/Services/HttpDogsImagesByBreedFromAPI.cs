using DogLoader.Controllers;
using DogLoader.Interfaces;
using DogLoader.Models;
using Newtonsoft.Json.Linq;
using System.Net;

namespace DogLoader.Services
{
    public class HttpDogsImagesByBreedFromAPI : IHttpDogsImagesByBreedFromAPI
    {
        private readonly ILogger<DogLoaderController> _logger;
        private readonly IHttpProcessing _httpProcessing; //Сервис обработки веб-запросов

        public HttpDogsImagesByBreedFromAPI(ILogger<DogLoaderController> logger, IHttpProcessing httpProcessing)
        {
            _logger = logger;
            _httpProcessing = httpProcessing;

            _logger.LogInformation("HttpDogsImagesByBreedFromAPI service is running...");
        }

        public async Task<List<DogImage>> GetDogsImagesByBreedList(DogBreed breed) //Получить список изображений по породе из API.
        {
            var httpResponse = await _httpProcessing.MakeGetRequestToAPI($"https://dog.ceo/api/breed/{breed.Name}/images"); //Выполнить обращение к API для получения изображений по породе.
            var resultListWithImages = ParseJSONBreedImagesToList(httpResponse); //Преобразовать полученный ответ об изображениях в список.

            if (!resultListWithImages.Any()) //Проверка списка на пустоту.
            {
                _logger.LogError($"{DateTime.Now}: HttpDogsImagesByBreedFromAPI service finished with empty list.");
                throw new Exception("Unable to load images. API http-request return a null value.");
            }
            return resultListWithImages;
        }

        /// <summary>
        /// Parsing JSON image breed response into a List.
        /// </summary>
        /// <param name="httpResponse"><see cref="HttpWebResponse"/> result.</param>
        /// <returns>The <see cref="List{T}"/> of <see cref="DogImage"/> converted from JSON.</returns>
        static List<DogImage> ParseJSONBreedImagesToList(HttpWebResponse httpResponse) //Преобразовать результат запроса в список изображений собак.
        {
            var resultList = new List<DogImage>();

            using (StreamReader r = new StreamReader(httpResponse.GetResponseStream())) //Создаёт чтение из потока.
            {
                var json = r.ReadToEnd(); //Получить сразу весь текст запроса.
                JObject imagesResponse = JObject.Parse(json); //Преобразование текста в набор JSON объектов.

                //Получить по токену message, в котором хранятся результаты запроса, потомков, содержащих в себе ссылки на изображения.
                var imagesResponseCollection = imagesResponse.SelectToken("message").Children();
                foreach (var imageProperty in imagesResponseCollection) //Получая каждое свойство в отдельности, преобразует его в объект "Изображение собаки"
                {
                    resultList.Add(new DogImage(imageProperty.ToString())); //Добавляет объект через конструктор, который заполняет имя изображения, извлекая его из ссылки.
                }
            }
            return resultList;
        }
    }
}
