using UnityEngine;
using System.Collections;

public enum weapon
{
	bow, boomerang, bumb, sword, flySword, whiteSword, flyWhiteSword
};

public class LinkMovement : MonoBehaviour {

	static Sprite[] linkSprite;

	public float velocityFactor = 1.0f;

	public static weapon weaponMode = weapon.sword;
	public GameObject weaponInstance;
	bool useWeapon = false;


	public GameObject swordPrefab;
	public int swordCooldown = 0;
	public Sprite swordUp, swordDown, swordRight, swordLeft;

	public GameObject arrowPrefab;
	public int arrowCooldown = 0;

	public static char currentDir = 'n';

	public float positionCorrectParam = 0.025f;
	public static Vector3 newRoomStartPos;

	public GameObject probePrefab;
	public GameObject probeInstance;


	Vector2 positionError = Vector2.zero;
	bool throughDoor = false;
	Vector3 throughDoorPos = Vector3.zero;
	BoxCollider doorBc1, doorBc2, doorBc3, doorBc4;

	int doorCount = 0;

	// Use this for initialization
	void Start () {
		linkSprite = Resources.LoadAll<Sprite>("link_sprites");
	}
	
	// Update is called once per frame
	void Update () {

		if (!throughDoor) {
			float horizontal_input = Input.GetAxis ("Horizontal");
			float vertical_input = Input.GetAxis ("Vertical");

			// Link should not be able to move diagonally
			if (horizontal_input != 0.0f) {
				vertical_input = 0.0f;
			}
			horizontal_input = (positionError.y != 0) ? 0.0f : horizontal_input;
			vertical_input = (positionError.x != 0) ? 0.0f : vertical_input;

			if(useWeapon) {
				horizontal_input = 0f;
				vertical_input = 0f;
			}

			if (horizontal_input > 0)	
				currentDir = 'e';
			else if (horizontal_input < 0)
				currentDir = 'w';
			else if (vertical_input > 0)
				currentDir = 'n';
			else if (vertical_input < 0)
				currentDir = 's';

			Vector3 newVelocity = new Vector3 (horizontal_input, vertical_input, 0);
			GetComponent<Rigidbody> ().velocity = newVelocity * velocityFactor;

			if (horizontal_input != 0.0f) {
				positionError.x = (horizontal_input > 0.0f) ? 1.0f : -1.0f;
			} else if (vertical_input != 0.0f) {
				positionError.y = (vertical_input > 0.0f) ? 1.0f : -1.0f;
			}

			// Animate
			GetComponent<Animator> ().SetFloat ("vertical_vel", newVelocity.y);
			GetComponent<Animator> ().SetFloat ("horizontal_vel", newVelocity.x);

			// If we're not moving, don't animate.
			//if (newVelocity.magnitude == 0)
			if (!useWeapon && positionError.x == 0.0f && positionError.y == 0.0f)
				GetComponent<Animator> ().speed = 0.00000001f;
			else
				GetComponent<Animator> ().speed = 1.0f;

			Combat ();
			UseTools ();

			// Correct postition to correct track
			if (positionError.x != 0.0f && horizontal_input == 0.0f) {
			
				Vector3 newPos = transform.position;

				float idealX = Mathf.Ceil (transform.position.x / 0.5f);
				if (positionError.x < 0.0f) {
					idealX = idealX - 1.0f;
				}
			
				if (Mathf.Abs (idealX * 0.5f - transform.position.x) > positionCorrectParam) {
					newPos.x = transform.position.x + (positionError.x * positionCorrectParam);
				} else {
					newPos.x = idealX * 0.5f;
					positionError.x = 0.0f;
				}
				transform.position = newPos;
			} 
			if (positionError.y != 0.0f && vertical_input == 0.0f) {

				Vector3 newPos = transform.position;

				float idealY = Mathf.Ceil (transform.position.y / 0.5f);
				if (positionError.y < 0.0f) {
					idealY = idealY - 1.0f;
				}

				if (Mathf.Abs (idealY * 0.5f - transform.position.y) > positionCorrectParam) {
					newPos.y = transform.position.y + (positionError.y * positionCorrectParam);

				} else {
					newPos.y = idealY * 0.5f;
					positionError.y = 0.0f;
				}
				transform.position = newPos;
			} 
		} 
		else {

			if (!CameraLeadMovement.cameraMoving) {
				transform.position = Vector3.MoveTowards( transform.position, newRoomStartPos, velocityFactor * Time.deltaTime );
			}
			else {
				transform.position = Vector3.MoveTowards(transform.position, throughDoorPos, velocityFactor * Time.deltaTime);
			}

			// check which posision is link in
			if ( currentDir == 'n' && (Mathf.Floor(transform.position.y / 11f)*11f + 1f) == transform.position.y ) {
				doorBc1.enabled = true;
				doorBc2.enabled = true;
				doorBc3.enabled = true;
				doorBc4.enabled = true;
				throughDoor = false;
				doorCount = 0;
			}
			else if ( currentDir == 's' && (Mathf.Ceil(transform.position.y / 11f)*11f -2f) == transform.position.y ) {
				doorBc1.enabled = true;
				doorBc2.enabled = true;
				doorBc3.enabled = true;
				doorBc4.enabled = true;
				throughDoor = false;
				doorCount = 0;
			}
			else if ( currentDir == 'e' && (Mathf.Floor(transform.position.x / 16f)*16f + 1f) == transform.position.x) {
				doorBc1.enabled = true;
				doorBc2.enabled = true;
				throughDoor = false;
				doorCount = 0;
			}
			else if ( currentDir == 'w' && (Mathf.Ceil(transform.position.x / 16f)*16f -2f) == transform.position.x) {
				doorBc1.enabled = true;
				doorBc2.enabled = true;
				throughDoor = false;
				doorCount = 0;
			}

		}
	}

