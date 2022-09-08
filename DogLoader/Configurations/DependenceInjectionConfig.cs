using DogLoader.Services;
using DogLoader.Interfaces;
using DogLoader.Models;
using StackExchange.Redis;

namespace DogLoader.Configurations
{
    /// <summary>
    /// Di-injection configurator.
    /// </summary>
    public static class DependenceInjectionConfig
    {
        /// <summary>
        /// Adding dependencies to the list of services.
        /// </summary>
        /// <returns><see cref="IServiceCollection"/> collection.</returns>
        public static IServiceCollection AddDependenceInjectionConfig(this IServiceCollection services)
        {
            //Добавление как Singleton т.к. необходимо сохранить свойства в последующих обращениях
            services.AddSingleton<IDogsCommandProcessing, DogsCommandProcessing>();
            services.AddSingleton<IDogsLoadManager, DogsLoadManager>();
            services.AddSingleton<IRedisHashLoader, RedisHashLoader>();
            //Добавление как Transient т.к. нет необходимости хранить данные о состоянии
            services.AddTransient<IDogsHttpLoader, DogsHttpLoader>();
            services.AddTransient<IHttpProcessing, HttpProcessing>();
            services.AddTransient<IHttpDogsImagesByBreedFromAPI, HttpDogsImagesByBreedFromAPI>();
            services.AddTransient<IHttpBreedsFromAPI, HttpBreedsFromAPI>();

            return services;
        }
    }
}
