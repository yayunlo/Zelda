using UnityEngine;
using System.Collections;


public class LinkMovement : MonoBehaviour
{

	static Sprite[] linkSprite;

	public float velocityFactor = 1.0f;
	int hitCooldown = 0;

	public static Weapon weaponMode = Weapon.SWORD;
	public GameObject weaponInstance;
	bool useWeapon = false;


	public GameObject swordPrefab;
	public int swordCooldown = 0;
	public Sprite swordUp, swordDown, swordRight, swordLeft;

	public GameObject arrowPrefab;
	public int arrowCooldown = 0;

	public GameObject bombPrefab;
	public int bombCooldown = 0;

	public float positionCorrectParam = 0.025f;
	public static Vector3 newRoomStartPos;

	public GameObject probePrefab;
	public GameObject probeInstance;


	Vector2 positionError = Vector2.zero;
	bool throughDoor = false;
	Vector3 throughDoorPos = Vector3.zero;
	BoxCollider doorBc1, doorBc2, doorBc3, doorBc4;

	int doorCount = 0;

	// Movement Variables
	public static char currentDir = 'n'; // Choose from "{n, s, e, w}"

	// Status Reference
	private Link status;

	// Use this for initialization
	void Start()
	{
		// linkSprite = Resources.LoadAll<Sprite>("link_sprites");
		status = GetComponent<Link>();

	}

	// Update is called once per frame
	void Update()
	{

		if (!throughDoor)
		{
			/*
			float horizontalVelocity = Input.GetAxis("Horizontal");
			float verticalVelocity = Input.GetAxis("Vertical");

			// Right now all the weapon share a common cd
			if (useWeapon || hitCooldown > 0)
			{
				horizontalVelocity = 0f;
				verticalVelocity = 0f;
			}
			// Link should not be able to move diagonally
			else if (horizontalVelocity != 0.0f)
			{
				verticalVelocity = 0.0f;
			}

			// Set direction
			if (horizontalVelocity > 0) currentDir = 'e';
			else if (horizontalVelocity < 0) currentDir = 'w';
			else if (verticalVelocity > 0) currentDir = 'n';
			else if (verticalVelocity < 0) currentDir = 's';

			// Update velocity
			GetComponent<Rigidbody>().velocity = (new Vector3(horizontalVelocity, verticalVelocity, 0)) * velocityFactor;

			// Animate
			GetComponent<Animator>().SetFloat("horizontal_vel", horizontalVelocity);
			GetComponent<Animator>().SetFloat("vertical_vel", verticalVelocity);

			if (horizontalVelocity != 0.0f)
			{
				positionError.x = (horizontalVelocity > 0.0f) ? 1.0f : -1.0f;
			}
			else if (verticalVelocity != 0.0f)
			{
				positionError.y = (verticalVelocity > 0.0f) ? 1.0f : -1.0f;
			}

			// Correct position to correct track
			if (positionError.x != 0.0f && horizontalVelocity == 0.0f)
			{

				Vector3 newPos = transform.position;

				float idealX = Mathf.Ceil(transform.position.x / 0.5f);
				if (positionError.x < 0.0f)
				{
					idealX = idealX - 1.0f;
				}

				if (Mathf.Abs(idealX * 0.5f - transform.position.x) > positionCorrectParam)
				{
					newPos.x = transform.position.x + (positionError.x * positionCorrectParam);
				}
				else
				{
					newPos.x = idealX * 0.5f;
					positionError.x = 0.0f;
				}
				transform.position = newPos;
			}
			if (positionError.y != 0.0f && verticalVelocity == 0.0f)
			{

				Vector3 newPos = transform.position;

				float idealY = Mathf.Ceil(transform.position.y / 0.5f);
				if (positionError.y < 0.0f)
				{
					idealY = idealY - 1.0f;
				}

				if (Mathf.Abs(idealY * 0.5f - transform.position.y) > positionCorrectParam)
				{
					newPos.y = transform.position.y + (positionError.y * positionCorrectParam);

				}
				else
				{
					newPos.y = idealY * 0.5f;
					positionError.y = 0.0f;
				}
				transform.position = newPos;
			}
			*/
		}
		else
		{

			if (!CameraLeadMovement.cameraMoving)
			{
				transform.position = Vector3.MoveTowards(transform.position, newRoomStartPos, velocityFactor * Time.deltaTime);
			}
			else
			{
				transform.position = Vector3.MoveTowards(transform.position, throughDoorPos, velocityFactor * Time.deltaTime);
			}

			// check which posision is link in
			if (currentDir == 'n' && (Mathf.Floor(transform.position.y / 11f) * 11f + 1f) == transform.position.y)
			{
				doorBc3.enabled = true;
				doorBc4.enabled = true;
			}
			else if (currentDir == 's' && (Mathf.Ceil(transform.position.y / 11f) * 11f - 2f) == transform.position.y)
			{
				doorBc3.enabled = true;
				doorBc4.enabled = true;
			}
			else if (currentDir == 'e' && (Mathf.Floor(transform.position.x / 16f) * 16f + 1f) == transform.position.x)
			{

			}
			else if (currentDir == 'w' && (Mathf.Ceil(transform.position.x / 16f) * 16f - 2f) == transform.position.x)
			{
			}
			doorBc1.enabled = true;
			doorBc2.enabled = true;
			throughDoor = false;
			doorCount = 0;


		}

		// If we're not moving, don't animate.
		//if (newVelocity.magnitude == 0)
		/*
		if (!useWeapon && hitCooldown == 0 && positionError.x == 0.0f && positionError.y == 0.0f)
			GetComponent<Animator>().speed = 0.00000001f;
		else
			GetComponent<Animator>().speed = 1.0f;
		*/
		Combat();
		UseTools();
		hitReaction();


	}

