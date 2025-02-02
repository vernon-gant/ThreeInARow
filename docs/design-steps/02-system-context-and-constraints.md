﻿## **System Context**

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

The **Board subsystem** handles all operations related to the board layout. Position and Move are also included in this subsystem.


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

1. Match - a sequence of three or more cells that form a valid match, either horizontally or vertically.
2. Emoji - representation of a single cell on the board.
3. Score - amount of points earned by the player.
4. Bonus - special actions triggered by specific events.
5. Move - a player's action of swapping two adjacent cells on the board.
6. Board - the game board as a collection of positions and states.

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
- Used to implement the **Mediator pattern** for decoupling components.
- Supports **request/response** and **notification** patterns for handling user input and system events.

2. **OneOf**:
- Useful for handling **union types** (e.g., success or error states).
- Can also be extended with custom types to manage state transitions and results effectively.