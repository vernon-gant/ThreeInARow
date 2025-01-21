# **Domain description**

Our goal is to create a 3 in a row game where we have a 8 to 8 board with elements which are represented by specific emojis. User can make moves which are swaps of adjacent elements. If elements have same emojis and are placed in a row of 3 elements they form a match. This match is deleted and new elements are added to the board. If new matches are detected after a drop of new elements we remove them and stabilize the board again.

Game ends when there are no possible moves on the board. User can also stop the game by entering appropriate command.



### ADTs


1. BoardLayout


This ADT is needed to operate with the positioning of elements on the board. We can swap elements, remove element from the layout, check if the layout is stable. When adding new elements to the board we can push an element to a column and shift columns down.

2. Match


Represents a sequence of elements which form a match. They all have same emojis. We can merge matches if two matches reference the same element. Can be either vertical or horizontal.

3. EmojiRegistry


Here we store all possible game emojis - this allows us to register them at the beginning and then get random emojis for generation of new elements.

4. MatchFinder


This ADT is responsible for finding matches on the board and also checking whether potential matches are found.

5. ScoreTracker


Used to track and update score. Offers operations like Add certain amount of points or double them.

6. BonusCondition


Describes a cituation when a bonus effect is produces. Exposes methods apply and reset.