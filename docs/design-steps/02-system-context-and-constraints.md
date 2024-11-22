## **System Context**

Our game includes **two main operations** at the highest level:

1. **Start a new game** and play.
2. **Exit the application.**

We will not track statistics between games, only statistics **within a single game**. The game will include a **console-based UI**, with the following constraints:

1. **Elements Representation**:
    - Elements must be encoded using a standard like UTF-16.
2. **No Asynchronous Behavior**:
    - The console does not support asynchronous input/output.
3. **User Input**:
    - User interactions will be managed through console input.
4. **System Feedback**:
    - All system responses will be printed on the console.

**Game results** will not be persisted, and statistics will only be displayed at the end of the game. The game will not include a timer, and users can exit by entering a specific character. The game will automatically end when no more valid moves are possible.

## **Main Subsystems**

### <ins>1. Grid</ins>

The **Grid subsystem** handles all operations related to the game grid and its elements.

- **Elements**:
    - Elements have a **visual representation** in standard format (e.g., "A") and in a modified format (e.g., strike-through or empty) when part of a match and removed from the grid.

- **Layers of Abstraction**:
    1. **Modifiable Grid**:
        - Offers operations like `Swap`, `DeleteRow`, and `DeleteColumn`.
    2. **Grid Snapshot**:
        - Provides APIs for examining elements in their **neighborhood context**, enabling decoupled investigation of possible combinations or valid moves.

- **Element Generator**:
    - Responsible for generating new elements to fill the grid when combinations are removed.


### <ins>2. Combination</ins>

The **Combination subsystem** inspects the **Grid Snapshot** to detect combinations.

- **Responsibilities**:
    - Implements the logic for detecting combinations (e.g., horizontal and vertical matches only).
    - Validates user moves by analyzing whether they produce a valid combination.
    - Evaluates the element context to determine if moves are valid.


### <ins>3. Statistics</ins>

The **Statistics subsystem** records and manages gameplay statistics.

- **Event-Based Approach**:
    - Notifications and their respective consumers are initialized at the start of a new game, updated during gameplay, and disposed of at the end.

- **Responsibilities**:
    - Records game events (e.g., moves, combinations, bonuses).
    - Does not handle computations related to bonuses or other gameplay logic—its sole focus is tracking and logging events.


### <ins>4. Bonuses</ins>

The **Bonuses subsystem** defines and executes bonus rules.

- **Examples**:
    - Bonuses triggered by matching 5 or more elements.

- **Event-Based Approach**:
    - Events and their contexts are evaluated to determine whether a bonus action should be applied.



## **Domain**

### <ins>1. Grid</ins>
- Represents the **matrix** where users can swap elements.

### <ins>2. Combination</ins>
- A **horizontal or vertical sequence** of 3 or more matching elements on the board. These combinations emerge:
    - After filling the board with new elements.
    - As a result of a user swap.

### <ins>3. Score</ins>
- Represents the **user’s current score**, which increases when combinations or bonuses occur.



## **Functionality**

1. **Start a New Game**:
    - The user can swap elements on the grid to form combinations.

2. **Swapping Elements**:
    - Swaps are performed based on user input (e.g., entering `1 1` and `2 2` to swap elements at these coordinates).
    - Only **horizontal and vertical swaps of adjacent elements** are allowed.

3. **Scoring**:
    - The score increases when:
        - A combination occurs.
        - A bonus condition is triggered.

4. **Game Progression**:
    - The game continues until no valid combinations remain on the board.


## **Libraries**

1. **MediatR**:
    - A robust solution for implementing an **event-based approach** with notifications and handlers.
    - Suitable for managing **bonuses** and **statistics** subsystems.

2. **OneOf**:
    - Useful for handling **union types** (e.g., success or error states).
    - Can also be extended with custom types to manage state transitions and results effectively.
