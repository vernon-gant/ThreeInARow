### Mediator + Observer

The MediatrR .NET library provides a very convenient way to decouple the system components and also to implement the observer pattern. After validating
the user move we will emit events to notify other components. Thus the producers and consumers of the events are decoupled.

### Chain of Responsibility

The concrete flow of the game can be implemented usng the Chain of Responsibility pattern. The game flow is a sequence of steps that are executed in a specific order.
The only challenge is to decide how to pass the return value of the previous step to the next step. This can be done by using a context object that will be passed to each step.