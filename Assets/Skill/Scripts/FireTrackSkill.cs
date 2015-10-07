using UnityEngine;
using System.Collections;

public class FireTrackSkill : MonoBehaviour
{
	public GameObject firePrefab;
	private GameObject fireInstance;
	private bool isSpawning;
	private int fixedUpdateCounter;
	private const int fireCounter = 5;
	private const int fixedUpdatePerSecond = 20;

	// Use this for initialization
	void Start()
	{
		isSpawning = false;
		fixedUpdateCounter = 0;
	}

	// Update is called once per frame
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.R))
		{
			isSpawning = true;
		}

		if (isSpawning)
		{
			Vector3 updatedPosition = transform.position;
			switch (LinkMovement.currentDir)
			{
				case 'n':
					updatedPosition.y += 5 * Time.deltaTime;
					break;
				case 's':
					updatedPosition.y -= 5 * Time.deltaTime;
					break;
				case 'e':
					updatedPosition.x += 5 * Time.deltaTime;
					break;
				case 'w':
					updatedPosition.x -= 5 * Time.deltaTime;
					break;

			}
			transform.position = updatedPosition;
		}
	}

	void FixedUpdate()
	{
		if (isSpawning)
		{
			++fixedUpdateCounter;
			if (fixedUpdateCounter % fixedUpdatePerSecond == 0)
			{
				fireInstance = Instantiate(firePrefab, transform.position, Quaternion.identity) as GameObject;
			}
			if (fixedUpdateCounter >= fireCounter * fixedUpdatePerSecond)
			{
				isSpawning = false;
				fixedUpdateCounter = 0;
			}
		}
	}
}
