using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Inventory : MonoBehaviour
{

    #region Variables
    /// <summary>
    /// The number of rows
    /// </summary>
    public int rows;

    /// <summary>
    /// The number of slots
    /// </summary>
    public int slots;

    /// <summary>
    /// The number of empty slots in the inventory
    /// </summary>
    private static int emptySlots;

    /// <summary>
    /// Offset used to move the hovering object away from the mouse 
    /// </summary>
    private float hoverYOffset;

    /// <summary>
    /// The width and height of the inventory
    /// </summary>
    private float inventoryWidth, inventoryHight;

    /// <summary>
    /// The left and top slots padding
    /// </summary>
    public float slotPaddingLeft, slotPaddingTop;

    /// <summary>
    /// The size of each slot
    /// </summary>
    public float slotSize;

    /// <summary>
    /// The slots prefab
    /// </summary>
    public GameObject slotPrefab;

    /// <summary>
    /// A prefab used for instantiating the hoverObject
    /// </summary>
    public GameObject iconPrefab;

    /// <summary>
    /// A reference to the object that hovers next to the mouse
    /// </summary>
    private static GameObject hoverObject;

    /// <summary>
    /// The slots that we are moving an item from
    /// </summary>
    private static Slot from;

    /// <summary>
    /// The slots that we are moving and item to
    /// </summary>
    private static Slot to;

    /// <summary>
    /// A reference to the inventorys RectTransform
    /// </summary>
    private RectTransform inventoryRect;

    /// <summary>
    /// A reference to the inventorys canvas
    /// </summary>
    public Canvas canvas;

    /// <summary>
    /// A reference to the EventSystem 
    /// </summary>
    public EventSystem eventSystem;

    #endregion

    #region Collections
    /// <summary>
    /// A list of all the slots in the inventory
    /// </summary>
    private List<GameObject> allSlots;
    #endregion

    #region Properties
    /// <summary>
    /// Property for accessing the amount of empty slots
    /// </summary>
    public static int EmptySlots
    {
        get { return emptySlots; }
        set { emptySlots = value; }
    }
    #endregion
    // Use this for initialization
    void Start()
    {   
        //Creates the inventory layout
        CreateLayout();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonUp(0)) //Checks if the user lifted the first mousebutton
        {   
            //Removes the selected item from the inventory
            if (!eventSystem.IsPointerOverGameObject(-1) && from != null) //If we click outside the inventory and the have picked up an item
            {
                from.GetComponent<Image>().color = Color.white; //Rests the slots color 
                from.ClearSlot(); //Removes the item from the slot
                Destroy(GameObject.Find("Hover")); //Removes the hover icon
                
                //Resets the objects
                to = null;
                from = null;
                hoverObject = null;
                emptySlots++;
            }
        }

        if (hoverObject != null) //Checks if the hoverobject exists
        {
            //The hoverObject's position
            Vector2 position; 

            //Translates the mouse screen position into a local position and stores it in the position
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, Input.mousePosition, canvas.worldCamera, out position);

            //Adds the offset to the position
            position.Set(position.x, position.y - hoverYOffset);

            //Sets the hoverObject's position
            hoverObject.transform.position = canvas.transform.TransformPoint(position);
        }
    }

    /// <summary>
    /// Creates the inventory's layout
    /// </summary>
    private void CreateLayout()
    {
        //Instantiates the allSlot's list
        allSlots = new List<GameObject>();

        //Calculates the hoverYOffset by taking 1% of the slot size
        hoverYOffset = slotSize * 0.01f;

        //Stores the number of empty slots
        emptySlots = slots;

        //Calculates the width of the inventory
        inventoryWidth = (slots / rows) * (slotSize + slotPaddingLeft) + slotPaddingLeft;

        //Calculates the highs of the inventory
        inventoryHight = rows * (slotSize + slotPaddingTop) + slotPaddingTop;

        //Creates a reference to the inventory's RectTransform
        inventoryRect = GetComponent<RectTransform>();

        //Sets the with and height of the inventory.
        inventoryRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, inventoryWidth);
        inventoryRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, inventoryHight);

        //Calculates the amount of columns
        int columns = slots / rows;

        for (int y = 0; y < rows; y++) //Runs through the rows
        {
            for (int x = 0; x < columns; x++) //Runs through the columns
            {   
                //Instantiates the slot and creates a reference to it
                GameObject newSlot = (GameObject)Instantiate(slotPrefab);

                //Makes a reference to the rect transform
                RectTransform slotRect = newSlot.GetComponent<RectTransform>();

                //Sets the slots name
                newSlot.name = "Slot";

                //Sets the canvas as the parent of the slots, so that it will be visible on the screen
                newSlot.transform.SetParent(this.transform.parent);

                //Sets the slots position
                slotRect.localPosition = inventoryRect.localPosition + new Vector3(slotPaddingLeft * (x + 1) + (slotSize * x), -slotPaddingTop * (y + 1) - (slotSize * y));

                //Sets the size of the slot
                slotRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, slotSize);
                slotRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, slotSize);

                //Adds the new slots to the slot list
                allSlots.Add(newSlot);

            }
        }
    }

    /// <summary>
    /// Adds an item to the inventory
    /// </summary>
    /// <param name="item">The item to add</param>
    /// <returns></returns>
    public bool AddItem(Item item)
    {
        if (item.maxSize == 1) //If the item isn't stackable
        {   
            //Places the item at an empty slot
            PlaceEmpty(item);
            return true;
        }
        else //If the item is stackable 
        {
            foreach (GameObject slot in allSlots) //Runs through all slots in the inventory
            {
                Slot tmp = slot.GetComponent<Slot>(); //Creates a reference to the slot

                if (!tmp.IsEmpty) //If the item isn't empty
                {
                    //Checks if the om the slot is the same type as the item we want to pick up
                    if (tmp.CurrentItem.type == item.type && tmp.IsAvailable) 
                    {
                        tmp.AddItem(item); //Adds the item to the inventory
                        return true;
                    }
                }
            }
            if (emptySlots > 0) //Places the item on an empty slots
            {
                PlaceEmpty(item);
            }
        }

        return false;
    }

    /// <summary>
    /// Places an item on an empty slot
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    private bool PlaceEmpty(Item item)
    {
        if (emptySlots > 0) //If we have atleast 1 empty slot
        {
            foreach (GameObject slot in allSlots) //Runs through all slots
            {
                Slot tmp = slot.GetComponent<Slot>(); //Creates a reference to the slot 

                if (tmp.IsEmpty) //If the slot is empty
                {
                    tmp.AddItem(item); //Adds the item
                    emptySlots--; //Reduces the number of empty slots
                    return true;
                }
            }
        }

        return false;
    }

    /// <summary>
    /// Moves an item to another slot in the inventory
    /// </summary>
    /// <param name="clicked"></param>
    public void MoveItem(GameObject clicked)
    {
        if (from == null) //If we haven't picked up an item
        {
            if (!clicked.GetComponent<Slot>().IsEmpty) //If the slot we clicked sin't empty
            {
                from = clicked.GetComponent<Slot>(); //The slot we ar emoving from

                from.GetComponent<Image>().color = Color.gray; //Sets the from slots color to gray, to visually indicate that its the slot we are moving from

                hoverObject = (GameObject)Instantiate(iconPrefab); //Instantiates the hover object 

                hoverObject.GetComponent<Image>().sprite = clicked.GetComponent<Image>().sprite; //Sets the sprite on the hover object so that it reflects the object we are moing

                hoverObject.name = "Hover"; //Sets the name of the hover object

                //Creates references to the transforms
                RectTransform hoverTransform = hoverObject.GetComponent<RectTransform>();
                RectTransform clickedTransform = clicked.GetComponent<RectTransform>();

                ///Sets the size of the hoverobject so that it has the same size as the clicked object
                hoverTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, clickedTransform.sizeDelta.x);
                hoverTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, clickedTransform.sizeDelta.y);

                //Sets the hoverobject's parent as the canvas, so that it is visible in the game
                hoverObject.transform.SetParent(GameObject.Find("Canvas").transform, true);

                //Sets the local scale to make usre that it has the correct size
                hoverObject.transform.localScale = from.gameObject.transform.localScale;


            }
        }
        else if (to == null) //Selects the slot we are moving to
        {
            to = clicked.GetComponent<Slot>(); //Sets the to object
            Destroy(GameObject.Find("Hover")); //Destroys the hover object
        }
        if (to != null && from != null) //If both to and from are null then we are done moving. 
        {
            Stack<Item> tmpTo = new Stack<Item>(to.Items); //Stores the items from the to slot, so that we can do a swap
           
            to.AddItems(from.Items); //Stores the items in the "from" slot in the "to" slot

            if (tmpTo.Count == 0) //If "to" slot if 0 then we dont need to move anything to the "from " slot.
            {
                from.ClearSlot(); //clears the from slot
            }
            else
            {
                from.AddItems(tmpTo); //If the "to" slot contains items thne we need to move the to the "from" slot
            }

            //Resets all values
            from.GetComponent<Image>().color = Color.white;
            to = null;
            from = null;
            hoverObject = null;
        }
    }
}
