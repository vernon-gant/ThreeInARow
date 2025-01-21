## 1. Board

### Commands

1. `Swap` - swap two cells on the board
    - precondition: `when cells are adjacent and in bounds of the board` 
    - postcondition: `the two cells are swapped`
2. `ShiftDown` - shift all columns with empty cells down
    - precondition: `there are empty cells on the board`
    - postcondition: `all columns with empty cells are shifted down`
3. `FillEmpty` - fill empty cells with random values
    - precondition: `there are empty cells on the board`
    - postcondition: `all empty cells are filled with random values`
4. `Delete` - remove an element from the board
    - precondition: `the cell is not empty`
    - postcondition: `the cell is empty`

### Queries

5. `View` - returns the read-only view of the board emoji ids
6. `Render` - returns the visual representation of the board

---

## 2. Move

### Queries

1. **`IsWithinBounds(dimensions)`** - Determines if the move is within the bounds of the board.

2. **`IsAdjacent()`** - Determines if the move involves swapping adjacent positions.

---

## 3. Match

### Commands

1. **`Merge(otherMatch)`** - Merges the current match with another match.
    - **Precondition**: `otherMatch` intersects or is adjacent to the current match.

2 **`Intersects(otherMatch)`** - Checks if the current match intersects with another match.

3 **`IsAdjacentTo(otherMatch)`** - Determines if the current match is adjacent to another match.


---

## 4. EmojiRegistry

### Commands

1. **`RegisterGameEmoji(emoji)`** - Registers a new emoji with a unique identifier.
    - **Precondition**: `emoji` is a non-empty string.
    - **Postcondition**: Adds a new entry to `_emojiIdToEmoji` with a unique `Guid` as the key and `emoji` as the value.

### Queries

1. **`GetRandomEmoji()`** - Returns a random emoji from the registry.

---

## 6. MatchingStrategy *(Abstract Class)*

### Queries

1. **`FindMatches(emojiIds)`** - Identifies matches on the board based on the specific strategy.
    - **Precondition**: `emojiIds` does not contain any empty cells.

2. **`HasPotentialMatch(emojiIds)`** - Checks if there exists at least one potential match based on the strategy.
    - **Precondition**: `emojiIds` does not contain any empty cells.