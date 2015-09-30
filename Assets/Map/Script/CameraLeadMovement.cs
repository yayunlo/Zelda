using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Room
{
    // A room contains a list of enemies
    // A list of items that can be picked up
    public string enemyType;
    public int enemyNum;
    public Vector3[] initialPos;

    public Room(string type, int num, Vector3[] pos)
    {
        enemyType = type;
        enemyNum = num;
        initialPos = pos;
    }
};

public class CameraLeadMovement : MonoBehaviour {

	public GameObject stalfosPrefab, keesePrefab, gelPrefab, goriyaPrefab, wallmasterPrefab, aquamentusPrefab, bladetrapePrefab;

	public static List<Room> enemies = new List<Room>();
	public static Dictionary<string, GameObject> enemyDictionary = new Dictionary<string, GameObject>();

	public static int roomId = 1;
	public GameObject[] enemyGameObject;



	public float speed = 3f;
	public GameObject Link;

	Vector2 currRoom = Vector2.zero;
	Vector3 newPos = Vector3.zero;
	public static bool cameraMoving = true;

	int enimyOutCountdown = 0;
	bool enimyOut = false;

	// Use this for initialization
	void Start () {
		Link = GameObject.Find("Link");
		currRoom.x = Mathf.Floor (Link.transform.position.x / 16.0f) * 16.0f;
		currRoom.y = Mathf.Floor (Link.transform.position.y / 11.0f) * 11.0f;
		cameraMoving = false;

		// initialize enimies
		Vector3[] enemyPos;

		enemyPos = new Vector3[3];
		enemyPos [0] = new Vector3 (4f,5f,0f);
		enemyPos [1] = new Vector3 (6f,3f,0f);
		enemyPos [2] = new Vector3 (8f,7f,0f);
		enemies.Add (new Room ("keese", 3, enemyPos)); // Room 0: Entry

		enemyPos = new Vector3[1];
		enemies.Add (new Room ("nothing", 0, enemyPos)); // Room 1
		enemies.Add (new Room ("stalfos", 0, enemyPos)); // Room 2
		enemies.Add (new Room ("stalfos", 0, enemyPos)); // Room 3
		enemies.Add (new Room ("keese", 0, enemyPos)); // Room 4
		enemies.Add (new Room ("stalfos", 0, enemyPos)); // Room 5
		enemies.Add (new Room ("keese", 0, enemyPos)); // Room 6
		enemies.Add (new Room ("nothing", 0, enemyPos)); // Room 7: old man
		enemies.Add (new Room ("gel", 0, enemyPos)); // Room 8
		enemies.Add (new Room ("gel", 0, enemyPos)); // Room 9
		enemies.Add (new Room ("goriya", 0, enemyPos)); // Room 10
		enemies.Add (new Room ("wallmaster", 0, enemyPos)); // Room 11
		enemies.Add (new Room ("stalfos", 0, enemyPos)); // Room 12
		enemies.Add (new Room ("aquamentus", 0, enemyPos)); // Room 13: Boss
		enemies.Add (new Room ("nothing", 0, enemyPos)); // Room 14
		enemies.Add (new Room ("bladetrape", 0, enemyPos)); // Room 15
		enemies.Add (new Room ("goriya", 0, enemyPos)); // Room 16

		enemyDictionary.Add ("keese", keesePrefab);
		enemyDictionary.Add ("stalfos", stalfosPrefab);
		enemyDictionary.Add ("gel", gelPrefab);
		enemyDictionary.Add ("goriya", goriyaPrefab);
		enemyDictionary.Add ("wallmaster", wallmasterPrefab);
		enemyDictionary.Add ("aquamentus", aquamentusPrefab);
		enemyDictionary.Add ("bladetrape", bladetrapePrefab);

	}
	
	// Update is called once per frame
	void Update () {
		Vector2 LinkRoom = new Vector2( Mathf.Floor (Link.transform.position.x / 16.0f) * 16f, Mathf.Floor(Link.transform.position.y / 11.0f) * 11f );
	
		if( currRoom != LinkRoom ) {
			float step = speed * Time.deltaTime;
			newPos = new Vector3(LinkRoom.x + 7.75f, LinkRoom.y + 6.5f, 0.0f);
			transform.position = Vector3.MoveTowards(transform.position, newPos, step);
			cameraMoving = true;

			destroyEnemies();
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

			generateEnemies(currRoom);

		}

	}

	void generateEnemies (Vector2 currRoom) {

		float roomY = Mathf.Floor ( transform.position.y / 11f );
		float roomX = Mathf.Floor ( transform.position.x / 16f);
		roomId = -1;

		if (roomY == 0f) {
			if(roomX == 1f) roomId = 0;
			else if (roomX == 2f) roomId = 1;
			else if (roomX == 3f) roomId = 2;
		} 
		else if (roomY == 1f) {
			roomId = 3;
		}
		else if (roomY == 2f) {
			if(roomX == 1f) roomId = 4;
			else if (roomX == 2f) roomId = 5;
			else if (roomX == 3f) roomId = 6;
		} 
		else if (roomY == 3f) {
			if(roomX == 0f) roomId = 7;
			else if (roomX == 1f) roomId = 8;
			else if (roomX == 2f) roomId = 9;
			else if (roomX == 3f) roomId = 10;
			else if (roomX == 4f) roomId = 11;
		} 
		else if (roomY == 4f) {
			if(roomX == 2f) roomId = 12;
			else if (roomX == 4f) roomId = 13;
			else if (roomX == 5f) roomId = 14;
		} 
		else if (roomY == 5f) {
			if(roomX == 1f) roomId = 15;
			else if (roomX == 2f) roomId = 16;
		}

//		print (roomId);
//		print (enemies [roomId].enemyType);
//		print (enemies [roomId].enemyNum);

		int enemyNum = enemies [roomId].enemyNum;
		string enemyType = enemies [roomId].enemyType;
		Vector3[] enemyPos = enemies [roomId].initialPos;

		// If no enemy remain in this room, return
		if (enemyNum <= 0) {
			return;
		} 
		else {

			enemyGameObject = new GameObject[enemyNum];
			for (int i = 0; i < enemyNum; ++i) {
				Vector3 pos = enemyPos[i];
				pos.x += currRoom.x;
				pos.y += currRoom.y;
				GameObject enemy = Instantiate(enemyDictionary[enemyType], pos, Quaternion.identity) as GameObject;
				enemyGameObject[i] = enemy;
			}

		}


	}

	void destroyEnemies () {

		int enemyNum = enemies [roomId].enemyNum;
		int enemyLeft = 0;
		for (int i = 0; i < enemyNum; ++i) {
			if(enemyGameObject[i] != null) {
				Destroy(enemyGameObject[i]);
				enemyLeft++;
			}
		}
		enemies [roomId].enemyNum = enemyLeft;
	}
}
