using UnityEngine;

public class MenuButtons : MonoBehaviour {
    public GameObject _button;
    GameObject start;
    MenuButton startB;

	// Use this for initialization
	void Start () {
        start =    (GameObject) Instantiate(_button, new Vector3(   0, -175, -30), Quaternion.identity);
        startB =   start.GetComponent<MenuButton>();

        startB._arrayOffset = 2;
	}
	
	// Update is called once per frame
	void Update () {
        if (startB._triggerAction)
            SendAction();
	}


    void SendAction () {
        Application.LoadLevel("Main");

    }
}
