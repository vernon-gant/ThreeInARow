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

### <ins>1. Board</ins>

The **Board subsystem** handles all operations related to the board layout. Position and Move are also included in this subsystem. Mechanism for adding new elements to the board is also included in this subsystem.
But no the generation, just the addition of new elements to the board.


### <ins>2. Match</ins>

The **Match subsystem** is responsible for detecting and managing matches of elements. Detects whether we need to merge a match or not.
Also delivers whether there are possible moves left on the board.


### <ins>3. Statistics</ins>

The **Statistics subsystem** records and manages gameplay statistics. Uses an event based approach to track and log events. Is not responsible for handling computations related to bonuses or other gameplay logic.
Mainly uses event based approach to track and log events.


### <ins>4. Score</ins>

The **Score subsystem** defines and executes scoring rules. For example that we get 10 points for each match of 3 elements.

- 10 points for each match of 3 elements.
- Bonuses triggered by matching 5 or more elements.

<br />

## **Domain**

### <ins>1. Board</ins>
- Represents the **matrix** where users can swap elements.

### <ins>2. Match</ins>
- A **horizontal or vertical sequence** of 3 or more matching elements on the board. These combinations emerge:
- After filling the board with new elements.
- As a result of a user swap.

### <ins>3. Score</ins>
- Represents the **user’s current score**, which increases when combinations or bonuses occur.

<br />

## **Functionality**

1. **Start a New Game**:
    - Users can start a new game by entering a specific command.

2. **Swapping Elements**:
    - Swaps are performed based on user input (e.g., entering `1 1` and `2 2` to swap elements at these coordinates).
    - Only **horizontal and vertical swaps of adjacent elements** are allowed.

3. **Scoring**:
    - The score increases when a match is detected or a bonus is triggered.

4. **Game Progression**:
    - The game continues until no valid combinations remain on the board.

<br />

## **Libraries**

1. **MediatR**:
    - A robust solution for implementing an **event-based approach** with notifications and handlers.
    - Suitable for managing **bonuses** and **statistics** subsystems.

2. **OneOf**:
    - Useful for handling **union types** (e.g., success or error states).
    - Can also be extended with custom types to manage state transitions and results effectively.