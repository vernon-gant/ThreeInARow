### `Board`

Initially was exposing getters and setters which allow direct manipulation of the board. This is not ideal as it breaks encapsulation and makes it harder to reason about the state of the board.
Also no operations were defined on the board, which makes it hard to reason about the board's state and how it changes. Was replaced with BoardLayout with a
set of operations that allow us to manipulate the board layout.

### `Element`

Was used to represent a single element on the board. However after analysis I decided to remove it because no explicit operations were defined on it.
Could encapsulate ID but still it was not necessary as we can use the ID directly.

### `ElementContext`

Also did not have any operations defined on it. It was used to store the context of an element on the board. However, this context can be stored directly on the board layout.
And we can query the board layout for the context of an element.