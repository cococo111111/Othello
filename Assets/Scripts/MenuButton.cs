using UnityEngine;

public class MenuButton : MonoBehaviour {
    SpriteRenderer _sr;
    public Sprite[] _sprites;
    public bool _triggerAction;
    public int _arrayOffset;

	// Use this for initialization
	void Start () {
        _sr = renderer as SpriteRenderer;
        _sr.sprite = _sprites[0 + _arrayOffset];
	}
	
    void OnMouseOver () {
        _sr.sprite = _sprites[1 + _arrayOffset];
    }
    
    void OnMouseDown () {
        _triggerAction = true;

    }

    void OnMouseExit () {
        _sr.sprite = _sprites[0 + _arrayOffset];
    }
}
