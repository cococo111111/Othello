using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Data;

namespace ArtificialIntelligence {
    public class PossibleMove {
        public int x, y, score;
        public PossibleMove (int _x, int _y, int _score) {
            x = _x;
            y = _y;
            score = _score;
        }
    }

    public class AI : MonoBehaviour {
        private Board board;
        static int _trn;
        readonly int bSize;

        public AI () {
            _trn = 1;
            board = FindObjectOfType<Setup>()._board;
            bSize = board._size;
        }

        public AI (int turn) {
            _trn = turn;
            board = FindObjectOfType<Setup>()._board;
            bSize = board._size;
        }

        public void AIMakeTurn (int ply, Board b, int turn) {
            PossibleMove move = this.ProduceBest(ply, b, turn);
            // Debug.Log("move = " + move.x + "," + move.y);
            b.AddPiece(move.x, move.y, turn);
            AI.TakeTurn(move.x, move.y, turn, b);
        }

        public PossibleMove ProduceBest(int level, Board b, int color) {
            // ensure a current working board
            Board scratchBoard = new Board(b._size);
            Board.CopyBoard(scratchBoard, b);

            BoardNode root = new BoardNode(scratchBoard);
            // root._turn = (color == 1) ? 0 : 1;

            // Start alphabeta algorithm and store the best H value
            PossibleMove bestH = 
                this.AlphaBeta(root, level, new PossibleMove(-1, -1,int.MinValue), 
                        new PossibleMove(-1, -1,int.MaxValue), true);

            // Debug.Log("ProduceBest = " + bestH.x + ", " + bestH.y + ", score = " + bestH.score);

            return bestH; 
        }

        public PossibleMove AlphaBeta
            (BoardNode node, int depth, PossibleMove alpha, PossibleMove beta, bool maximizingPlayer) { 

            if (depth == 0) { // || node.IsLeaf) {
                // Debug.Log("node._move[0] = " + node._move.x);
                // test node collection here
                return new PossibleMove (node._move.x, node._move.y, BoardNode.GetHeuristicValue(node, depth)); 
            }

            if (maximizingPlayer == true) {
                // Debug.Log("Max");
                List<PossibleMove> childs = CheckLegalMoves(node._bData, 1);
                // List<BoardNode> children = BoardNode.GetChildren(node, 1);
                PossibleMove score;
                foreach (PossibleMove kid in childs) {
                    var bn = BoardNode.GetChild(node._bData, kid, 1);
                    score = AlphaBeta(bn, depth - 1, alpha, beta, !maximizingPlayer);
                    alpha = alpha.score == Mathf.Max(alpha.score, score.score) ? alpha : score;
                    alpha.x = kid.x;
                    alpha.y = kid.y;
                    if (beta.score <= alpha.score)
                        break; // beta cut-off
                }
                // Debug.Log("alpha = " + alpha.score);
                return alpha;
            }
            else {
                // Debug.Log("Min");
                List<PossibleMove> childs = CheckLegalMoves(node._bData, 0);
                // List<BoardNode> children = BoardNode.GetChildren(node, 0);
                PossibleMove score;
                foreach (PossibleMove kid in childs) {
                    var bn = BoardNode.GetChild(node._bData, kid, 0);
                    score = AlphaBeta(bn, depth - 1, alpha, beta, !maximizingPlayer);
                    beta = beta.score == Mathf.Min(beta.score, score.score) ? beta : score;  // beta = min(beta,score)
                    beta.x = kid.x;
                    beta.y = kid.y;
                    if (beta.score <= alpha.score)
                        break; // alpha cut-off
                }
                // Debug.Log("beta = " + beta.score);
                return beta;
            }
        }

        // randomize list function
        public static void ShufflePossibleMovesList(List<PossibleMove> list) {
            System.Random rng = new System.Random();  
            int n = list.Count;  
            while (n > 1) {  
                n--;  
                int k = rng.Next(n - 1);
                PossibleMove value = list[k];  
                list[k] = list[n];  
                list[n] = value;  
            }  
        }

        public static List<PossibleMove> CheckLegalMoves (Board b, int turn) {

            var legal = new List<PossibleMove>();
            for (int i = 0; i < b._size; i++) 
                for (int j = 0; j < b._size; j++ ) 
                    if (CheckCurrent(i, j, turn, b)) {
                        PossibleMove temp = new PossibleMove(i, j, int.MinValue);
                        legal.Add(temp);
                    }
            AI.ShufflePossibleMovesList(legal);
            return legal;
        }

