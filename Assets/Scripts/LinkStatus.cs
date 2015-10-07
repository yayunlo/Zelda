using UnityEngine;
using System.Collections;

public class LinkStatus : MonoBehaviour {

    public static int health;
    public static int rupee_count;
    public static int key_count;
    public static int bomb_count;

	// Use this for initialization
	void Start () {
        health = 6;
        rupee_count = 0;
        key_count = 0;
        bomb_count = 0;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