	void MovementWithinRoom()
	{
		float horizontalVelocity = Input.GetAxis("Horizontal");
		float verticalVelocity = Input.GetAxis("Vertical");

		switch (status.actionState)
		{
			case ActionState.NONE:
			case ActionState.MOVING:
				verticalVelocity = (horizontalVelocity == 0f) ? verticalVelocity : 0f;
				break;
			case ActionState.ADJUSTING:
			case ActionState.RECASTING:
				horizontalVelocity = 0f;
				verticalVelocity = 0f;
				break;
			default:
				break;
		}

		// Right now all the weapon share a common cd
		/*
		if (status.actionState == ActionState.COMBATING || hitCooldown > 0)
		{
			horizontalVelocity = 0f;
			verticalVelocity = 0f;
		}
		// Link should not be able to move diagonally
		else if (horizontalVelocity != 0.0f)
		{
			verticalVelocity = 0.0f;
		}
		*/
		// Set direction
		/*
		if (horizontalVelocity > 0) currentDir = 'e';
		else if (horizontalVelocity < 0) currentDir = 'w';
		else if (verticalVelocity > 0) currentDir = 'n';
		else if (verticalVelocity < 0) currentDir = 's';
		*/

		// Update velocity
		GetComponent<Rigidbody>().velocity = (new Vector3(horizontalVelocity, verticalVelocity, 0)) * velocityFactor;

		// Animate
		GetComponent<Animator>().SetFloat("horizontal_vel", horizontalVelocity);
		GetComponent<Animator>().SetFloat("vertical_vel", verticalVelocity);
	}

