using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using DogLoader.Models;
using DogLoader.Services;
using DogLoader.Interfaces;
using StackExchange.Redis;
using Microsoft.Extensions.Hosting;
using static System.Net.Mime.MediaTypeNames;
using Microsoft.Extensions.Logging;

namespace DogLoader.Controllers
{
    /// <summary>
    /// Controller for executing user requests.
    /// </summary>
    [ApiController]
    [Route("dogloader")]
    public class DogLoaderController : ControllerBase
    {       
        private readonly ILogger<DogLoaderController> _logger; //Добавление возможности логирования.
        private readonly IDogsCommandProcessing _dogproc; //Добавление сервиса запускающего процесс обработки полученной команды.
        ProcessStatus loadingStatus; //Содержит в себе результат обработки для вывода пользователю
        /// <summary>
        /// Error codes for query responses.
        /// </summary>
        enum CustomErrorCodes
        {
            notFound = 0,
            postError,
            postByBreedError
        }
        /// <summary>
        /// Decryption of error codes for responses to requests.
        /// </summary>
        private static readonly string[] CustomErrodStrings = new[]
        {
            "It is impossible to process a command with such a set of parameters",
            "DogsController finished with error in POST. Error text: {0}",
            "DogsController finished with error in POST/byBreed. Error text: {0}"
        };
        public DogLoaderController(
            ILogger<DogLoaderController> logger,
            IDogsCommandProcessing dogproc)
        {
            _dogproc = dogproc;
            _logger = logger;

            _logger.LogInformation("DogLoader Controller is running...");
        }

        [HttpPost] //Контролер для загрузки всех пород.
        public async Task<ActionResult<LoadingCommand>> Post([FromBody] LoadingCommand command)
        {
            try
            {
                if (!ModelState.IsValid || command.Command != "run")
                    return BadRequest(command);
                //Проверка корректности полученной команды
                if (!CommandCheck(command))
                    return NotFound(CustomErrodStrings[(int)CustomErrorCodes.notFound]);
                //Запуск менеджера процесса загрузки изображений собак.
                //Метод возвращает два вида результата:
                //1) LoadingStatus - если процесс уже запущен. Статус содержит в себе значение "run" и количество загруженных изображений на данный момент.
                //2) ProcessStatus - если запущен новый процесс. Статус содержит  себе значение "ok".                            
                loadingStatus = await _dogproc.ProcessStatusManager(command); //Запуск сервиса обработки команды. Возращает текущий статус о работе.
            }
            catch (Exception ex)
            {
                _logger.LogError(CustomErrodStrings[(int)CustomErrorCodes.postError]);
                throw;
            }
            _logger.LogInformation("DogsController worked successfully.");
            
            return Ok(loadingStatus);
        }

        [HttpPost("byBreed")] //Контролер для загрузки по породе
        public async Task<ActionResult<LoadingCommand>> PostByBreed([FromBody] LoadingBreedCommand command)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(command);
                //Проверка корректности полученной команды
                if (!CommandCheck(command))
                    return NotFound(CustomErrodStrings[(int)CustomErrorCodes.notFound]);
                //Запуск менеджера процесса загрузки изображений собак.
                //Метод возвращает два вида результата:
                //1) LoadingStatus - если процесс уже запущен.
                //2) ProcessStatus - если запущен новый процесс.                             
                loadingStatus = await _dogproc.ProcessStatusManager(command);
            }
            catch (Exception ex)
            {
                _logger.LogError(CustomErrodStrings[(int)CustomErrorCodes.postByBreedError]);
                throw;
            }
            _logger.LogInformation("DogsController byBreed worked successfully.");
            return Ok(loadingStatus);
        }

        /// <summary>
        /// Checking the correctness of the entered command.
        /// </summary>
        /// <param name="command">The <see cref="LoadingCommand"/> or <see cref="LoadingBreedCommand"/> which is being checked.</param>
        /// <returns>Returns true if the command is valid.</returns>
        bool CommandCheck(LoadingCommand command)
        {            
            return command.Command == "run" && command.Count > 0; //Проверка чтобы команда была run и количество загружаемых сообщений больше нуля
        }
    }
}
