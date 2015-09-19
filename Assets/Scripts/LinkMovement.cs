using UnityEngine;
using System.Collections;

public class LinkMovement : MonoBehaviour {

	public float velocityFactor = 1.0f;
	public GameObject swordPrefab;
	public GameObject swordInstance;
	public int swordCooldown = 30;
	public char currentDir = 'n';
	public float positionCorrectParam = 0.025f;
	Vector2 positionError = Vector2.zero;
	

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
		horizontal_input = (positionError.y != 0) ? 0.0f : horizontal_input;
		vertical_input = (positionError.x != 0) ? 0.0f : vertical_input;

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

		if (horizontal_input != 0.0f) {
			positionError.x = (horizontal_input > 0.0f)? 1.0f : -1.0f;
		} 
		else if (vertical_input != 0.0f) {
			positionError.y = (vertical_input > 0.0f)? 1.0f : -1.0f;
		}

		// Animate
		GetComponent<Animator> ().SetFloat ("vertical_vel", newVelocity.y);
		GetComponent<Animator> ().SetFloat ("horizontal_vel", newVelocity.x);

		// If we're not moving, don't animate.
		//if (newVelocity.magnitude == 0)
		if (positionError.x == 0.0f && positionError.y == 0.0f)
			GetComponent<Animator> ().speed = 0.00000001f;
		else
			GetComponent<Animator> ().speed = 1.0f;

		Combat ();

		// Correct postition to correct track
		if (positionError.x != 0.0f && horizontal_input == 0.0f) {
			
			Vector3 newPos = transform.position;

			float idealX = Mathf.Ceil (transform.position.x / 0.5f);
			if ( positionError.x < 0.0f) {
				idealX = idealX - 1.0f;
			}
			
			if( Mathf.Abs( idealX * 0.5f - transform.position.x ) > positionCorrectParam) {
				newPos.x = transform.position.x + (positionError.x * positionCorrectParam);
			}
			else {
				newPos.x = idealX * 0.5f;
				positionError.x = 0.0f;
			}
			transform.position = newPos;
		} 
		if (positionError.y != 0.0f && vertical_input == 0.0f) {

			Vector3 newPos = transform.position;

			float idealY = Mathf.Ceil (transform.position.y / 0.5f);
			if ( positionError.y < 0.0f) {
				idealY = idealY - 1.0f;
			}

			if( Mathf.Abs( idealY * 0.5f - transform.position.y ) > positionCorrectParam) {
				newPos.y = transform.position.y + (positionError.y * positionCorrectParam);

			}
			else {
				newPos.y = idealY * 0.5f;
				positionError.y = 0.0f;
			}
			transform.position = newPos;
		} 

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
			Destroy (coll.gameObject);
		} 
		else {
			if(positionError.x != 0) {
				Vector3 newPos = transform.position;
				newPos.x = Mathf.Round(newPos.x / 0.5f) * 0.5f;
				transform.position = newPos;
				positionError.x = 0.0f;
			}
			if(positionError.y != 0) {
				Vector3 newPos = transform.position;
				newPos.y = Mathf.Round(newPos.y / 0.5f) * 0.5f;
				transform.position = newPos;
				positionError.y = 0.0f;
			}
		}
	}
}