	void Combat()
	{

		if (!useWeapon && Input.GetKeyDown(KeyCode.F))
			weaponMode = Weapon.FLYSWORD;

		if (!useWeapon && Input.GetKeyDown(KeyCode.C))
		{
			if (weaponMode == Weapon.SWORD || weaponMode == Weapon.FLYSWORD)
				weaponMode = Weapon.BOW;
			else if (weaponMode == Weapon.BOW)
				weaponMode = Weapon.SWORD;

			print(weaponMode);
		}

		if (weaponMode == Weapon.SWORD || weaponMode == Weapon.FLYSWORD)
		{

			// Progress the sword cooldown.
			if (swordCooldown > 0)
				swordCooldown--;
			else
			{
				if (weaponInstance != null)
				{
					if (weaponMode == Weapon.SWORD)
					{
						Destroy(weaponInstance);
					}
					else if (weaponMode == Weapon.FLYSWORD)
					{
						weaponInstance.GetComponent<Sword>().towardDir = currentDir;
						weaponInstance.GetComponent<Sword>().fly = true;
						weaponInstance = null;
					}
				}

				weaponInstance = null;
				gameObject.GetComponent<Animator>().SetInteger("ver_weapon", 0);
				gameObject.GetComponent<Animator>().SetInteger("hor_weapon", 0);
				useWeapon = false;

			}
			if (Input.GetKeyDown(KeyCode.Space) && weaponInstance == null)
			{

				// Spawn the sword into being
				weaponInstance = Instantiate(swordPrefab, transform.position, Quaternion.identity) as GameObject;

				swordCooldown = 10;
				useWeapon = true;

				// Adjust sword angle and position based on current facing direction
				if (currentDir == 'n')
				{
					weaponInstance.transform.position += new Vector3(0, 1, 0);

				}
				else if (currentDir == 'e')
				{
					weaponInstance.transform.position += new Vector3(1, 0, 0);
					weaponInstance.transform.Rotate(new Vector3(0, 0, 1), 270);
				}
				else if (currentDir == 's')
				{
					weaponInstance.transform.position += new Vector3(0, -1, 0);
					weaponInstance.transform.Rotate(new Vector3(0, 0, 1), 180);
				}
				else if (currentDir == 'w')
				{
					weaponInstance.transform.position += new Vector3(-1, 0, 0);
					weaponInstance.transform.Rotate(new Vector3(0, 0, 1), 90);
				}

			}
		}
		else if (weaponMode == Weapon.BOW)
		{

			if (arrowCooldown == 0)
				useWeapon = false;
			else if (arrowCooldown > 0)
				arrowCooldown--;

			if (Input.GetKeyDown(KeyCode.Space) && !useWeapon)
			{

				// Spawn the sword into being
				GameObject weaponInstance = Instantiate(arrowPrefab, transform.position, Quaternion.identity) as GameObject;
				arrowCooldown = 3;
				useWeapon = true;


				// Adjust arrow angle and position based on current facing direction
				if (currentDir == 'n')
				{
					weaponInstance.transform.position += new Vector3(0, 1, 0);
					weaponInstance.GetComponent<Arrow>().towardDir = 'n';
				}
				else if (currentDir == 'e')
				{
					weaponInstance.transform.position += new Vector3(1, 0, 0);
					weaponInstance.transform.Rotate(new Vector3(0, 0, 1), 270);
					weaponInstance.GetComponent<Arrow>().towardDir = 'e';
				}
				else if (currentDir == 's')
				{
					weaponInstance.transform.position += new Vector3(0, -1, 0);
					weaponInstance.transform.Rotate(new Vector3(0, 0, 1), 180);
					weaponInstance.GetComponent<Arrow>().towardDir = 's';
				}
				else if (currentDir == 'w')
				{
					weaponInstance.transform.position += new Vector3(-1, 0, 0);
					weaponInstance.transform.Rotate(new Vector3(0, 0, 1), 90);
					weaponInstance.GetComponent<Arrow>().towardDir = 'w';
				}

				weaponInstance = null;
			}
		}


		if (useWeapon)
		{
			if (currentDir == 'n')
			{
				gameObject.GetComponent<Animator>().SetInteger("ver_weapon", 1);
				gameObject.GetComponent<Animator>().SetInteger("hor_weapon", 0);
			}
			else if (currentDir == 's')
			{
				gameObject.GetComponent<Animator>().SetInteger("ver_weapon", -1);
				gameObject.GetComponent<Animator>().SetInteger("hor_weapon", 0);
			}
			else if (currentDir == 'e')
			{
				gameObject.GetComponent<Animator>().SetInteger("ver_weapon", 0);
				gameObject.GetComponent<Animator>().SetInteger("hor_weapon", 1);
			}
			else if (currentDir == 'w')
			{
				gameObject.GetComponent<Animator>().SetInteger("ver_weapon", 0);
				gameObject.GetComponent<Animator>().SetInteger("hor_weapon", -1);
			}

		}


	}

	void UseTools()
	{

		if (Input.GetKeyDown(KeyCode.K) && status.keyCount > 0)
		{

			Vector3 probePos = transform.position;

			if (currentDir == 'n') probePos += new Vector3(0, 1f, 0);
			else if (currentDir == 's') probePos += new Vector3(0, -1f, 0);
			else if (currentDir == 'e') probePos += new Vector3(1f, 0, 0);
			else if (currentDir == 'w') probePos += new Vector3(-1f, 0, 0);


			probeInstance = Instantiate(probePrefab, probePos, Quaternion.identity) as GameObject;

			status.keyCount--;

			if (probeInstance != null) Destroy(probeInstance);

			probeInstance = Instantiate(probePrefab, probePos, Quaternion.identity) as GameObject;

		}

		if (Input.GetKeyDown(KeyCode.B) && status.bombCount > 0)
		{
			Vector3 placeBombPos = transform.position;

			if (currentDir == 'n') placeBombPos += new Vector3(0, 1f, 0);
			else if (currentDir == 's') placeBombPos += new Vector3(0, -1f, 0);
			else if (currentDir == 'e') placeBombPos += new Vector3(1f, 0, 0);
			else if (currentDir == 'w') placeBombPos += new Vector3(-1f, 0, 0);

			GameObject bombInstance = Instantiate(bombPrefab, placeBombPos, Quaternion.identity) as GameObject;

			status.bombCount--;

		}
	}

