using DogLoader.Controllers;
using DogLoader.Interfaces;
using DogLoader.Models;

namespace DogLoader.Services
{
    public class DogsLoadManager : IDogsLoadManager
    {
        private readonly ILogger<DogLoaderController> _logger;
        private readonly IDogsHttpLoader _dogsHttpLoader; //Сервис загрузки собак из интернета.
        /// <summary>
        /// Number of images uploaded.
        /// </summary>
        private int countOfLoadedPics { set; get; } //Количество загруженных изображений на текущий момент.
        /// <summary>
        /// The status of the download process.
        /// </summary>
        private bool onLoading { set; get; } //Статус фонового процесса: true - если процесс запущен, false - если процесс в ожидании.

        public DogsLoadManager(
            ILogger<DogLoaderController> logger,
            IDogsHttpLoader dogsHTTPLoader)
        {
            _logger = logger;
            _dogsHttpLoader = dogsHTTPLoader;

            _logger.LogInformation("DogsLoadManager service is running...");
        }

        public async Task LoadingManager(LoadingCommand command)
        {
            onLoading = true;
            countOfLoadedPics = 0;
            //Асинхронный стрим для получения прогресса загрузки собак. Отражает этот прогресс в классе менеджере.
            IAsyncEnumerable<int> loadingInLocalCache = _dogsHttpLoader.LoadDogsInLocalCacheAsync(command);
            try
            {
                await foreach (var image in loadingInLocalCache) //После каждой итерации загрузки одного изображение счётчик увеличивается на один.
                {                    
                    countOfLoadedPics++;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.Now}: DogsLoadManager service finished with errors in loading dogs. Error text: {ex.Message}");
            }
            _logger.LogInformation("Loading is finished.");
            onLoading = false;
        }

        public int GetCountOfLoadingImages() //Возврат количества загруженных изображений.
        {
            return countOfLoadedPics;
        }

        public bool GetOnLoadingStatus() //Возврат текущего статуса фонового процесса.
        {
            return onLoading;
        }
    }
}
