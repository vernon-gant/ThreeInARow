### MatchingStrategy

Implements strategy pattern to define the strategy for matching elements on the board. It is responsible for detecting matches and bonuses. As an example we may have a vertical matching strategy and a horizontal matching strategy.
We can also have a strategy that detects matches of a specific length, or a strategy that detects matches of a specific shape.

### Mediator

Is delivered by the MediatR library. It is used to implement the Mediator pattern for decoupling components.

### Observer pattern

Is not represented by a single class rather we use the same mediatr library for sending events and notifications. The observer pattern is used to notify other components about events that have occurred.
For example, when a match is detected, we can notify the score tracker to update the score.