        public static bool CheckCurrent (int x, int y, int _turn, Board b) {
            // return false if current position is not blank
            if (b.GetValue(x,y) != 2)
                return false;

            // check to see if surrounding block contains at least one of the
            // opposite color
            bool top, topRight, right, bottomRight, bottom, bottomLeft, left, topLeft;
            top = topRight = right = bottomRight = bottom = bottomLeft = left = topLeft = false;

            top =           (b.GetValue(x, y+1) != _turn    && b.GetValue(x, y+1) != 2);
            topRight =      (b.GetValue(x+1, y+1) != _turn  && b.GetValue(x+1, y+1) != 2);
            right =         (b.GetValue(x+1, y) != _turn    && b.GetValue(x+1, y) != 2);
            bottomRight =   (b.GetValue(x+1, y-1) != _turn  && b.GetValue(x+1, y-1) != 2);
            bottom =        (b.GetValue(x, y-1) != _turn    && b.GetValue(x, y-1) != 2);
            bottomLeft =    (b.GetValue(x-1, y-1) != _turn  && b.GetValue(x-1, y-1) != 2);
            left =          (b.GetValue(x-1, y) != _turn    && b.GetValue(x-1, y) != 2);
            topLeft =       (b.GetValue(x-1, y+1) != _turn  && b.GetValue(x-1, y+1) != 2);

            if (!(top || topRight || right || bottomRight || bottom || bottomLeft || left || topLeft)) return false;

            bool xtop, xtopRight, xright, xbottomRight, xbottom, xbottomLeft, xleft, xtopLeft;
            xtop = xtopRight = xright = xbottomRight = xbottom = xbottomLeft = xleft = xtopLeft = true;

            // horizontal //
            for (int i = 2; left && i < b._size; i++) { 
                if (b.GetValue(x-i, y) == _turn) {
                    left = true;
                    break;
                }
                if (b.GetValue(x-i, y) == 2) {
                    xleft = false;
                    break;
                }
            }
            for (int i = 2; right && i < b._size; i++) {
                if (b.GetValue(x+i, y) == _turn) {
                    right = true;
                    break;
                }            
                if (b.GetValue(x+i, y) == 2) {
                    xright = false;
                    break;
                }
            }

            // veritcal //
            for (int i = 2; bottom && i < b._size; i++) {
                if (b.GetValue(x, y-i) == _turn) {
                    bottom = true;
                    break;
                }
                if (b.GetValue(x, y-i) == 2) {
                    xbottom = false;
                    break;
                }
            }
            for (int i = 2; top && i < b._size; i++) {
                if (b.GetValue(x, y+i) == _turn) {
                    top = true;
                    break;
                }
                if (b.GetValue(x, y+i) == 2) {
                    xtop = false;
                    break;
                }
            }

            // diagonal / //
            for (int i = 2; bottomLeft && i < b._size; i++) {
                if (b.GetValue(x-i, y-i) == _turn) {
                    bottomLeft = true;
                    break;
                }
                if (b.GetValue(x-i, y-i) == 2) {
                    xbottomLeft = false;
                    break;
                }
            }
            for (int i = 2; topRight && i < b._size; i++) {
                if (b.GetValue(x+i, y+i) == _turn) {
                    topRight = true;
                    break;
                }
                if (b.GetValue(x+i, y+i) == 2) {
                    xtopRight = false;
                    break;
                }
            }

            // diagonal \ // 
            for (int i = 2; topLeft && i < b._size; i++) {
                if (b.GetValue(x-i, y+i) == _turn) {
                    topLeft = true;
                    break;
                }
                if (b.GetValue(x-i, y+i) == 2) {
                    xtopLeft = false;
                    break;
                }
            }
            for (int i = 2; bottomRight && i < b._size; i++) {
                if (b.GetValue(x+i, y-i) == _turn) {
                    bottomRight = true;
                    break;
                }
                if (b.GetValue(x+i, y-i) == 2) {
                    xbottomRight = false;
                    break;
                }
            }        
            return (right && xright || left && xleft || bottom && xbottom || top && xtop || 
                    topRight && xtopRight || topLeft && xtopLeft || 
                    bottomRight && xbottomRight || bottomLeft && xbottomLeft);
        }

