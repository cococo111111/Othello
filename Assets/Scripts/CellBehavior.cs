using System.Collections;
using UnityEngine;
using ArtificialIntelligence;
using Data;

public class CellBehavior : MonoBehaviour {
    public Sprite[] _sprites;
    SpriteRenderer sr;
    public bool _legalPosition;

    // _color: 0 = black, 1 = white
    public bool _color;
    public int _x, _y;
    int currentValue;
    int currentTurn;

    public GameFlow _gf;
    public Board _board;
    public bool _current;
    int localTurnCount;
    bool advanceEnabled;

    // Use this for initialization
	void Start () {
        _gf = FindObjectOfType<GameFlow>();
	    sr = GetComponent<SpriteRenderer>();
        sr.sprite = _sprites[0]; // default - blank
        _board = FindObjectOfType<Setup>()._board;
        _current = false;
        _legalPosition = false;
        localTurnCount = _gf._turnCount;
	}
	
	// Update is called once per frame
	void Update () {
        if (!_current || currentTurn != _gf._currentTurn || _gf._turnCount > localTurnCount) {
            currentValue = _board.GetValue(_x,_y);
            _legalPosition = AI.CheckCurrent(_x,_y,_gf._currentTurn,_board);
            _current = true;
            currentTurn = _gf._currentTurn;
            localTurnCount++;
        }
        if (currentValue == 0 && sr.sprite != _sprites[1]) {
            if (sr.sprite == _sprites[2])
                StartCoroutine(fadeIn());
            else
                StartCoroutine(fadeInBlack());
            sr.sprite = _sprites[1];
        }
        if (currentValue == 1 && sr.sprite != _sprites[2]) {
            sr.sprite = _sprites[2];
            StartCoroutine(fadeInWhite());
        }
	}

    IEnumerator fadeInBlack () {
        for (float i = 0.6f; i <= 1.0f; i += 0.1f) {
            Color c = sr.material.color;
            c.a = i;
            sr.material.color = c;
            yield return null; 
        }
    }

    IEnumerator fadeInWhite () {
        for (float i = 0; i <= 1.0f; i += 0.1f) {
            Color c = sr.material.color;
            c.a = i;
            sr.material.color = c;
            yield return null; 
        }
    }

    IEnumerator fadeIn () {
        for (float i = 0; i <= 1.0f; i += 0.1f) {
            Color c = sr.material.color;
            c.a = i;
            sr.material.color = c;
            yield return null; 
        }
    }

    void OnMouseEnter () {
        if (_legalPosition) {
            if (_gf._currentTurn == 0 && sr.sprite != _sprites[3])
                sr.sprite = _sprites[3];
            else if (_gf._currentTurn == 1 && sr.sprite != _sprites[0])
                sr.sprite = _sprites[0];
        }
    }

    void OnMouseExit () {
        if (currentValue == 2)
            sr.sprite = _sprites[0];
    }

    void OnMouseDown() {
        if (_legalPosition && sr.sprite == _sprites[3]) {
            Select();
        }
    }

    void OnMouseUp () {
    }

    void Select () {
        _board.AddPiece(_x,_y,_gf._currentTurn);
        AI.TakeTurn(_x,_y,_gf._currentTurn,_board);
        advanceEnabled = true;
        Advance ();
    }

    void Advance () {
        if (advanceEnabled) {
            _gf.TurnTaken();
            _current = false;
            advanceEnabled = false;
        }
    }
}
