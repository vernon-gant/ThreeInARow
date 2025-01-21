### BoardLayout
This ADT allows us to manipulate the layout of the board where we can swap elements, shift elements down and fill the board with new elements.
This ADT is not responsible for detecting matches or scoring points. It is only responsible for the layout of the board.

### Match
Represents a match of elements on the board. A match is a sequence of three or more cells that form a valid match, either horizontally or vertically.
We can check if two matches intersect, merge two matches or check if they are sequential.

### EmojiRegistry
Used to store game emojis as we operate on the layout with emoji ids. This registry is used to map emoji ids to emojis.

### ScoreTracker
Simple ADT to keep track of the score. It is responsible for updating the score by adding points and resetting the score.

### Move
Represents a player's action of swapping two adjacent cells on the board.

### StatisticsUnit
Represents a single unit of statistics. It is used to track events and log them. Is characterized by a name, description and a value.

### MatchingStrategy
Defines the strategy for matching elements on the board. It is responsible for detecting matches and bonuses. As an example we may have
a vertical matching strategy and a horizontal matching strategy.