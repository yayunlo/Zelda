using UnityEngine;
using System.Collections;

public class CameraLeadMovement : MonoBehaviour {

	public GameObject Link;
	Vector2 currRoom = Vector2.zero;
	public float speed = 3f;

	// Use this for initialization
	void Start () {
		Link = GameObject.Find("Link");
		currRoom.x = Mathf.Floor (Link.transform.position.x / 16.0f) * 16.0f;
		currRoom.y = Mathf.Floor (Link.transform.position.x / 11.0f) * 11.0f;
	}
	
	// Update is called once per frame
	void Update () {
		Vector2 LinkRoom = new Vector2( Mathf.Floor (Link.transform.position.x / 16.0f) * 16f, Mathf.Floor(Link.transform.position.y / 11.0f) * 11f );
	
		if( currRoom != LinkRoom ) {
			float step = speed * Time.deltaTime;
			Vector3 newPos = new Vector3(LinkRoom.x + 8f, LinkRoom.y + 5.0f, 0.0f);
			transform.position = Vector3.MoveTowards(transform.position, newPos, step);
		}

	}
}