	void hitReaction()
	{
		if (hitCooldown > 0)
		{

			if (currentDir == 'n')
			{
				gameObject.GetComponent<Animator>().SetInteger("ver_hit", 1);
				gameObject.GetComponent<Animator>().SetInteger("hor_hit", 0);
			}
			else if (currentDir == 's')
			{
				gameObject.GetComponent<Animator>().SetInteger("ver_hit", -1);
				gameObject.GetComponent<Animator>().SetInteger("hor_hit", 0);
			}
			else if (currentDir == 'e')
			{
				gameObject.GetComponent<Animator>().SetInteger("ver_hit", 0);
				gameObject.GetComponent<Animator>().SetInteger("hor_hit", 1);
			}
			else if (currentDir == 'w')
			{
				gameObject.GetComponent<Animator>().SetInteger("ver_hit", 0);
				gameObject.GetComponent<Animator>().SetInteger("hor_hit", -1);
			}
			hitCooldown--;
		}
		else
		{
			gameObject.GetComponent<Animator>().SetInteger("ver_hit", 0);
			gameObject.GetComponent<Animator>().SetInteger("hor_hit", 0);
		}

	}

	void OnCollisionEnter(Collision coll)
	{

		if (coll.gameObject.tag == "Rupee")
		{
			status.rupeeCount++;
			Destroy(coll.gameObject);
		}
		else if (coll.gameObject.tag == "Bomb")
		{
			status.bombCount++;
			Destroy(coll.gameObject);
		}
		else if (coll.gameObject.tag == "Key")
		{
			status.keyCount++;
			Destroy(coll.gameObject);
		}
		else if (coll.gameObject.tag == "Enemy")
		{
			status.health--;
		}
		else if (coll.gameObject.tag == "Door")
		{

			if (transform.position.y <= 1f)
				return;

			Vector3 newPos = transform.position;
			doorCount += 1;

			switch (doorCount)
			{
				case 1:
					doorBc1 = coll.gameObject.GetComponent<BoxCollider>();
					doorBc1.enabled = false;
					break;
				case 2:
					doorBc2 = coll.gameObject.GetComponent<BoxCollider>();
					doorBc2.enabled = false;
					break;
				case 3:
					doorBc3 = coll.gameObject.GetComponent<BoxCollider>();
					doorBc3.enabled = false;
					break;
				case 4:
					doorBc4 = coll.gameObject.GetComponent<BoxCollider>();
					doorBc4.enabled = false;
					break;
				default:
					break;
			}

			if ((currentDir == 'e' || currentDir == 'w') && doorCount == 2)
			{
				if (currentDir == 'e')
				{
					newPos.x -= 1;
				}
				else if (currentDir == 'w')
				{
					newPos.x += 1;
				}
			}
			else if ((currentDir == 'n' || currentDir == 's') && doorCount == 4)
			{
				if (currentDir == 'n')
				{
					newPos.y -= 1;
				}
				else if (currentDir == 's')
				{
					newPos.y += 1;
				}
			}

			if (currentDir == 'n')
			{
				newPos.y = (Mathf.Floor(newPos.y / 11f) + 1f) * 11f;
			}
			else if (currentDir == 's')
			{
				newPos.y = (Mathf.Floor(newPos.y / 11f) * 11f) - 1f;
			}
			else if (currentDir == 'e')
			{
				newPos.x = (Mathf.Floor(newPos.x / 16f) + 1f) * 16f;
			}
			else if (currentDir == 'w')
			{
				newPos.x = (Mathf.Floor(newPos.x / 16f) * 16f) - 1f;
			}

			throughDoorPos = newPos;
			CameraLeadMovement.cameraMoving = true;
			throughDoor = true;
			positionError = Vector2.zero;
		}
		else if (coll.gameObject.tag == "Probe")
		{
			Destroy(coll.gameObject);
		}

		else
		{

			Vector3 newPos = transform.position;
			newPos.x = Mathf.Round(newPos.x / 0.5f) * 0.5f;
			newPos.y = Mathf.Round(newPos.y / 0.5f) * 0.5f;
			transform.position = newPos;
			positionError.x = 0.0f;
			positionError.y = 0.0f;

		}
	}

	void OnTriggerEnter(Collider coll)
	{
		if (coll.gameObject.tag == "Enemy")
		{
			print("hit by enemy");
			status.health--;
			hitCooldown = 18;
		}

	}

	public bool isAdjusted()
	{
		return true;
	}
}
