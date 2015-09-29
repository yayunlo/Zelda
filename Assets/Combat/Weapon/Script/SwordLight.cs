using UnityEngine;
using System.Collections;

public class SwordLight : MonoBehaviour {

	public Vector3 destination;
	public int dir;
	public float speed = 3f;
	public int shineRate = 2;

	static Sprite[] spriteArray;
	public Sprite normalSprite, shineSprite;

	int shineCount = 0;

	// Use this for initialization
	void Start () {
		spriteArray = Resources.LoadAll<Sprite>("link_sprites");

		if (dir == 1) {
			normalSprite = spriteArray [173];
			shineSprite = spriteArray [175];
		} else if (dir == 2) {
			normalSprite = spriteArray [172];
			shineSprite = spriteArray [174];
		} else if (dir == 3) {
			normalSprite = spriteArray [187];
			shineSprite = spriteArray[189];
		} else if (dir == 4) {
			normalSprite = spriteArray[188];
			shineSprite = spriteArray[190];
		}


	}
	
	// Update is called once per frame
	void Update () {
		transform.position = Vector3.MoveTowards (transform.position, destination, speed * Time.deltaTime);

		shineCount++;
		if (shineCount == shineRate) {
			gameObject.GetComponent<SpriteRenderer>().sprite = shineSprite;
		}
		else if (shineCount == shineRate*2) {
			gameObject.GetComponent<SpriteRenderer>().sprite = normalSprite;
			shineCount = 0;
		}

		if (transform.position == destination) {
			Destroy(gameObject);
		}
	}
}
