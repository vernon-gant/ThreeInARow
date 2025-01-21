// See https://aka.ms/new-console-template for more information

using System.Reflection;

using MediatR;
using MediatR.Registration;

using Microsoft.Extensions.DependencyInjection;

using ThreeInARow.Board;
using ThreeInARow.Core;
using ThreeInARow.Game;
using ThreeInARow.GameEvents;
using ThreeInARow.Matching;
using ThreeInARow.Scoring;
using ThreeInARow.Statistics;

var services = new ServiceCollection();

// Register MediatR
var serviceConfig = new MediatRServiceConfiguration();
ServiceRegistrar.AddRequiredServices(services, serviceConfig);

// Register EmojiRegistry as Singleton
services.AddSingleton<EmojiRegistry>();

// Register BoardLayout with EmojiRegistry
services.AddSingleton(provider =>
{
    var emojiRegistry = provider.GetRequiredService<EmojiRegistry>();
    // Register game emojis
    emojiRegistry.RegisterGameEmoji("A");
    emojiRegistry.RegisterGameEmoji("B");
    emojiRegistry.RegisterGameEmoji("C");
    emojiRegistry.RegisterGameEmoji("D");
    emojiRegistry.RegisterGameEmoji("E");

    // Initialize BoardLayout with 8x8 grid
    return new BoardLayout(new Guid[8, 8]);
});

// Register Matching Strategies
var matchingStrategies = new List<MatchingStrategy>
{
    new HorizontalMatching(3),
    new VerticalMatching(3)
};

var matchingHandler = new MatchingHandler(matchingStrategies);

// Register Handlers for FindMatches and HasPotentialMatches
services.AddSingleton<IRequestHandler<HasPotentialMatches, bool>, MatchingHandler>(_ => matchingHandler);
services.AddTransient<IRequestHandler<FindMatches, List<Match>>, MatchingHandler>(_ => matchingHandler);

// Register Notification Handlers
services.AddSingleton<INotificationHandler<MatchFound>, MatchScoreHandler>();
services.AddSingleton<INotificationHandler<MatchFound>, MatchesRecorder>();
services.AddSingleton<INotificationHandler<MoveMade>, MovesMadeRecorder>();

// Register Statistics Units
services.AddSingleton<ScoreTracker>();
services.AddSingleton<StatisticUnit, MatchesRecorder>();
services.AddSingleton<StatisticUnit, MovesMadeRecorder>();

// Register Game
services.AddSingleton<Game>();

// Build ServiceProvider
var serviceProvider = services.BuildServiceProvider();

// Initialize and Run Game
var game = serviceProvider.GetRequiredService<Game>();
await game.RunAsync();

internal partial class Program;