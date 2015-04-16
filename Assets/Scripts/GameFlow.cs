using System.Collections;
using UnityEngine;
using ArtificialIntelligence;
using Data;

public class GameFlow : MonoBehaviour {
    public int _currentTurn;
    public Board _board;    // this is the main game board
    public int _turnCount;
    public AI _gameAI;
    public int[] _boardStats;
    public int _ai_state;
    // _ai_state will be used to determine the depth of the searching and act
    // as a way for the user to change the difficulty of the gameplay
    // 0 = easy, 1 = medium, 2 = hard;
    protected int _depth;

    void Awake () {
        _currentTurn = 0;
        _ai_state = FindObjectOfType<Setup>()._AI_State;
    }

    void Start () {
        _board = FindObjectOfType<Setup>()._board;
        _gameAI = new AI();

        switch (_ai_state) {
            case 0: 
                _depth = 2;
                break;
            case 1:
                _depth = 3;
                break;
            case 2:
                _depth = 5;
                break;
            default:
                _depth = 3;
                break;
        }
    }

    public void TurnTaken () {
        // Check the board for few open positions to save compute time and
        // errors;
        _boardStats = BoardStats.CalcStats(_board);
        if(_boardStats[2] <= 3) 
                _depth = 1;
        else if (_boardStats[2] <= 10)
                _depth = 2;

        _currentTurn = _currentTurn == 0 ? 1 : 0;

        if (WinCondition()) {
            var go = (GameObject) Instantiate(FindObjectOfType<Setup>()._inGameSplash, new Vector3(0,0,-20), Quaternion.identity);
            if (_boardStats[0] >= _boardStats[1]) 
                go.GetComponentInChildren<GUIText>().text = "Black Wins! " + _boardStats[0] + ":" + _boardStats[1];
            else
                go.GetComponentInChildren<GUIText>().text = "White Wins! " + _boardStats[1] + ":" + _boardStats[0];
        }

        _turnCount++;

        // AI makes a turn when 1
        if (_currentTurn == 1)
            AIsTurn();
    }

    void AIsTurn () {
        StartCoroutine(AIwaitsToMove());
    }

    IEnumerator AIwaitsToMove () {
        yield return new WaitForSeconds(1);
        _gameAI.AIMakeTurn(_depth, _board, _currentTurn);
        TurnTaken();
    }

    public bool WinCondition() {
        // win conditions:
        //  *   if there is not more legal moves for the current turn
        //  *   if the AI cannot move
        return AI.CheckLegalMoves(_board, _currentTurn).Count == 0;
    }
}
