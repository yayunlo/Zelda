using UnityEngine;
using System.Collections;

public class CustomPhysics : MonoBehaviour
{
	private const float positionErrorFixRate = 0.025f;

	//public interface
	public bool isAdjusted;
	//public Vector3 adjustingVelocity;

	//private bool isErrorSet;
	//private Direction errorDirection;
	private Vector3 errorDirectionVector;
	private bool isTargetSet;
	private Vector3 targetGridPoint;
	private Link playerPawn;

	// Use this for initialization
	void Start()
	{
		//isAdjusted = true;
		//adjustingVelocity = Vector3.zero;
		//isErrorSet = false;
		//errorDirection = Direction.STAY;
		errorDirectionVector = Vector3.zero;
		isTargetSet = false;
		targetGridPoint = Vector3.zero;
		playerPawn = GetComponent<Link>();

	}

	// Update is called once per frame
	void Update()
	{
		// Detect & set Error
		/*
		if (!isErrorSet)
		{
			if (playerPawn.currentDirection != Direction.STAY)
			{
				errorDirection = playerPawn.currentDirection;
				isAdjusted = false;
				isErrorSet = true;
			}
		}
		*/

		// Test Link state
		if (GetComponent<Link>().actionState != ActionState.ADJUSTING)
		{
			return;
		}

		// Calculate target position
		if (!isTargetSet)
		{
			//print(GetComponent<Rigidbody>().velocity);

			//errorDirectionVector = GetComponent<Rigidbody>().velocity.normalized;
			//print(errorDirectionVector);
			//GetComponent<Rigidbody>().velocity = Vector3.zero;

			//isAdjusted = false;

			targetGridPoint = transform.position;
			//targetGridPoint = targetGridPoint + 0.5f * playerPawn.GetComponent<Rigidbody>().velocity.normalized; 
			switch (playerPawn.currentDirection)
			{
				case Direction.NORTH:
					targetGridPoint.y = Mathf.Ceil(transform.position.y / 0.5f) * 0.5f;
					break;
				case Direction.SOUTH:
					targetGridPoint.y = Mathf.Floor(transform.position.y / 0.5f) * 0.5f;
					break;
				case Direction.EAST:
					targetGridPoint.x = Mathf.Ceil(transform.position.x / 0.5f) * 0.5f;
					break;
				case Direction.WEST:
					targetGridPoint.x = Mathf.Floor(transform.position.x / 0.5f) * 0.5f;
					break;
				default:
					break;
			}

			//print(targetGridPoint);

			isTargetSet = true;
		}

		// Fix & reset error
		if (isTargetSet)
		{
			transform.position = CheckPosition(transform.position, targetGridPoint);
		}
	}


	void OnCollisionEnter(Collision collision)
	{
		targetGridPoint = transform.position;
		switch (playerPawn.currentDirection)
		{
			case Direction.NORTH:
				if (collision.transform.position.y > transform.position.y && Mathf.Abs(collision.transform.position.x - transform.position.x) < 1.0f)
				{
					// zero velocity
					GetComponent<Rigidbody>().velocity = Vector3.zero;
					targetGridPoint.y = Mathf.Floor(transform.position.y / 0.5f) * 0.5f;
					isTargetSet = false;
				}
				break;
			case Direction.SOUTH:
				if (collision.transform.position.y < transform.position.y && Mathf.Abs(collision.transform.position.x - transform.position.x) < 1.0f)
				{
					// zero velocity
					GetComponent<Rigidbody>().velocity = Vector3.zero;
					targetGridPoint.y = Mathf.Ceil(transform.position.y / 0.5f) * 0.5f;
					isTargetSet = false;
				}
				break;
			case Direction.EAST:
				if (collision.transform.position.x > transform.position.x && Mathf.Abs(collision.transform.position.y - transform.position.y) < 1.0f)
				{
					// zero velocity
					GetComponent<Rigidbody>().velocity = Vector3.zero;
					targetGridPoint.x = Mathf.Floor(transform.position.x / 0.5f) * 0.5f;
					isTargetSet = false;
				}
				break;
			case Direction.WEST:
				if (collision.transform.position.x < transform.position.x && Mathf.Abs(collision.transform.position.y - transform.position.y) < 1.0f)
				{
					// zero velocity
					GetComponent<Rigidbody>().velocity = Vector3.zero;
					targetGridPoint.x = Mathf.Ceil(transform.position.x / 0.5f) * 0.5f;
					isTargetSet = false;
				}
				break;
			default:
				break;
		}
		transform.position = targetGridPoint;
	}

	// return updated position
	Vector3 CheckPosition(Vector3 start, Vector3 target)
	{
		Vector3 diff = target - start;
		if(Vector3.Magnitude(diff) < positionErrorFixRate)
		{
			GetComponent<Rigidbody>().velocity = Vector3.zero;
			// set complete flag
			//isAdjusted = true;
			//isErrorSet = false;
			isTargetSet = false;
			return target;
		}
		else
		{
			return start; // + positionErrorFixRate * diff.normalized;
		}
	}
}