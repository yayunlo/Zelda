using UnityEngine;
using System.Collections;

public class Sword : MonoBehaviour {

	public GameObject swordLightPrefab;

	public bool fly = false;
	public char towardDir = 'n';
	public float speed = 3f;
	public Sprite normalSprite, shineSprite;
	public int shineRate = 2;
	public float lightRange = 1f;
	int shineCount = 0;

	bool setRigidBody = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (fly == true) {
//			if (!setRigidBody) {
//				gameObject.AddComponent<Rigidbody>().useGravity = false;
//				setRigidBody = true;
//			}
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

			shineCount++;
			if(shineCount == shineRate) {
				gameObject.GetComponent<SpriteRenderer>().sprite = shineSprite;
			}
			else if (shineCount == shineRate*2) {
				gameObject.GetComponent<SpriteRenderer>().sprite = normalSprite;
				shineCount = 0;
			}
			
		}
	}

	void OnTriggerEnter ( Collider coll ) {
		if (coll.gameObject.tag == "Wall" 
			|| coll.gameObject.tag == "Door" 
			|| coll.gameObject.tag == "LockedDoor") {


			GameObject swordLight;

			// 4 dirrection light
			swordLight = Instantiate(swordLightPrefab, transform.position, Quaternion.identity) as GameObject;
			swordLight.transform.position += new Vector3(0.25f, 0.25f, 0f);
			swordLight.gameObject.GetComponent<SwordLight>().destination = transform.position + new Vector3(lightRange,lightRange,0f);
			swordLight.gameObject.GetComponent<SwordLight>().dir = 1;

			swordLight = Instantiate(swordLightPrefab, transform.position, Quaternion.identity) as GameObject;
			swordLight.transform.position += new Vector3(-0.25f, 0.25f, 0f);
			swordLight.gameObject.GetComponent<SwordLight>().destination = transform.position + new Vector3(-lightRange,lightRange,0f);
			swordLight.gameObject.GetComponent<SwordLight>().dir = 2;

			swordLight = Instantiate(swordLightPrefab, transform.position, Quaternion.identity) as GameObject;
			swordLight.transform.position += new Vector3(-0.25f, -0.25f, 0f);
			swordLight.gameObject.GetComponent<SwordLight>().destination = transform.position + new Vector3(-lightRange,-lightRange,0f);
			swordLight.gameObject.GetComponent<SwordLight>().dir = 3;

			swordLight = Instantiate(swordLightPrefab, transform.position, Quaternion.identity) as GameObject;
			swordLight.transform.position += new Vector3(0.25f, -0.25f, 0f);
			swordLight.gameObject.GetComponent<SwordLight>().destination = transform.position + new Vector3(lightRange,-lightRange,0f);
			swordLight.gameObject.GetComponent<SwordLight>().dir = 4;

			Destroy (gameObject);

		}
	}
}
