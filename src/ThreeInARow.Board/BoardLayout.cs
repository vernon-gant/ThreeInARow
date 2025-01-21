using ThreeInARow.Core;

namespace ThreeInARow.Board;

public class BoardLayout(Guid[,] emojiIds)
{
    public void Swap(Move move)
    {
        if (!move.IsWithinBounds(emojiIds.GetLength(0))) return;

        (emojiIds[move.From.Row, move.From.Column], emojiIds[move.To.Row, move.To.Column]) = (emojiIds[move.To.Row, move.To.Column], emojiIds[move.From.Row, move.From.Column]);
    }

    public void ShiftDown()
    {
        for (var column = 0; column < emojiIds.GetLength(1); column++)
        {
            var emptyRow = emojiIds.GetLength(0) - 1;

            for (var row = emojiIds.GetLength(0) - 1; row >= 0; row--)
            {
                if (emojiIds[row, column] == Guid.Empty)
                {
                    emptyRow = row;

                    continue;
                }

                (emojiIds[row, column], emojiIds[emptyRow, column]) = (emojiIds[emptyRow, column], emojiIds[row, column]);
                emptyRow--;
            }
        }
    }

    public void FillEmpty(EmojiRegistry emojiRegistry)
    {
        for (var row = 0; row < emojiIds.GetLength(0); row++)
        {
            for (var column = 0; column < emojiIds.GetLength(1); column++)
            {
                if (emojiIds[row, column] == Guid.Empty)
                {
                    emojiIds[row, column] = emojiRegistry.RandomGameEmoji;
                }
            }
        }
    }

    public void Delete(Position position)
    {
        emojiIds[position.Row, position.Column] = Guid.Empty;
    }

    public string[,] Render(EmojiRegistry emojiRegistry)
    {
        var rendered = new string[emojiIds.GetLength(0), emojiIds.GetLength(1)];

        for (var row = 0; row < emojiIds.GetLength(0); row++)
        {
            for (var column = 0; column < emojiIds.GetLength(1); column++)
            {
                if (emojiIds[row, column] == Guid.Empty)
                {
                    rendered[row, column] = " ";

                    continue;
                }

                rendered[row, column] = emojiRegistry.Render(emojiIds[row, column]);
            }
        }

        return rendered;
    }

    public Guid[,] View
    {
        get
        {
            var deepCopy = new Guid[emojiIds.GetLength(0), emojiIds.GetLength(1)];

            for (var row = 0; row < emojiIds.GetLength(0); row++)
            {
                for (var column = 0; column < emojiIds.GetLength(1); column++)
                {
                    deepCopy[row, column] = emojiIds[row, column];
                }
            }

            return deepCopy;
        }
    }
}