using DogLoader.Configurations;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//Отдельный конфигуратор для внедрения зависимостей в собственных сервисах
builder.Services.AddDependenceInjectionConfig();
//Рекомендуется использовать диспетчер секретов для подключения к распределённому кэшу.
//В данном случае, в целях исключения локальных зависимостей, использются данные из строки подключения настроек приложения.
builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(builder.Configuration.GetConnectionString("Redis")));

var app = builder.Build();

//Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
