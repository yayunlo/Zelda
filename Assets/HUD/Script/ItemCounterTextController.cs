using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ItemCounterTextController : MonoBehaviour {

    public Text counterText;


	// Use this for initialization
	void Start () {
        counterText = GetComponent<Text>();
        counterText.text = "X 0\nX 0\nX 0";
	}
	
	// Update is called once per frame
	void Update () {
        counterText.text = "X " + LinkStatus.rupee_count + "\nX " + LinkStatus.key_count + "\nX " + LinkStatus.bomb_count;
	}
}
