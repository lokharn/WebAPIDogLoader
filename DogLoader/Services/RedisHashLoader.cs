using DogLoader.Controllers;
using DogLoader.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using StackExchange.Redis; //Используется данный Redis клиент по причине удобного заполнения хэш-таблиц.

namespace DogLoader.Services
{
    public class RedisHashLoader : IRedisHashLoader
    {
        private readonly ILogger<DogLoaderController> _logger;
        private readonly IConnectionMultiplexer _connectionMultiplexer; //Сервис соединения к локальным хранилищем Redis.
        IDatabase database;
        public RedisHashLoader(IConnectionMultiplexer multiplexer,
            ILogger<DogLoaderController> logger)
        {
            _connectionMultiplexer = multiplexer;
            _logger = logger;
            database = _connectionMultiplexer.GetDatabase(); //Получить локальную базу данных по заданному соединению. 
            _logger.LogInformation("RedisHashLoader service is running...");
        }

        public async Task SetHashTableAsync(string tablename, string hashkey, string hashvalue) //Ассинхронное заполнение хэш-таблицы.
        {
            //Заполнить как хеш-таблицу, где: имя таблицы - имя породы, ключ - наименование изображения, значение - полная ссылка на изображение.
            database.HashSetAsync(tablename, hashkey, hashvalue).Wait();
        }

    }
}
