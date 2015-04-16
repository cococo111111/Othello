# Othello

Othello, also known as [Reversi](http://en.wikipedia.org/wiki/Reversi), 
is an abstract strategy game that has been around for over a hundred 
years (follow the link for more info about the gameplay).

This implementation of Reversi uses player vs computer mode. The AI uses 
the [MiniMax]() algorithm with alpha-beta pruning to discover the most
advantageous move for the computer. 

### Files of interest:

*[AI.cs](https://github.com/josefpaij/Othello/blob/master/Assets/Scripts/AI.cs)* 
Contains all functions called by the AI from GameFlow.cs.

*[GameFlow.cs](https://github.com/josefpaij/Othello/blob/master/Assets/Scripts/GameFlow.cs)*
Handles the exchange of turns, win conditions, and score keeping.

*[CellBehavior.cs](https://github.com/josefpaij/Othello/blob/master/Assets/Scripts/CellBehavior.cs)*
Cell functionality (mouse over, tile flips, etc.).
