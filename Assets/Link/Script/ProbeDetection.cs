using UnityEngine;
using System.Collections;

public class ProbeDetection : MonoBehaviour {

	public Sprite doorTopL, doorTopR, doorDownL, doorDownR, doorLeft, doorRight;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter ( Collider coll ) {
		if (coll.gameObject.tag == "LockedDoor") {

			print (coll.gameObject.tag);

			if(LinkMovement.currentDir == 'n') {
				
				if(coll.transform.position.x < transform.position.x)
					coll.gameObject.GetComponent<SpriteRenderer>().sprite = doorTopL;
				else if (coll.transform.position.x > transform.position.x) 
					coll.gameObject.GetComponent<SpriteRenderer>().sprite = doorTopR;

				coll.gameObject.GetComponent<BoxCollider>().enabled = false;
			}
			else if (LinkMovement.currentDir == 's') {
				
				if(coll.transform.position.x < transform.position.x)
					coll.gameObject.GetComponent<SpriteRenderer>().sprite = doorDownL;
				else if (coll.transform.position.x > transform.position.x) 
					coll.gameObject.GetComponent<SpriteRenderer>().sprite = doorDownR;

				coll.gameObject.GetComponent<BoxCollider>().enabled = false;
			}
			else if (LinkMovement.currentDir == 'e') {
				coll.gameObject.GetComponent<SpriteRenderer>().sprite = doorRight;
				coll.gameObject.GetComponent<BoxCollider>().enabled = false;
			}
			else if (LinkMovement.currentDir == 'w') {
				coll.gameObject.GetComponent<SpriteRenderer>().sprite = doorLeft;
				coll.gameObject.GetComponent<BoxCollider>().enabled = false;
			}
			
		}
	
	}
	
}