	void Combat (){

		if (!useWeapon && Input.GetKeyDown (KeyCode.F))
			weaponMode = weapon.flySword;

		if (!useWeapon && Input.GetKeyDown (KeyCode.C)) {
			if(weaponMode == weapon.sword || weaponMode == weapon.flySword)
				weaponMode = weapon.bow;
			else if (weaponMode == weapon.bow)
				weaponMode = weapon.sword;

			print (weaponMode);
		}

		if (weaponMode == weapon.sword || weaponMode == weapon.flySword) {
		
			// Progress the sword cooldown.
			if (swordCooldown > 0)
				swordCooldown--;
			else { 
				if(weaponInstance != null){
					if (weaponMode == weapon.sword) {
						Destroy (weaponInstance);
					} 
					else if (weaponMode == weapon.flySword) {
						weaponInstance.GetComponent<Sword> ().towardDir = currentDir;
						weaponInstance.GetComponent<Sword> ().fly = true;
						weaponInstance = null;
					}
				}

				weaponInstance = null;
				gameObject.GetComponent<Animator> ().SetInteger ("ver_weapon", 0);
				gameObject.GetComponent<Animator> ().SetInteger ("hor_weapon", 0);
				useWeapon = false;

			}
			if (Input.GetKeyDown (KeyCode.Space) && weaponInstance == null) {
				
				// Spawn the sword into being
				weaponInstance = Instantiate (swordPrefab, transform.position, Quaternion.identity) as GameObject;
				swordCooldown = 15;
				useWeapon = true;
				
				// Adjust sword angle and position based on current facing direction
				if (currentDir == 'n') {
					weaponInstance.transform.position += new Vector3 (0, 1, 0);

				} else if (currentDir == 'e') {
					weaponInstance.transform.position += new Vector3 (1, 0, 0);
					weaponInstance.transform.Rotate (new Vector3 (0, 0, 1), 270);
				} else if (currentDir == 's') {
					weaponInstance.transform.position += new Vector3 (0, -1, 0);
					weaponInstance.transform.Rotate (new Vector3 (0, 0, 1), 180);
				} else if (currentDir == 'w') {
					weaponInstance.transform.position += new Vector3 (-1, 0, 0);
					weaponInstance.transform.Rotate (new Vector3 (0, 0, 1), 90);
				}
				
			}
		}
		else if ( weaponMode == weapon.bow ) {

			if(arrowCooldown == 0)
				useWeapon = false;
			else if(arrowCooldown > 0)
				arrowCooldown--;

			if(Input.GetKeyDown(KeyCode.Space) && !useWeapon) {

				// Spawn the sword into being
				GameObject weaponInstance = Instantiate(arrowPrefab, transform.position, Quaternion.identity) as GameObject;
				arrowCooldown = 3;
				useWeapon = true;


				// Adjust arrow angle and position based on current facing direction
				if(currentDir == 'n') {
					weaponInstance.transform.position += new Vector3(0, 1, 0);
					weaponInstance.GetComponent<Arrow>().towardDir = 'n';
				}
				else if(currentDir == 'e') {
					weaponInstance.transform.position += new Vector3(1, 0, 0);
					weaponInstance.transform.Rotate(new Vector3(0, 0, 1), 270);
					weaponInstance.GetComponent<Arrow>().towardDir = 'e';
				}
				else if(currentDir == 's') {
					weaponInstance.transform.position += new Vector3(0, -1, 0);
					weaponInstance.transform.Rotate(new Vector3(0, 0, 1), 180);
					weaponInstance.GetComponent<Arrow>().towardDir = 's';
				}
				else if(currentDir == 'w') {
					weaponInstance.transform.position += new Vector3(-1, 0, 0);
					weaponInstance.transform.Rotate(new Vector3(0, 0, 1), 90);
					weaponInstance.GetComponent<Arrow>().towardDir = 'w';
				}

				weaponInstance = null;
			}
		}


		if (useWeapon) {
			if (currentDir == 'n') {
				gameObject.GetComponent<Animator>().SetInteger("ver_weapon", 1);
				gameObject.GetComponent<Animator>().SetInteger("hor_weapon", 0);
			}
			else if (currentDir == 's') {
				gameObject.GetComponent<Animator>().SetInteger("ver_weapon", -1);
				gameObject.GetComponent<Animator>().SetInteger("hor_weapon", 0);
			}
			else if (currentDir == 'e') {
				gameObject.GetComponent<Animator>().SetInteger("ver_weapon", 0);
				gameObject.GetComponent<Animator>().SetInteger("hor_weapon", 1);
			}
			else if (currentDir == 'w') {
				gameObject.GetComponent<Animator>().SetInteger("ver_weapon", 0);
				gameObject.GetComponent<Animator>().SetInteger("hor_weapon", -1);
			}
			
		}


	}

