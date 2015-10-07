using UnityEngine;
using System.Collections;

public class FireTrack : MonoBehaviour {

	private const float lifeTime = 10.0f;
	private float age;

	// Use this for initialization
	void Start () {
		age = 0.0f;
	}
	
	// Update is called once per frame
	void Update () {
		age += Time.deltaTime;
		if (age >= lifeTime)
		{
			Destroy(gameObject);
		}
	}

	void OnTriggerEnter(Collider collider)
	{
		if (collider.gameObject.tag == "Enemy")
		{
			Destroy(collider.gameObject);
			Destroy(gameObject);
		}
	}
}
