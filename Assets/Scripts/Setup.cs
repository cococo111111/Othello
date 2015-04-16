using UnityEngine;
using Data;

public class Setup : MonoBehaviour {

    public GameObject _cellPrefab;
    public GameObject[,] _cells;
    public CellBehavior[,] _cellBehaviors;
    public int _cellDistance;
    public Board _board;
    public int _boardSize = 8;
    public GameFlow _gf;
    public GameObject _inGameSplash;
    public int _AI_State;

    void Awake () {
        _gf = gameObject.AddComponent<GameFlow>();
        _board = new Board(_boardSize);
    }

	// Add cells, board, and cellBehaviors
	void Start () {
        _cells = new GameObject[_boardSize,_boardSize];
        _cellBehaviors = new CellBehavior[_boardSize,_boardSize];

        for (int x = 0; x < _boardSize; x++) {
            for (int y = 0; y < _boardSize; y++) {
                _cells[x,y] = (GameObject) Instantiate (
                        _cellPrefab, new Vector3((x-4)*_cellDistance+37, 
                            (4-y)*_cellDistance-35.5f, 0), Quaternion.identity); 
                _cellBehaviors[x,y] = _cells[x,y].GetComponent<CellBehavior>();
                _cellBehaviors[x,y]._x = x;
                _cellBehaviors[x,y]._y = y;
            }
        }
    }
}
