using DogLoader.Controllers;
using DogLoader.Interfaces;
using DogLoader.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Nodes;
using static System.Net.Mime.MediaTypeNames;

namespace DogLoader.Services
{
    public class DogsHttpLoader : IDogsHttpLoader
    {
        private readonly IRedisHashLoader _hashLoader; //Сервис загрузки в хэш-таблицы Redis
        private readonly ILogger<DogLoaderController> _logger;
        private readonly IHttpBreedsFromAPI _httpBreeds; //Сервис загрузки списка пород
        private readonly IHttpDogsImagesByBreedFromAPI _httpDogsImages; //Сервис загрузки списка изображений по породе

        int maxCupOfImages;

        public DogsHttpLoader(
            IRedisHashLoader hashloader,
            ILogger<DogLoaderController> logger,
            IHttpBreedsFromAPI httpBreeds,
            IHttpDogsImagesByBreedFromAPI httpDogsImages)
        {
            _hashLoader = hashloader;
            _logger = logger;
            _httpBreeds = httpBreeds;
            _httpDogsImages = httpDogsImages;

            _logger.LogInformation("DogsHTTPLoader service is running...");
        }

        public async IAsyncEnumerable<int> LoadDogsInLocalCacheAsync(LoadingCommand loadingCommand) //Загрузка собак в локальное хранилище.
        {
            List<DogBreed> breeds = new List<DogBreed>(); //Предварительное объявление, чтобы воспользоваться в блоке исключений.
            maxCupOfImages = loadingCommand.Count; //Указание максимально возможной загрузки изображений каждой породы.

            try
            {
                if (loadingCommand.GetType().GetProperty("Breed") != null) //Проверка имеет ли команда значение породы.
                {
                    breeds.Add(new DogBreed { Name = ((LoadingBreedCommand)loadingCommand).Breed }); //Если имеется порода будут загружаться изображения по одной породе.
                }
                else
                {
                    breeds = await _httpBreeds.GetBreedsList(); //Если порода не указана, производится загрузка по всем доступным в API породам.
                }                
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.Now}: DogsHTTPLoader service finished with error in loading breeds. Error text: {ex.Message}");
                throw;
            }

            for (int i = 0; i < breeds.Count; i++) //Обрабатывает изображения по каждой породе в списке.
            {
                List<DogImage> images = new List<DogImage>(); //Предварительное объявление изображений для последующего заполнения в блоке исключения.
                try
                {
                    images = await _httpDogsImages.GetDogsImagesByBreedList(breeds[i]); //Получить из сервиса работы с API изображения по породе.
                }
                catch (Exception ex)
                {
                    _logger.LogError($"{DateTime.Now}: DogsHTTPLoader service finished with error in loading images. Error text: {ex.Message}");
                    throw;
                }
                //Обрабатываем полученный список изображений с ограничением на общее количество изображений и количество изображений доступных к загрузке.
                for (int j = 0; j < images.Count && j < maxCupOfImages; j++) 
                {
                    try
                    {                        
                        await _hashLoader.SetHashTableAsync(breeds[i].Name, images[j].ImageName, images[j].ImagePath); //Запись изображения в таблицу.
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError($"{DateTime.Now}: DogsHTTPLoader service finished with error in set hash. Error text: {ex.Message}");
                        throw;
                    }
                                       
                    _logger.LogInformation($"{DateTime.Now}: loading image {i + 1}.{j + 1}: {images[j].ImageName}");
                    yield return i;
                }
            }
        }
    }
}
