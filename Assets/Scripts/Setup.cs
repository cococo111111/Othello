using UnityEngine;

public class Setup : MonoBehaviour {

    public GameObject _cellPrefab;
    public GameObject[,] _cells;
    public int _cellDistance;
    private int tempCellDistance;

    public int _boardSize;
    float quadrantSize;
    float offset;

	// Use this for initialization
	void Start () {
        quadrantSize = _boardSize/2.0f;
        offset = 0.5f;
        _cells = new GameObject[_boardSize,_boardSize];

        for (int x = 0; x < _boardSize; x++) {
            for (int y = 0; y < _boardSize; y++) {
                _cells[x,y] = (GameObject) Instantiate (
                        _cellPrefab, 
                        new Vector3((quadrantSize-x-offset)*_cellDistance, 
                            0, (quadrantSize-y-offset)*_cellDistance), 
                        Quaternion.identity); 
                // Debug.Log("Cell created");
            }
        }
    }

    // Update is called once per frame
    void Update () {
        // when transforming distance bewteen cells
        if (_cellDistance != tempCellDistance) {
            for (int x = 0; x < _boardSize; x++) {
                for (int y = 0; y < _boardSize; y++) {
                    _cells[x,y].transform.position = 
                        new Vector3((quadrantSize-x-offset)*_cellDistance, 
                                0, (quadrantSize-y-offset)*_cellDistance);
                }
            }
            tempCellDistance = _cellDistance;
            // Debug.Log("Cell moved");
        }
    }
}
