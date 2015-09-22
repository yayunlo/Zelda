using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
    
    /// <summary>
    /// The player's movement speed
    /// </summary>
    public float speed;

    /// <summary>
    /// A reference to the inventory
    /// </summary>
    public Inventory inventory;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
        HandleMovement();
	}

    /// <summary>
    /// Handles the players movement
    /// </summary>
    private void HandleMovement()
    {
        //Calculates the players translation so that we will move framerate independent
        float translation = speed * Time.deltaTime;

        //Moves the player
        transform.Translate(new Vector3(Input.GetAxis("Horizontal") * translation, 0, Input.GetAxis("Vertical") * translation));
    }

    /// <summary>
    /// Handles the player's collision
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Item") //If we collide with an item that we can pick up
        {
            inventory.AddItem(other.GetComponent<Item>()); //Adds the item to the inventory.
        }
    }
}
