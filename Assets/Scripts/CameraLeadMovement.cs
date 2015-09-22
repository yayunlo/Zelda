using UnityEngine;
using System.Collections;

public class CameraLeadMovement : MonoBehaviour {

	public float speed = 3f;
	public GameObject Link;

	Vector2 currRoom = Vector2.zero;
	Vector3 newPos = Vector3.zero;
	public static bool cameraMoving = true;

	// Use this for initialization
	void Start () {
		Link = GameObject.Find("Link");
		currRoom.x = Mathf.Floor (Link.transform.position.x / 16.0f) * 16.0f;
		currRoom.y = Mathf.Floor (Link.transform.position.y / 11.0f) * 11.0f;
		cameraMoving = false;
	}
	
	// Update is called once per frame
	void Update () {
		Vector2 LinkRoom = new Vector2( Mathf.Floor (Link.transform.position.x / 16.0f) * 16f, Mathf.Floor(Link.transform.position.y / 11.0f) * 11f );
	
		if( currRoom != LinkRoom ) {
			float step = speed * Time.deltaTime;
			newPos = new Vector3(LinkRoom.x + 8f, LinkRoom.y + 4.0f, 0.0f);
			transform.position = Vector3.MoveTowards(transform.position, newPos, step);
			cameraMoving = true;
		}

		if (cameraMoving && transform.position == newPos) {
			currRoom = LinkRoom;
			Vector3 linkNewPos = Link.transform.position;

			if(LinkMovement.currentDir == 'n') {
				linkNewPos.y += 1f;
			} 
			else if (LinkMovement.currentDir == 's' ) {
				linkNewPos.y -= 1f;
			}
			else if(LinkMovement.currentDir == 'e') {
				linkNewPos.x += 1f;
			} 
			else if (LinkMovement.currentDir == 'w' ) {
				linkNewPos.x -= 1f;
			}
			LinkMovement.newRoomStartPos = linkNewPos;

			cameraMoving = false;
			newPos = Vector3.zero;
		}

	}
}
