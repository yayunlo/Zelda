using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Map : MonoBehaviour
{
	// Map data structure
	static public int mapTileWidth, mapTileHeight;
	static private int[,] tile_indice;
	static private Tile[,] map;
	static private List<Tile> tile_pool; /* Memory Management */

	// Screen Size Info
	static public Vector2 screenTileSize = new Vector2(16, 15);
	static public int tileOverage = 2;

	// Map game-play data and file pointers
	static public TextAsset mapTileIndiceData;
	static public TextAsset collisionData;
	static public TextAsset destructibleData;
	static public TextAsset tabData;
	static private string collisionScript;
	static private string destructibleScript;
	static private string tagScript;

	// On screen rendering info
	static public Transform mapAnchor;
	static public GameObject tilePrefab;

	// public Vector2 texture_scale;

	void Start()
	{
		ParseFiles();

		// Generate the mapAnchor to which all of the Tiles will be parented
		mapAnchor = (new GameObject("MapAnchor")).transform;
		
		//Init other data structure for map
		tile_pool = new List<Tile>();
		map = new Tile[mapTileWidth, mapTileHeight]; // Should fill with nulls - JB

		RedrawScreen(true);
	}


	void FixedUpdate()
	{
		RedrawScreen();
	}


	public void RedrawScreen(bool clearAll = false)
	{
		if (clearAll)
		{
			// Clear every Tile that was on screen
			for (int i = 0; i < mapTileWidth; i++)
			{
				for (int j = 0; j < mapTileHeight; j++)
				{
					if (map[i, j] != null)
					{
						HideAndDiscardTile(map[i, j]); // Move this tile back on to the stack
					}
				}
			}
		}

		int cam_x = Mathf.RoundToInt(Camera.main.transform.position.x);
		int cam_y = Mathf.RoundToInt(Camera.main.transform.position.y);

		int left = cam_x - (int)screenTileSize.x / 2 - tileOverage;
		int right = cam_x + (int)screenTileSize.x / 2 + tileOverage;
		int buttom = cam_y - (int)screenTileSize.y / 2 - tileOverage;
		int top = cam_y + (int)screenTileSize.y / 2 + tileOverage;

		Tile t;
		for (int i = left - tileOverage; i < right + tileOverage; i++)
		{
			for (int j = buttom - tileOverage; j < top + tileOverage; j++)
			{
				// Don't go out of bounds
				if (i < 0 || j < 0 || i >= mapTileWidth || j >= mapTileHeight)
				{
				}
				// Off-screen Tile || On-screen Null Tiles
				else if (i < left || i > right || j < buttom || j > top || tile_indice[i, j] == 0)
				{
					if (map[i, j] != null)
					{
						HideAndDiscardTile(map[i, j]);
					}
				}
				else if (map[i, j] == null)
				{
					PickUpAndRecreateTile(i, j, tile_indice[i, j], collisionScript[i, j]);
				}
			}
		}
	}


	public void ParseFiles()
	{
		// Remove the line endings from the text of the collision and destructible data
		collisionScript = RemoveLineEndings(collisionData.text);
		destructibleScript = RemoveLineEndings(destructibleData.text);
		tagScript = RemoveLineEndings(tabData.text);

		// Read in the map data
		string[] lines = mapTileIndiceData.text.Split('\n');
		mapTileHeight = lines.Length;
		string[] tileNums = lines[0].Split(' ');
		mapTileWidth = tileNums.Length;

		// Place the map data into a 2D Array to make it faster to access
		tile_indice = new int[mapTileWidth, mapTileHeight];
		for (int j = 0; j < mapTileHeight; j++)
		{
			tileNums = lines[j].Split(' '); // Yes, this is slightly inefficient because it repeats a prev line for j=0. Does that actually matter? - JB
			for (int i = 0; i < mapTileWidth; i++)
			{
				tile_indice[i, j] = int.Parse(tileNums[i]);
			}
		}
	}


	public Tile PickUpAndRecreateTile(int x, int y, int tileIndice, char coll_type, char tag)
	{
		Tile t;
		// If the pool is empty, create a new Tile
		if (tile_pool.Count == 0)
		{
			GameObject newPoolTileObject = Instantiate<GameObject>(tilePrefab);
			newPoolTileObject.transform.SetParent(mapAnchor, true);
			newPoolTileObject.SetActive(false);
			t = newPoolTileObject.GetComponent<Tile>();
		}
		else
		{
			t = tile_pool[tile_pool.Count - 1];
			tile_pool.RemoveAt(tile_pool.Count - 1);
		}

		t.InitTile(x, y, tileIndice, coll_type, tag);

		gameObject.SetActive(true);

		return t;
	}

	public void HideAndDiscardTile(Tile t)
	{
		map[(int)t.transform.localPosition.x, (int)t.transform.localPosition.y] = null;
		t.gameObject.SetActive(false);
		tile_pool.Add(t);
	}


	static public string ReorderLinesOfDataFiles(string sIn)
	{
		string sOut;
		sIn = sIn.Trim();
		string[] lines = sIn.Split('\n');
		sOut = "";
		for (int i = lines.Length - 1; i >= 0; i--)
		{
			sOut += lines[i];
		}
		return sOut;
	}

	public static string RemoveLineEndings(string sIn)
	{
		if (System.String.IsNullOrEmpty(sIn))
		{
			return sIn;
		}
		string lineSeparator = ((char)0x2028).ToString();
		string paragraphSeparator = ((char)0x2029).ToString();

		return sIn.Replace("\r\n", string.Empty).Replace("\n", string.Empty).Replace("\r", string.Empty).Replace("\f", string.Empty).Replace(lineSeparator, string.Empty).Replace(paragraphSeparator, string.Empty);
	}



}

