For now it seems like no specific implementations of trees of graphs are needed. Although we can imagine
that elements of the board(not the ui grid) could be represented as nodes in a graph but internally the ElementSpace
will just store the reference to the element and a reference to neighbor context which in turn will store two lists
of elements that are neighbors of the element in the context of the board. For the rest we will use standard data structures like lists and
dictionaries.