using _2048Game.Services;
using _2048Game.Services.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace _2048Game;

[ExcludeFromCodeCoverage]
public static class Program
{
    public static void Main()
    {
        var serviceCollection = new ServiceCollection();

        ConfigureServices(serviceCollection);

        var serviceProvider = serviceCollection.BuildServiceProvider();

        var inputService = serviceProvider.GetRequiredService<IInputService>();

        inputService.StartGameAndListenInput();
    }

    private static void ConfigureServices(IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<IInputService, InputService>();
        serviceCollection.AddSingleton<IConsoleService, ConsoleService>();
        serviceCollection.AddSingleton<IGameService, GameService>();
        serviceCollection.AddSingleton<IRenderService, RenderService>();
        serviceCollection.AddSingleton<IStorageService, StorageService>();
        serviceCollection.AddSingleton<IBoardService>(provider =>
        {
            var (Tiles, ScoreBoard) = provider.GetRequiredService<IStorageService>().LoadGame();
            return new BoardService(Tiles, ScoreBoard);
        });
    }
}