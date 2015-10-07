using UnityEngine;
using System.Collections;

public enum ActionState
{
	NONE, MOVING, MOVING_WITH_ADJUSTING, ADJUSTING, RECASTING
}

public enum Weapon
{
	BOW, BOOMERANG, BOMB, SWORD, FLYSWORD, WHITESWORD /*interesting*/, FLYWHITESWORD
};

public enum Direction
{
	NORTH, SOUTH, EAST, WEST, STAY
}

public class Link : MonoBehaviour
{
	// Internal data
	public int health;
	public int rupeeCount;
	public int keyCount;
	public int bombCount;
	// Animation state variables
	private int recastCountDown;
	private int skillCooldown;
	private int fixedUpdateCounter;
	private bool controllerEnabled;
	public ActionState actionState;
	// Weapon state variable
	private Weapon mainWeaponSelection;
	private Weapon offWeaponSelection;
	// Movement info
	public Direction currentDirection;
	public float velocityFactor; /* Please init this variable in unity editor */

	private const int fixedUpdatePerSecond = 50;
	// Recasting state will last for 10 frame
	private const int recastFrames = 10;

	// Use this for initialization
	void Start()
	{
		health = 6;
		rupeeCount = 0;
		keyCount = 0;
		bombCount = 0;
		recastCountDown = recastFrames;
		skillCooldown = 0;
		fixedUpdateCounter = 0;
		controllerEnabled = true;
		actionState = ActionState.NONE;
		mainWeaponSelection = Weapon.SWORD;
		offWeaponSelection = Weapon.BOMB;
		currentDirection = Direction.STAY;
	}

	void Update()
	{
		// Update Direction
		Direction oldDirection = currentDirection;

		if (getVelocity().x > 0) currentDirection = Direction.EAST;
		else if (getVelocity().x < 0) currentDirection = Direction.WEST;
		else if (getVelocity().y > 0) currentDirection = Direction.NORTH;
		else if (getVelocity().y < 0) currentDirection = Direction.SOUTH;
		else if (getVelocity().x == 0 && getVelocity().y == 0)
			currentDirection = Direction.STAY;

		bool isDirectionChange = oldDirection != currentDirection;

		switch (actionState)
		{
			case ActionState.NONE:
				controllerEnabled = true;
				//GetComponent<Animator>().speed = 0.00000001f;
				if (GetComponent<Rigidbody>().velocity != Vector3.zero)
				{
					actionState = ActionState.MOVING;
				}
				break;
			case ActionState.MOVING:
				controllerEnabled = true;

				//GetComponent<Animator>().speed = 0.00000001f;

				if (isDirectionChange)
				{
					actionState = ActionState.ADJUSTING;
				}
				break;
			case ActionState.ADJUSTING:
				controllerEnabled = false;
				//print(GetComponent<Rigidbody>().velocity);
				//GetComponent<Rigidbody>().velocity = Vector3.zero;
				//GetComponent<Animator>().speed = 0.00000001f;
				if (isAdjusted())
				{
					actionState = ActionState.NONE;
				}
				break;
			case ActionState.RECASTING:
				controllerEnabled = false;
				//GetComponent<Animator>().speed = 0.00000001f;
				--recastCountDown;
				if (recastCountDown == 0)
				{
					recastCountDown = recastFrames;
					actionState = ActionState.NONE;
				}
				break;
			default:
				break;
		}

		// Set Animator Params
		GetComponent<Animator>().SetFloat("vertical_vel", getVelocity().y);
		GetComponent<Animator>().SetFloat("horizontal_vel", getVelocity().x);

		//print(GetComponent<Animator>().speed);
	}

	// Skill Recharging
	void FixedUpdate()
	{
		if (skillCooldown > 0)
		{
			if (fixedUpdateCounter >= fixedUpdatePerSecond)
			{
				fixedUpdateCounter = 0;
				--skillCooldown;
			}
			else
			{
				++fixedUpdateCounter;
			}
		}
	}

	// Interface for controller
	public void setVelocity(Vector3 _input)
	{
		if (_input.magnitude == 0 && actionState == ActionState.MOVING)
		{
			// we don't want controller to zero the velocity before interpolation completes
			// but we need to start adjusting pos
			actionState = ActionState.ADJUSTING;
			return;
		}

		if (!controllerEnabled)
		{
			return;
		}

		// Set Velocity
		GetComponent<Rigidbody>().velocity = velocityFactor * _input;
	}

	// change state into recasting
	public void attack()
	{
		if (!controllerEnabled)
		{
			return;
		}

		// attack animation
	}

	public void setWeaponSelection(WeaponSwitchMode _weaponSwitchMode)
	{
		switch (_weaponSwitchMode)
		{
			case WeaponSwitchMode.TOGGLE_FLYSWORD:
				if (mainWeaponSelection != Weapon.FLYSWORD)
				{
					mainWeaponSelection = Weapon.FLYSWORD;
				}
				else
				{
					mainWeaponSelection = Weapon.SWORD;
				}
				break;
			case WeaponSwitchMode.TOGGLE_SWORD_BOW:
				if (mainWeaponSelection != Weapon.BOW)
				{
					mainWeaponSelection = Weapon.BOW;
				}
				else
				{
					mainWeaponSelection = Weapon.SWORD;
				}
				break;
			case WeaponSwitchMode.TOGGLE_BOMB_BOOMERANG:
				if (offWeaponSelection != Weapon.BOMB)
				{
					offWeaponSelection = Weapon.BOMB;
				}
				else
				{
					offWeaponSelection = Weapon.BOOMERANG;
				}
				break;
			default:
				break;
		}
	}

	bool isCooldown()
	{
		return skillCooldown > 0;
	}

	bool isAdjusted()
	{
		return transform.position.x % 0.5f == 0.0f && transform.position.y % 0.5f == 0.0f;
	}

	Vector3 getVelocity()
	{
		switch (actionState)
		{
			case ActionState.NONE:
			case ActionState.MOVING:
			case ActionState.ADJUSTING:
				return GetComponent<Rigidbody>().velocity;
			case ActionState.RECASTING:
			default:
				return Vector3.zero;
		}
	}
}
