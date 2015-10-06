using UnityEngine;
using System.Collections;

public class PhysicsManager : MonoBehaviour
{

	// Use this for initialization
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}

	public Vector3 correctPlayerInput(Vector3 _playerInput)
	{
		Vector3 correctedVector = _playerInput;
		if (correctedVector.x != 0.0f)
		{
			correctedVector.y = 0.0f;
		}

		return correctedVector;
	}
}