        public static void TakeTurn(int x, int y, int _turn, Board b) {
            // apply current move to diagonals, vertical & horizontal
            bool top, topRight, right, bottomRight, bottom, bottomLeft, left, topLeft;
            top = topRight = right = bottomRight = bottom = bottomLeft = left = topLeft = false;

            bool xtop, xtopRight, xright, xbottomRight, xbottom, xbottomLeft, xleft, xtopLeft;
            xtop = xtopRight = xright = xbottomRight = xbottom = xbottomLeft = xleft = xtopLeft = true;

            // horizontal //
            for (int i = 1; i < b._size; i++) { 
                if (b.GetValue(x-i, y) == _turn) {
                    left = true;
                    break;
                }
                if (b.GetValue(x-i, y) == 2) {
                    xleft = false;
                    break;
                }
            }
            for (int i = 1; i < b._size; i++) {
                if (b.GetValue(x+i, y) == _turn) {
                    right = true;
                    break;
                }            
                if (b.GetValue(x+i, y) == 2) {
                    xright = false;
                    break;
                }
            }

            // veritcal //
            for (int i = 1; i < b._size; i++) {
                if (b.GetValue(x, y-i) == _turn) {
                    bottom = true;
                    break;
                }
                if (b.GetValue(x, y-i) == 2) {
                    xbottom = false;
                    break;
                }
            }
            for (int i = 1; i < b._size; i++) {
                if (b.GetValue(x, y+i) == _turn) {
                    top = true;
                    break;
                }
                if (b.GetValue(x, y+i) == 2) {
                    xtop = false;
                    break;
                }
            }

            // diagonal / //
            for (int i = 1; i < b._size; i++) {
                if (b.GetValue(x-i, y-i) == _turn) {
                    bottomLeft = true;
                    break;
                }
                if (b.GetValue(x-i, y-i) == 2) {
                    xbottomLeft = false;
                    break;
                }
            }
            for (int i = 1; i < b._size; i++) {
                if (b.GetValue(x+i, y+i) == _turn) {
                    topRight = true;
                    break;
                }
                if (b.GetValue(x+i, y+i) == 2) {
                    xtopRight = false;
                    break;
                }
            }

            // diagonal \ // 
            for (int i = 1; i < b._size; i++) {
                if (b.GetValue(x-i, y+i) == _turn) {
                    topLeft = true;
                    break;
                }
                if (b.GetValue(x-i, y+i) == 2) {
                    xtopLeft = false;
                    break;
                }
            }
            for (int i = 1; i < b._size; i++) {
                if (b.GetValue(x+i, y-i) == _turn) {
                    bottomRight = true;
                    break;
                }
                if (b.GetValue(x+i, y-i) == 2) {
                    xbottomRight = false;
                    break;
                }
            }        

            // make sure there are adjacent blocks

            if (right && xright) 
                for (int i = 1; i < b._size && b.GetValue(x+i, y) != _turn; i++)
                    b.AddPiece(x+i, y, _turn);

            if (left && xleft)
                for (int i = 1; i < b._size && b.GetValue(x-i, y) != _turn; i++)
                    b.AddPiece(x-i, y, _turn);

            if (bottom && xbottom)
                for (int i = 1; i < b._size && b.GetValue(x, y-i) != _turn; i++)
                    b.AddPiece(x, y-i, _turn);

            if (top && xtop)
                for (int i = 1; i < b._size && b.GetValue(x, y+i) != _turn; i++)
                    b.AddPiece(x, y+i, _turn);

            if (bottomLeft && xbottomLeft)
                for (int i = 1; i < b._size && b.GetValue(x-i, y-i) != _turn; i++)
                    b.AddPiece(x-i, y-i, _turn);

            if (topRight && xtopRight)
                for (int i = 1; i < b._size && b.GetValue(x+i, y+i) != _turn; i++)
                    b.AddPiece(x+i, y+i, _turn);

            if (topLeft && xtopLeft)
                for (int i = 1; i < b._size && b.GetValue(x-i, y+i) != _turn; i++)
                    b.AddPiece(x-i, y+i, _turn);

            if (bottomRight && xbottomRight)
                for (int i = 1; i < b._size && b.GetValue(x+i, y-i) != _turn; i++)
                    b.AddPiece(x+i, y-i, _turn);
        }
    }
}
