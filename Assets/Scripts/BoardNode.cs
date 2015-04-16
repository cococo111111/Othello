using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ArtificialIntelligence;
using Data;

namespace Data {
    public class BoardNode {
        public BoardNode _parent;
        public List<BoardNode> _children;
        public int _turn;
        public PossibleMove _move;

        public Board _bData;
        // private int numberOfTiles;
        // private int numberOfMoves;

        public BoardNode (Board b) {
            _bData = b;
            _children = new List<BoardNode>();
        }

        public BoardNode (Board b, PossibleMove move) {
            _bData = b;
            _move = move;
            _children = new List<BoardNode>();
        }
        
        public bool IsLeaf {
            get {
                return (_children.Count == 0); 
            }
        }

        
        public static BoardNode GetChild (Board b, PossibleMove pm, int trn) {
            var nb = new Board(b._size);
            Board.CopyBoard(nb, b);
            var item = new BoardNode(nb, pm);
            item._bData.AddPiece(pm.x, pm.y, trn);
            AI.TakeTurn(pm.x, pm.y, trn, item._bData);
            return item;
        }

        public static List<BoardNode> GetChildren (BoardNode bn, int trn) {
            List<PossibleMove> moves = AI.CheckLegalMoves(bn._bData, bn._turn);
            var childs = new List<BoardNode>();
            foreach (PossibleMove pos in moves) {
                var b = new Board(bn._bData._size);
                Board.CopyBoard(b, bn._bData);
                var item = new BoardNode(b, pos);
                item._bData.AddPiece(pos.x, pos.y, trn);
                AI.TakeTurn(pos.x, pos.y, trn, item._bData);
                childs.Add(item);
                item._parent = bn;
            }
            bn._children = childs;
            return childs;
        }

        public static int GetHeuristicValue (BoardNode bn, int turn) {
           int[] scores = bn._bData.GetScores();
           return scores[turn];
        }
    }
}
