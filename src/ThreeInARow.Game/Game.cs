using MediatR;

using ThreeInARow.Board;
using ThreeInARow.Core;
using ThreeInARow.GameEvents;
using ThreeInARow.Matching;
using ThreeInARow.Statistics;

namespace ThreeInARow.Game;

public class Game(IMediator mediator, BoardLayout boardLayout, IEnumerable<StatisticUnit> statistics, EmojiRegistry emojiRegistry)
{
    public async Task RunAsync()
    {
        PrintWelcomeMessage();

        // Process any initial matches
        boardLayout.FillEmpty(emojiRegistry);

        await InitialShuffle();

        await RenderBoard();

        while (true)
        {
            bool hasMoves = await CheckPossibleMoves();

            if (!hasMoves)
            {
                Console.WriteLine("No more possible moves. Game Over!");

                break;
            }

            var move = GetUserMove();

            if (move == null)
            {
                Console.WriteLine("Exiting game.");

                break;
            }

            if (!ValidateMove(move.Value))
            {
                Console.WriteLine("Invalid move. Please try again.");

                continue;
            }

            ExecuteMove(move.Value);
            await mediator.Publish(new MoveMade());

            var matches = await FindAndProcessMatches();

            if (matches.Count != 0) continue;

            boardLayout.Swap(move.Value);
            Console.WriteLine("No matches created. Move reverted.");
            await RenderBoard();
        }

        PrintStatistics();
    }

    private void PrintWelcomeMessage()
    {
        Console.WriteLine("===================================");
        Console.WriteLine("        Welcome to Three in a Row!  ");
        Console.WriteLine("===================================");
        Console.WriteLine("Instructions:");
        Console.WriteLine("1. The board is 8x8, filled with emojis (A, B, C, D, E).");
        Console.WriteLine("2. Swap adjacent emojis to form a row or column of three or more identical emojis.");
        Console.WriteLine("3. Enter moves as 'A1 A2' to swap elements at positions A1 and A2.");
        Console.WriteLine("4. Type 'exit' to quit the game at any time.");
        Console.WriteLine();
    }

    private async Task RenderBoard()
    {
        var rendered = boardLayout.Render(emojiRegistry);
        Console.WriteLine("Current Board:");

        for (int row = 0; row < rendered.GetLength(0); row++)
        {
            for (int column = 0; column < rendered.GetLength(1); column++)
            {
                Console.Write($"{rendered[row, column]} ");
            }

            Console.WriteLine();
        }

        Console.WriteLine();
    }

    private async Task InitialShuffle()
    {
        var matches = await mediator.Send(new FindMatches
                                          {
                                              EmojiIds = boardLayout.View
                                          });

        while (matches.Count > 0)
        {
            matches.ForEach(match => match.Elements.ForEach(boardLayout.Delete));
            boardLayout.FillEmpty(emojiRegistry);
            matches = await mediator.Send(new FindMatches
                                          {
                                              EmojiIds = boardLayout.View
                                          });
        }
    }

    private async Task<bool> CheckPossibleMoves()
    {
        return await mediator.Send(new HasPotentialMatches
                                   {
                                       EmojiIds = boardLayout.View
                                   });
    }

    private Move? GetUserMove()
    {
        Console.Write("Enter your move (e.g., A1 A2) or 'exit': ");
        var input = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(input))
        {
            Console.WriteLine("Invalid input.");

            return GetUserMove();
        }

        if (input.Trim().ToLower() == "exit")
        {
            return null;
        }

        var parts = input.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);

        if (parts.Length != 2)
        {
            Console.WriteLine("Invalid input format.");

            return GetUserMove();
        }

        try
        {
            var from = ParsePosition(parts[0]);
            var to = ParsePosition(parts[1]);

            return new Move(from, to);
        }
        catch
        {
            Console.WriteLine("Invalid position. Please use format like 'A1'.");

            return GetUserMove();
        }
    }

    private Position ParsePosition(string input)
    {
        input = input.ToUpper();

        if (input.Length < 2)
            throw new FormatException();

        char rowChar = input[0];

        if (!char.IsLetter(rowChar))
            throw new FormatException();

        if (!int.TryParse(input.Substring(1), out int column))
            throw new FormatException();

        int row = rowChar - 'A';
        int col = column - 1; // Assuming columns are 1-indexed

        return new Position(row, col);
    }

    private bool ValidateMove(Move move)
    {
        // Ensure move is within bounds and adjacent
        return move.IsWithinBounds(boardLayout.View.GetLength(0)) && move.IsForAdjacent;
    }

    private void ExecuteMove(Move move)
    {
        boardLayout.Swap(move);
    }

    private async Task<List<Match>> FindAndProcessMatches()
    {
        var matches = await mediator.Send(new FindMatches
                                          {
                                              EmojiIds = boardLayout.View
                                          });

        if (matches.Count > 0)
        {
            foreach (var match in matches)
            {
                await mediator.Publish(new MatchFound(match));
            }

            matches.ForEach(match => match.Elements.ForEach(boardLayout.Delete));

            await RenderBoard();

            await ShiftAndFill();
        }

        return matches;
    }

    private async Task ShiftAndFill()
    {
        boardLayout.ShiftDown();
        await RenderBoard();

        boardLayout.FillEmpty(emojiRegistry);
        await RenderBoard();
    }

    private void PrintStatistics()
    {
        Console.WriteLine("===================================");
        Console.WriteLine("            Game Statistics        ");
        Console.WriteLine("===================================");

        foreach (var stat in statistics)
        {
            Console.WriteLine($"{stat.Name}: {stat.Value}");
        }

        Console.WriteLine("===================================");
        Console.WriteLine("Thank you for playing!");
    }
}