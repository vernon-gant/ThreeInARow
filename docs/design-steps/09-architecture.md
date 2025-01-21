## Elements generation

New elements are generated using the EmojiRegistry which is prepopulated at the beginning of the game with the emojis we want to operate on.
BoardLayout takes as parameter the EmojiRegistry and fills the board with new elements. However this happens only after we delete elements from
the layout.

## Elements deletion

After a user makes a swap we check using the MatchingFinder if there are any matches. If there are, we delete the elements and fill the board with new ones.
We also emit events that a new match has been found and that the score should be updated + a new move made event is emitted for statistics and maybe bonuses.

## Statistics

We record moves made, matches found, score and time played. We also have a bonus system that is triggered by the number of moves made. The more moves the user makes the higher the bonus.