using UnityEngine;
using System.Collections;

public class Arrow : MonoBehaviour {

	public char towardDir = '0';
	public float speed = 5f;
	public GameObject explorePrefab;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 newPos = transform.position;
		
		if(towardDir == 'n') {
			newPos.y += speed * Time.deltaTime;
		}
		else if (towardDir == 's') {
			newPos.y -= speed * Time.deltaTime;
		}
		else if (towardDir == 'e') {
			newPos.x += speed * Time.deltaTime;
		}
		else if (towardDir == 'w') {
			newPos.x -= speed * Time.deltaTime;
		}
		transform.position = newPos;
	}

	void OnTriggerEnter ( Collider coll ) {
		if (coll.gameObject.tag == "Wall" 
		    || coll.gameObject.tag == "Door" 
		    || coll.gameObject.tag == "LockedDoor") {

			Vector3 explodePos = transform.position;
			
			if(towardDir == 'n') {
				explodePos.y += 0.5f;
			}
			else if (towardDir == 's') {
				explodePos.y -= 0.5f;
			}
			else if (towardDir == 'e') {
				explodePos.x += 0.5f;
			}
			else if (towardDir == 'w') {
				explodePos.x -= 0.5f;
			}

			Instantiate(explorePrefab, explodePos, Quaternion.identity );
			Destroy (gameObject);
		}
	}
}
