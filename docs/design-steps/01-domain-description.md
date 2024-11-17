# **Domain description**

The game "Three-in-a-Row" revolves around a player interacting with an 8x8 board filled with 5 types of elements. 
Player swaps adjacent cells to create matches (3 or more identical elements in a row or column). 
Valid moves result in matches being destroyed, followed by cells above falling, and new elements spawning to fill the empty spaces. 
The game tracks scores, bonuses, and statistics, ensuring a balance between randomness and playability.

---

## **Abstract Data Types (ADTs)**

### **1. Board**
- **Purpose**: Represents the 8x8 grid, managing its structure and operations.
- **Responsibilities**:
    - Maintain grid state (e.g., cell contents).
    - Enable cell swaps and clearings.
    - Abstract querying operations for other modules.

### **2. Combination Finder**
- **Purpose**: Detects and evaluates matching patterns on the board.
- **Responsibilities**:
    - Identify valid combinations (e.g., rows or columns).
    - Validate moves for potential matches.
    - Analyze grid state for continuity of gameplay.

### **3. Score Tracker**
- **Purpose**: Manages the scoring system.
- **Responsibilities**:
    - Calculate points for matches and bonuses.
    - Track the player’s current score.

### **4. Bonus**
- **Purpose**: Represents a specific bonus triggered by patterns or moves.
- **Responsibilities**:
    - Encapsulate bonus activation rules.
    - Define the effect on the board (e.g., clear rows or columns).

### **5. Bonus Manager**
- **Purpose**: Tracks and applies bonuses during gameplay.
- **Responsibilities**:
    - Monitor game events to trigger bonuses.
    - Apply active bonuses and manage their lifecycle.

### **6. Statistics Recorder**
- **Purpose**: Tracks game metrics and events.
- **Responsibilities**:
    - Log moves, bonuses, and scores.