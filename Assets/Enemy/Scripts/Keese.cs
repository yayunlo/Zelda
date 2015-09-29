using UnityEngine;
using System.Collections;

public class keese : MonoBehaviour {

	public float speed = 1.5f;
	public Vector3[] posSet;
	Vector3 nextPos = Vector3.zero;
	public int dir;

	// Use this for initialization
	void Start () {

		posSet = new Vector3[8];
		posSet [0] = new Vector3 (1f, 0f, 0f);
		posSet [1] = new Vector3 (1f, 1f, 0f);
		posSet [2] = new Vector3 (0f, 1f, 0f);
		posSet [3] = new Vector3 (-1f, 1f, 0f);
		posSet [4] = new Vector3 (-1f, 0f, 0f);
		posSet [5] = new Vector3 (-1f, -1f, 0f);
		posSet [6] = new Vector3 (0f, -1f, 0f);
		posSet [7] = new Vector3 (1f, -1f, 0f);

		nextPos = transform.position;
	}
	
	// Update is called once per frame
	void Update () {

		if (transform.position == nextPos) {
			float chanceToTurn = Random.Range(0f, 1f);
			if(chanceToTurn < 0.8) {
				dir = Random.Range(0, 7);
				nextPos += posSet[dir];
			}
			else
				nextPos += posSet[dir];
		} 
		else {
			transform.position = Vector3.MoveTowards(transform.position, nextPos, speed * Time.deltaTime);
		}


	}

	void OnTriggerEnter ( Collider coll ) {

		if (coll.gameObject.tag == "Weapon") {

			Destroy (gameObject);
		} 
		else if ( coll.gameObject.tag == "Wall" || coll.gameObject.tag == "Door" || coll.gameObject.tag == "LockedDoor") {

			Vector3 roomPos = new Vector3(Mathf.Floor(transform.position.x/16f)*16f, Mathf.Floor(transform.position.y/11f)*11f, 0f);
			Vector3 keesePos = transform.position - roomPos;
			if(keesePos.x < 8 && keesePos.y < 5) 
				dir = 1;
			else if (keesePos.x >= 8 && keesePos.y < 5)
				dir = 3;
			else if (keesePos.x < 8 && keesePos.y >= 5)
				dir = 7;
			else if(keesePos.x >= 8 && keesePos.y >= 5)
				dir = 5;

			nextPos += 2 * posSet[dir];

		}
		
	}
}
