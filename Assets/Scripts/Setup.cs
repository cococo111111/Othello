using UnityEngine;

public class Setup : MonoBehaviour {

    public GameObject _cellPrefab;
    public GameObject[,] _cells;
    public int _cellDistance;
    private int tempCellDistance;

    const int boardSize = 8;

	// Use this for initialization
	void Start () {
        _cells = new GameObject[8,8];
        for (int x = 0; x < 8; x++) {
            for (int y = 0; y < 8; y++) {
                _cells[x,y] = (GameObject) Instantiate (
                        _cellPrefab, 
                        new Vector3((4-x)*_cellDistance, 0, (y-4)*_cellDistance), 
                        Quaternion.identity); 
                Debug.Log("Cell created");
            }
        }
    }

    // Update is called once per frame
    void Update () {
        if (_cellDistance != tempCellDistance) {
            for (int x = 0; x < 8; x++) {
                for (int y = 0; y < 8; y++) {
                    _cells[x,y].transform.position = new Vector3((4-x)*_cellDistance, 0, (y-4)*_cellDistance);
                }
            }
            tempCellDistance = _cellDistance;
            Debug.Log("Cell created");
        }
    }
}
