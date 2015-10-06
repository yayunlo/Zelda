using UnityEngine;
using System.Collections;

public enum AnimationState
{
	NONE, MOVING, ADJUSTING, RECASTING
}

public enum Weapon
{
	BOW, BOOMERANG, BOMB, SWORD, FLYSWORD, WHITESWORD /*interesting*/, FLYWHITESWORD
};

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
	public AnimationState animationState;
	// Weapon state variable
	private Weapon mainWeaponSelection;
	private Weapon offWeaponSelection;
	// Movement info
	private char currentDirection;
	public float velocityFactor; /* Please init this variable in unity editor */
	private bool isDirectionChange;
	
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
		animationState = AnimationState.NONE;
		mainWeaponSelection = Weapon.SWORD;
		offWeaponSelection = Weapon.BOMB;
		currentDirection = 'n';
		isDirectionChange = false;
	}

	void Update()
	{
		switch (animationState)
		{
			case AnimationState.NONE:
				controllerEnabled = true;
				break;
			case AnimationState.MOVING:
				controllerEnabled = true;
				if (isDirectionChange)
				{
					animationState = AnimationState.ADJUSTING;
				}
				break;
			case AnimationState.ADJUSTING:
				controllerEnabled = false;
				if (isAdjusted())
				{
					animationState = AnimationState.NONE;
				}
				break;
			case AnimationState.RECASTING:
				controllerEnabled = false;
				--recastCountDown;
				if (recastCountDown == 0)
				{
					recastCountDown = recastFrames;
					animationState = AnimationState.NONE;
				}
				break;
			default:
				break;
		}
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
	public void setAnimationState(KeyState _keyState)
	{
		switch (animationState)
		{
			case AnimationState.NONE:
				if (_keyState == KeyState.MOVEMENT)
				{
					animationState = AnimationState.MOVING;
				}
				else if (_keyState == KeyState.ATTACK)
				{
					animationState = AnimationState.RECASTING;
				}
				break;
			case AnimationState.MOVING:
				if (_keyState == KeyState.NONE)
				{
					animationState = AnimationState.ADJUSTING;
				}
				break;
			case AnimationState.ADJUSTING:
			case AnimationState.RECASTING:
			default:
				break;
		}
	}

	// detect direction change
	public void move(Vector3 _input)
	{
		if (!controllerEnabled)
		{
			return;
		}

		char oldDirection = currentDirection;
		GetComponent<Rigidbody>().velocity = velocityFactor * _input;

		if (_input.x > 0)	   currentDirection = 'e';
		else if (_input.x < 0) currentDirection = 'w';
		else if (_input.y > 0) currentDirection = 'n';
		else if (_input.y < 0) currentDirection = 's';

		isDirectionChange = oldDirection != currentDirection;
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
		return true;
	}

}
