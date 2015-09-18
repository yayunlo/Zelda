using UnityEngine;
using System.Collections;

public class LinkMovement : MonoBehaviour {

	public float velocityFactor = 1.0f;
	public GameObject swordPrefab;
	public GameObject swordInstance;
	public int swordCooldown = 30;
	public char currentDir = 'n';


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		float horizontal_input = Input.GetAxis ("Horizontal");
		float vertical_input = Input.GetAxis ("Vertical");

		// Link should not be able to move diagonally
		if (horizontal_input != 0.0f) {
			vertical_input = 0.0f;
		}

		if(horizontal_input > 0)	
			currentDir = 'e';
		else if(horizontal_input < 0)
			currentDir = 'w';
		else if(vertical_input > 0)
			currentDir = 'n';
		else if(vertical_input < 0)
			currentDir = 's';


		Vector3 newVelocity = new Vector3 (horizontal_input, vertical_input, 0);
		GetComponent<Rigidbody> ().velocity = newVelocity * velocityFactor;

		// Animate
		GetComponent<Animator> ().SetFloat ("vertical_vel", newVelocity.y);
		GetComponent<Animator> ().SetFloat ("horizontal_vel", newVelocity.x);

		// If we're not moving, don't animate.
		if (newVelocity.magnitude == 0)
			GetComponent<Animator> ().speed = 0.00000001f;
		else
			GetComponent<Animator> ().speed = 1.0f;

		Combat ();
	}

	void Combat (){

		// Progress the sword cooldown.
		if (swordCooldown > 0)
			swordCooldown--;
		else if (swordInstance != null)
			Destroy (swordInstance);

		if (Input.GetKeyDown (KeyCode.Space) && swordInstance == null) {

			// Spawn the sword into being
			swordInstance = Instantiate(swordPrefab, transform.position, Quaternion.identity) as GameObject;
			swordCooldown = 15;

			// Adjust sword angle and position based on current facing direction
			if(currentDir == 'n') {
				swordInstance.transform.position += new Vector3(0, 1, 0);
			}
			else if(currentDir == 'e') {
				swordInstance.transform.position += new Vector3(1, 0, 0);
				swordInstance.transform.Rotate(new Vector3(0, 0, 1), 270);
			}
			else if(currentDir == 's') {
				swordInstance.transform.position += new Vector3(0, -1, 0);
				swordInstance.transform.Rotate(new Vector3(0, 0, 1), 180);
			}
			else if(currentDir == 'w') {
				swordInstance.transform.position += new Vector3(-1, 0, 0);
				swordInstance.transform.Rotate(new Vector3(0, 0, 1), 90);
			}
		
		}

	}

	void OnCollisionEnter (Collision coll) {

		if (coll.gameObject.tag == "Rupee") {
			Destroy(coll.gameObject);
		}
	}
}
