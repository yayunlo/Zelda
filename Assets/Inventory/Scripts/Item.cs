﻿using UnityEngine;
using System.Collections;

public enum ItemType {MANA, HEALTH, RUPEE, KEY, BOMB};

public class Item : MonoBehaviour 
{
    /// <summary>
    /// The current item type
    /// </summary>
    public ItemType type;

    public bool isPackable;

    /// <summary>
    /// The item's neutral sprite
    /// </summary>
    public Sprite spriteNeutral;

    /// <summary>
    /// The item's highlighted sprite
    /// </summary>
    public Sprite spriteHighlighted;

    /// <summary>
    /// The max amount of times the item can stack
    /// </summary>
    public int maxSize;

    /// <summary>
    /// Uses the item
    /// </summary>
    public void Use()
    {
        switch (type) //Checks which kind of item this is
        {
            case ItemType.MANA:
                Debug.Log("I just used a mana potion");
                break;
            case ItemType.HEALTH:
                Debug.Log("I just used a health potion");
                break;
            case ItemType.RUPEE:
                Debug.Log("I can't use rupee");
                break;
            case ItemType.KEY:
                Debug.Log("I just used a key");
                break;
            case ItemType.BOMB:
                Debug.Log("I just used a bomb");
                break;
        }

    }

}
