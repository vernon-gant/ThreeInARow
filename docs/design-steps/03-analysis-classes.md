# Matching Layer

### Match
Represents a sequence of three or more cells that form a valid match, either horizontally or vertically.

### Cell
Represents a single conceptual cell, independent of its position on the board.

### CellSequence
Encapsulates a list of cells used to detect matches or potential matches.

### MatchingContext
Provides a high-level abstraction for detecting matches and potential matches in the current context.

---

# Board Layer

### Grid
Represents the game board as a collection of positions and states.

### BoardElement
Represents a visual element on the game board with attributes like type and rendering properties.

---

# Statistics Layer

### Score
Tracks the player's current score and supports scoring operations.

### Move
Represents a player's action of swapping two adjacent cells on the board.

---

# Bonuses Layer

### MatchesRegistry
Maintains a registry of all detected matches for use by bonus handlers.

### MatchesNotificationHandler
Handles notifications related to matches and triggers relevant bonus actions.

### FiftyMovesHandler
Triggers a special bonus by refreshing the board after 50 moves.


StableBoard

Represents a board where all elements are stable, and no empty cells exist.
Operations:
HasPotentialMatches
Swap
ClearOfType
Matches
ClearMatches
ToFillableBoard
FillableBoard

Responsible for filling empty cells in a board, conceptually adding new elements.
Operations:
WithOneNew – Add exactly one new element.
WithRandom – Fill empty cells with random new elements.
WithZeroNew – No new elements are added.
This allows incremental control of how new elements enter the board.
FallingColumnsBoard

Represents a board with falling elements. Focuses on incremental shifting of elements and dropping new ones as needed.
Operations:
ShiftColumns – Shift all columns downward to fill empty cells.
ShiftWithImmediateNew – Shift columns while immediately adding new elements at the top.
FillNextEmpty – Add a new element only to the next free position.
DropNewToNextFree – Drop new elements to the next free positions in a falling manner.