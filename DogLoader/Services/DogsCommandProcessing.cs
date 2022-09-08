using DogLoader.Interfaces;
using DogLoader.Models;
using DogLoader.Controllers;
using Microsoft.Extensions.Caching.Distributed;

namespace DogLoader.Services
{
    public class DogsCommandProcessing : IDogsCommandProcessing
    {
        private readonly ILogger<DogLoaderController> _logger;
        private readonly IDogsHttpLoader _dogsHttpLoader;
        private readonly IDogsLoadManager _loadManager; //Сервис управления загрузкой
        private int countOfLoadedPics { set; get; }
        private bool onLoading { set; get; }
        enum CustomResponseCodes 
        {
            ok = 0,
            run
        }
        private static readonly string[] CustomResponseStrings = new[]
        {
            "ok", 
            "run"
        };

        public DogsCommandProcessing(
            ILogger<DogLoaderController> logger,
            IDogsHttpLoader dogsHTTPLoader,
            IDogsLoadManager loadManager)
        {
            _logger = logger;
            _loadManager = loadManager;

            _logger.LogInformation("DogsProcessing service is running...");
        }

        public async Task<ProcessStatus> ProcessStatusManager(LoadingCommand command) //Обрабатывает команду на загрузку для обычного POST и для POST по породам
        {
            if (_loadManager.GetOnLoadingStatus()) //Проверяет производится ли загрузка в данный момент
            {                
                _logger.LogInformation("Process running.");
                return await GetRunStatus(); //Возвращает статус "run" и количество загруженных изображений
            }
            else
            {
                Task.Run(() => _loadManager.LoadingManager(command)); //Запуск фонового процесса загрузки изображений
                _logger.LogInformation("Load command accepted for processing.");
                return await GetOkStatus(); //Возвращает статус "ok" и количество загруженных изображений
            }            
        }

        /// <summary>
        /// Get the OK status for the message.
        /// </summary>
        /// <returns><see cref="ProcessStatus"/> object describing the OK message.</returns>
        async Task<ProcessStatus> GetOkStatus() => new ProcessStatus { Status = CustomResponseStrings[(int)CustomResponseCodes.ok] };
        /// <summary>
        /// Get the RUN status for the message with the number of uploaded images parameter..
        /// </summary>
        /// <returns><see cref="LoadingStatus"/> object describing the RUN message.</returns>
        async Task<LoadingStatus> GetRunStatus() => new LoadingStatus { Status = CustomResponseStrings[(int)CustomResponseCodes.run], Count = _loadManager.GetCountOfLoadingImages() };
    }
}
