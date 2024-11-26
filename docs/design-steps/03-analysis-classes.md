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

