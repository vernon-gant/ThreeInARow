# **Domain description**

Our goal is to create a game where the player has to match emojis on a grid. The game will have a score system that rewards the player for matching emojis. The player can swap emojis to create matches and the game will automatically add new emojis to the board. The game ends when there are no more possible matches.

We will operate with element ids on the board layout as it allows us to add behavior to the elements. For example, we can add a bomb element that explodes when matched, destroying nearby elements.

---

## **Abstract Data Types (ADTs)**

### **1. BoardLayout**
- **Purpose**: Represents the grid and is only responsible for the layout of the board. It does not contain any information about the contents of the cells.
- **Responsibilities**:
    - Swap cells and rollback if no match is found.
    - Shift columns and add new cells to the board.
    - Build view to allow other components to access the grid.

### **2. Match**
- **Purpose**: Represents a valid match of emojis on the board.
- **Responsibilities**:
    - Store the cells that form the match.
    - Merge matches if they share cells.

### **3. MatchingStrategy**
- **Purpose**: Defines the rules for matching patterns.
- **Responsibilities**:
    - Find matches like vertical, horizontal, and L-shape.
    - Return the matches found on the board.

### **4. ScoreTracker**
- **Purpose**: Keeps track of the player's score.
- **Responsibilities**:
    - Update score based on matches.
    - Provide score information to the user interface.

### **5. EmojiRegistry**
- **Purpose**: Manages the emoji assets used in the game.
- **Responsibilities**:
    - Register game emojis.
    - Store which element is mapped to which emoji.

### **6. ElementGenerator**
- **Purpose**: Generates random elements for the board.
- **Responsibilities**:
    - Generate random elements for the board.