using Data;

namespace Data {
    public class BoardStats {
        public static int[] CalcStats (Board b) {
            int[] _stats = new int[3];
            for (int i = 0; i < b._size; i++) { 
                for (int j = 0; j < b._size; j++) {
                    int place = b.GetValue(i,j);
                        if (place == 0) _stats[0]++; //_black++;
                        if (place == 1) _stats[1]++; //_white++;
                        if (place == 2) _stats[2]++; //_blank++;
                }
            }
            return _stats;
        }
    }
    public class Board {
        // int values: 0 = blackPiece, 1 = whitePiece, 2 = noPiece

        private int[,] board;
        public int _size;

        public Board (int s) {
            board = new int[s, s];
            _size = s;
            NewBoard(true);
        }

        public int GetValue (int x, int y) {
            if (x >= _size || y >= _size || x < 0 || y < 0) 
                return 2; // empty
            return board[x,y];
        }

        public int[] GetScores () {
            var scores = new int[2];
            scores[0] = scores[1] = 0;
            for (int i = 0; i < _size; i++) { 
                for (int j = 0; j < _size; j++) {
                    if (board[i,j] == 0)
                        scores[0]++;
                    else if (board[i,j] == 1)
                        scores[1]++;
                }
            }
            return scores;
        }
        
        public int GetBoardSize () {
            return _size;
        }

        public void AddPiece (int x, int y, int color) {
            board[x,y] = color;
        }

        public static void CopyBoard(Board to, Board from) {
            for (int i = 0; i < from._size; i++) 
                for (int j = 0; j < from._size; j++) 
                    to.AddPiece(i, j, from.GetValue(i,j)); 
        }

        public void NewBoard (bool ab) {
            if (!ab) {
                board = new int[_size, _size];
                for (int i = 0; i < _size; i++) { 
                    for (int j = 0; j < _size; j++) {
                        if (i == 3 && j == 3 || i == 4 && j == 4)
                            board[i,j] = 0;
                        else if (i == 3 && j == 4 || i == 4 && j == 3) 
                            board[i,j] = 1;
                        else
                            board[i,j] = 2;
                    }
                }
            }
            else {
                board = new int[_size, _size];
                for (int i = 0; i < _size; i++) { 
                    for (int j = 0; j < _size; j++) {
                        if (i == 3 && j == 3 || i == 4 && j == 4)
                            board[i,j] = 1;
                        else if (i == 3 && j == 4 || i == 4 && j == 3) 
                            board[i,j] = 0;
                        else
                            board[i,j] = 2;
                    }
                }
            }
        }
    }
}
