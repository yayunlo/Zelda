using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour {
    static Sprite[]         spriteArray;
    public Texture2D        spriteTexture;
	public int				tileIndice;
	public BoxCollider		bc;
    public Material			mat;
    public SpriteRenderer	sprend;

	void Awake() {
        if (spriteArray == null) {
            spriteArray = Resources.LoadAll<Sprite>(spriteTexture.name);
        }

		bc = GetComponent<BoxCollider>();

        sprend = GetComponent<SpriteRenderer>();
        //Renderer rend = gameObject.GetComponent<Renderer>();
        //mat = rend.material;
	}


	public void InitTile(int _x, int _y, int _tileIndice, char coll_type, char tag_type)
	{
		transform.localPosition = new Vector3(_x, _y, 0);
		gameObject.name = _x.ToString("D3") + "x" + _y.ToString("D3");
		sprend.sprite = spriteArray[_tileIndice];
		SetCollider(coll_type);
		SetTag(tag_type);
		//TODO: Add something for destructibility - JB

	}

	// Arrange the collider for this tile
	void SetCollider(char coll_type) {
        
        // Collider info from collisionData
        bc.enabled = true;
        switch (coll_type) {
        case 'S': // Whole
            bc.center = Vector3.zero;
            bc.size = Vector3.one;
            break;
        case 'Q': // Top, Left
            bc.center = new Vector3( -0.25f, 0.25f, 0 );
            bc.size =   new Vector3( 0.5f, 0.5f, 1 );
            break;
        case 'W': // Top
            bc.center = new Vector3( 0, 0.25f, 0 );
            bc.size =   new Vector3( 1, 0.5f, 1 );
            break;
        case 'E': // Top, Right
            bc.center = new Vector3( 0.25f, 0.25f, 0 );
            bc.size =   new Vector3( 0.5f, 0.5f, 1 );
            break;
        case 'A': // Left
            bc.center = new Vector3( -0.25f, 0, 0 );
            bc.size =   new Vector3( 0.5f, 1, 1 );
            break;
        case 'D': // Right
            bc.center = new Vector3( 0.25f, 0, 0 );
            bc.size =   new Vector3( 0.5f, 1, 1 );
            break;
        case 'Z': // Bottom, left
            bc.center = new Vector3( -0.25f, -0.25f, 0 );
            bc.size =   new Vector3( 0.5f, 0.5f, 1 );
            break;
        case 'X': // Bottom
            bc.center = new Vector3( 0, -0.25f, 0 );
            bc.size =   new Vector3( 1, 0.5f, 1 );
            break;
        case 'C': // Bottom, Right
            bc.center = new Vector3( 0.25f, -0.25f, 0 );
            bc.size =   new Vector3( 0.5f, 0.5f, 1 );
            break;
            
        default: // Anything else: _, |, etc.
            bc.enabled = false;
            break;
        }

	}

	void SetTag (char tag_type) {

		SpriteRenderer sprite = GetComponent<SpriteRenderer> ();

		switch (tag_type) {
		case 'D': // Door
			tag = "Door";
			sprite.sortingOrder = 2;
			break;

		case 'L': // Locked Door
			tag = "LockedDoor";
			break;

		case 'W':
			tag = "Wall";
			break;

		default: 
			tag = "Untagged";
			sprite.sortingOrder = 0;
			break;
		}

	}

}




/*
for (int j=0; j<h; j++) {
	string[] tiles = lines[j].Split(' ');
	w = tiles.Length;
	for (int i=0; i<w; i++) {
		foreach (Vector2 stopPoint in stopPoints) {
			if (i == stopPoint.x && j == stopPoint.y) {
				print ("Hit a stopPoint: "+i+"x"+j);
			}
		}
		
		// Find out which tile we're using - JB
		tileNum = int.Parse(tiles[i]);
		if (tileNum == 0) continue; // Skip tiles that we don't need. - JB
		
		go = Instantiate<GameObject>(quadPrefab);
		go.name = i.ToString("D3")+"x"+j.ToString("D3");
		rend = go.GetComponent<Renderer>();
		rend.material.mainTexture = mapSprites;
		rend.material.mainTextureScale = texScaleV2;
		
		x = tileNum % spriteSheetW;
		y = tileNum / spriteSheetW;
		
		rend.material.mainTextureOffset = new Vector2( x/ssF, y/ssF );
		
		go.transform.position = new Vector3(i, j, 0);
		
		// Collider info from collisionData
		BoxCollider bc = go.GetComponent<BoxCollider>();
		bc.enabled = true;
		char c = collisionS[tileNum];
		switch (c) {
		case 'S': // Whole
			bc.center = Vector3.zero;
			bc.size = Vector3.one;
			break;
		case 'Q': // Top, Left
			bc.center = new Vector3( -0.25f, 0.25f, 0 );
			bc.size =   new Vector3( 0.5f, 0.5f, 1 );
			break;
		case 'W': // Top
			bc.center = new Vector3( 0, 0.25f, 0 );
			bc.size =   new Vector3( 1, 0.5f, 1 );
			break;
		case 'E': // Top, Right
			bc.center = new Vector3( 0.25f, 0.25f, 0 );
			bc.size =   new Vector3( 0.5f, 0.5f, 1 );
			break;
		case 'A': // Left
			bc.center = new Vector3( -0.25f, 0, 0 );
			bc.size =   new Vector3( 0.5f, 1, 1 );
			break;
		case 'D': // Right
			bc.center = new Vector3( 0.25f, 0, 0 );
			bc.size =   new Vector3( 0.5f, 1, 1 );
			break;
		case 'Z': // Bottom, left
			bc.center = new Vector3( -0.25f, -0.25f, 0 );
			bc.size =   new Vector3( 0.5f, 0.5f, 1 );
			break;
		case 'X': // Bottom
			bc.center = new Vector3( 0, -0.25f, 0 );
			bc.size =   new Vector3( 1, 0.5f, 1 );
			break;
		case 'C': // Bottom, Right
			bc.center = new Vector3( 0.25f, -0.25f, 0 );
			bc.size =   new Vector3( 0.5f, 0.5f, 1 );
			break;
			
		default: // Anything else: _, |, etc.
			bc.enabled = false;
			break;
		}
	}
 */   