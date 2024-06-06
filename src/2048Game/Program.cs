using _2048Game.Models;
using _2048Game.Models.Abstractions;
using _2048Game.Services;
using _2048Game.Services.Abstractions;
using Microsoft.Extensions.DependencyInjection;

var serviceCollection = new ServiceCollection();

ConfigureServices();

var serviceProvider = serviceCollection.BuildServiceProvider();

var inputService = serviceProvider.GetRequiredService<InputService>();
var gameService = serviceProvider.GetRequiredService<IGameService>();

gameService.StartGame();
var isGameRunning = true;
while (isGameRunning)
{
    isGameRunning = inputService.HandleInput();
}

return;

void ConfigureServices()
{
    serviceCollection.AddSingleton<InputService>();
    serviceCollection.AddSingleton<IConsoleService, ConsoleService>();
    serviceCollection.AddSingleton<IGameService, GameService>();
    serviceCollection.AddSingleton<IRenderService, RenderService>();
    serviceCollection.AddSingleton<IStorageService, StorageService>();
    serviceCollection.AddSingleton<ITileControlService, TileControlService>();

    serviceCollection.AddSingleton<IBoard>(provider =>
        provider.GetRequiredService<IStorageService>().LoadGame().Item1 ?? new Board()
    );
    serviceCollection.AddSingleton<ScoreBoard>(provider =>
        provider.GetRequiredService<IStorageService>().LoadGame().Item2 ?? new ScoreBoard()
    );
}