using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ItemCounterTextController : MonoBehaviour {

    public Text counterText;

	private Link playerPawn;

	// Use this for initialization
	void Start () {
        counterText = GetComponent<Text>();
        counterText.text = "X 0\nX 0\nX 0";
		playerPawn = GameObject.Find("Link").GetComponent<Link>();
	}
	
	// Update is called once per frame
	void Update () {
        counterText.text = "X " + playerPawn.rupeeCount + "\nX " + playerPawn.keyCount + "\nX " + playerPawn.bombCount;
	}
}
