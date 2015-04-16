using UnityEngine;

public class InGameButtons : MonoBehaviour {
    public GameObject _button;
    GameObject start;
    MenuButton startB;

	// Use this for initialization
	void Start () {
        start =    (GameObject) Instantiate(_button, new Vector3(   0, -165, -30), Quaternion.identity);
        startB =   start.GetComponent<MenuButton>();

        startB._arrayOffset = 0;
	}
	
	// Update is called once per frame
	void Update () {
        if (startB._triggerAction)
            IGSendAction();
	}

    void IGSendAction () {
        Application.LoadLevel("Main");
    }
}
