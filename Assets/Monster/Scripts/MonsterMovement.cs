using UnityEngine;
using System.Collections;

public class MonsterMovement : MonoBehaviour {

    // Pre-written movement script should only contain 'n', 's', 'e', 'w' all in lower case
    public string movementScript;
    // Current Direction should be one of 'n', 's', 'e', 'w'
    public char currentDir;

    public float velocityFactor;

    private Vector3 velocity;

    /*---------------------------------------------------------------------------------------*/

    // cursor for reading movement script
    private int movementReader;

    /*---------------------------------------------------------------------------------------*/

    // timeBuffer for fixed update
    private float timeBuffer;

    private const float updateTime = 1.0f;

    /*---------------------------------------------------------------------------------------*/

    private float lockPosX;

    private float lockPosY;
    
	// Use this for initialization
	void Start ()
    {
        // Init the cursor to 0
        movementReader = 0;
        // Init current direction to the first of the intended direction
        currentDir = movementScript[movementReader];
        // Init X, Y lock position
        lockPosX = transform.position.x;
        lockPosY = transform.position.y;
    }

	
	// Update is called once per frame
	void Update () {
        
        switch (currentDir)
        {
            case 'n':
                velocity = Vector3.up;
                break;
            case 's':
                velocity = Vector3.up * -1;
                break;
            case 'e':
                velocity = Vector3.right;
                break;
            case 'w':
                velocity = Vector3.right * -1;
                break;
            default:
                Debug.Log("Invalid move, check movement script on" + name);
                break;
        }
        
        GetComponent<Rigidbody>().velocity = velocity * velocityFactor;

        // Transformation Error Correction
        switch (currentDir)
        {
            case 'n':
            case 's':
                transform.position = new Vector3(lockPosX, transform.position.y, 0);
                break;
            case 'e':
            case 'w':
                transform.position = new Vector3(transform.position.x, lockPosY, 0);
                break;
            default:
                Debug.Log("Invalid move, check movement script on" + name);
                break;
        }
        // Lock rotation
        // transform.rotation = Quaternion.Euler(lockPosVar, lockPosVar, lockPosVar);
        

        timeBuffer += Time.deltaTime;

        if (timeBuffer > updateTime)
        {
            // Correct the position of monster to the nearest int
            transform.position = new Vector3(Mathf.Round(transform.position.x),
                                             Mathf.Round(transform.position.y),
                                             0);
            // Update lock position info
            lockPosX = transform.position.x;
            lockPosY = transform.position.y;
            movementReader = (movementReader == movementScript.Length - 1) ? 0 : movementReader + 1;
            currentDir = movementScript[movementReader];
            timeBuffer = 0.0f;
        }

    }

	void OnCollisionEnter(Collision coll)
	{

	}
}
