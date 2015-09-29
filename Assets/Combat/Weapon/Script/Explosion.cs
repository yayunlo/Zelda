using UnityEngine;
using System.Collections;

public class Explosion : MonoBehaviour {

	public int countdown = 4;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (countdown > 0)
			countdown--;
		else 
			Destroy (gameObject);
	}
}
