### **1. Matching Cluster**
- **Purpose**: Handles all operations related to identifying and processing matches on the game board.
- **Classes**:
    - `Match`: Represents a sequence of three or more cells forming a valid match.
    - `CellSequence`: Encapsulates a list of cells used to detect matches or potential matches.
    - `Cell`: Represents a conceptual cell without a fixed position on the board.

---

### **2. Board Cluster**
- **Purpose**: Manages the structure and operations of the game board.
- **Classes**:
    - `Grid`: Represents the game board and its elements.
    - `BoardElement`: Encapsulates a visual element on the board, such as its type and rendering properties.

---

### **3. Statistics Cluster**
- **Purpose**: Tracks and manages gameplay statistics.
- **Classes**:
    - `Score`: Tracks the player's score and provides operations for updating it.

---

### **4. Bonus Cluster**
- **Purpose**: Defines and manages bonus behaviors triggered by gameplay events.
- **Classes**:
    - `MatchesRegistry`: Keeps track of all matches for use by bonus handlers.
    - `MatchesNotificationHandler`: Listens for match events and triggers relevant bonus actions.

---