	void UseTools () {

		if (Input.GetKeyDown (KeyCode.K) && LinkStatus.key_count > 0) {

			Vector3 probePos = transform.position;

			if( currentDir == 'n' ) probePos += new Vector3(0, 1f, 0);
			else if (currentDir == 's') probePos += new Vector3(0, -1f, 0);
			else if (currentDir == 'e') probePos += new Vector3(1f, 0, 0);
			else if (currentDir == 'w') probePos += new Vector3(-1f, 0, 0);


			probeInstance = Instantiate(probePrefab, probePos, Quaternion.identity) as GameObject;

            LinkStatus.key_count--;

			if( probeInstance != null) Destroy(probeInstance);

			probeInstance = Instantiate(probePrefab, probePos, Quaternion.identity) as GameObject;

		}
	}

	void OnCollisionEnter (Collision coll) {

		if (coll.gameObject.tag == "Rupee") {
            LinkStatus.rupee_count++;
			Destroy (coll.gameObject);
		}
        else if (coll.gameObject.tag == "Bomb")
        {
            LinkStatus.bomb_count++;
            Destroy(coll.gameObject);
        }
        else if (coll.gameObject.tag == "Key")
        {
            LinkStatus.key_count++;
            Destroy(coll.gameObject);
        }
		else if (coll.gameObject.tag == "Door") {

			if( transform.position.y <= 1f )
				return;

			Vector3 newPos = transform.position;
			doorCount += 1;

			switch(doorCount) {
			case 1:
				doorBc1 = coll.gameObject.GetComponent<BoxCollider> ();
				doorBc1.enabled = false;
				break;
			case 2:
				doorBc2 = coll.gameObject.GetComponent<BoxCollider> ();
				doorBc2.enabled = false;
				break;
			case 3:
				doorBc3 = coll.gameObject.GetComponent<BoxCollider> ();
				doorBc3.enabled = false;
				break;
			case 4:
				doorBc4 = coll.gameObject.GetComponent<BoxCollider> ();
				doorBc4.enabled = false;
				break;
			default:
				break;
			}

			if ( (currentDir == 'e' || currentDir == 'w') && doorCount == 2) {
				if (currentDir == 'e') {
					newPos.x -= 1;
				} else if (currentDir == 'w') {
					newPos.x += 1;
				}
			}
			else if ((currentDir == 'n' || currentDir == 's') && doorCount == 4) {
				if (currentDir == 'n') {
					newPos.y -= 1;
				} else if (currentDir == 's') {
					newPos.y += 1;
				}
			}

			if (currentDir == 'n') {
				newPos.y = (Mathf.Floor (newPos.y / 11f) + 1f) * 11f;
			} else if (currentDir == 's') {
				newPos.y = (Mathf.Floor (newPos.y / 11f) * 11f) - 1f;
			} else if (currentDir == 'e') {
				newPos.x = (Mathf.Floor (newPos.x / 16f) + 1f) * 16f;
			} else if (currentDir == 'w') {
				newPos.x = (Mathf.Floor (newPos.x / 16f) * 16f) - 1f;
			}

			throughDoorPos = newPos;
			CameraLeadMovement.cameraMoving = true;
			throughDoor = true;
			positionError = Vector2.zero;
		} 
		else if (coll.gameObject.tag == "Probe") {
			Destroy(coll.gameObject);
		}

		else {

			Vector3 newPos = transform.position;
			newPos.x = Mathf.Round(newPos.x / 0.5f) * 0.5f;
			newPos.y = Mathf.Round(newPos.y / 0.5f) * 0.5f;
			transform.position = newPos;
			positionError.x = 0.0f;
			positionError.y = 0.0f;

		}
	}
}
