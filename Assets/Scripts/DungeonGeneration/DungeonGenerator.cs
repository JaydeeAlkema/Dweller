using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// The Dungeon Generator class is responsible for generatin the entire level a.k.a. Dungeon.
/// This dungeon involves rooms and pathways to those rooms. Each room can contain: Enemies, Treasures, NPC and more.
/// </summary>
public class DungeonGenerator : MonoBehaviour
{
	#region Private Variables
	[SerializeField] private bool getPlayerPrefs = false;               // Determines if the playerprefs should be read when within the editor.
	[SerializeField] private string seed = "";
	[Space]
	[SerializeField] private DungeonTheme theme = DungeonTheme.Demonic; // The theme of the dungeon. Since each dungeon is an entire level, we can define the theme high up.
	[SerializeField] private Transform enemyParentTransform = default;  // The parent transform of the enemies.
	[SerializeField] private List<EnemyList> enemyLists = new List<EnemyList>();    // List with Enemy Lists. Within these lists are the enemies that can be spawned per theme.
	[SerializeField] private int spawnChance = 1;                       // How much percentage chance there is to spawn an enemy.
	[Space]
	[SerializeField] private Transform roomParentTransform = default;   // Parent of the rooms in the scene.
	[SerializeField] private Vector2Int minRoomSize = Vector2Int.zero;  // Min Room size.
	[SerializeField] private Vector2Int maxRoomSize = Vector2Int.zero;  // Max Room size.
	[Space]
	[SerializeField] private Transform pathwayParentTransform = default;    // Parent of the pathways in the scene.
	[SerializeField] private int minPathwayCount = 15;                  // Minimum amount of pathways that will be generated.
	[SerializeField] private int maxPathwayCount = 30;                  // Maximum amount of pathways that will be generated.
	[SerializeField] private int minPathwayLength = 10;                 // Minimum length of the pathway before making a turn.
	[SerializeField] private int maxPathwayLength = 20;                 // Maximum length of the pathway before making a turn.

	[Header("Wall Sprites")]
	[SerializeField] private Sprite[] tileGroundSprites = null;            // Tile Ground Sprite.
	[SerializeField] private Sprite tileWallLeftSprite = null;          // Tile Wall Left Sprite.
	[SerializeField] private Sprite tileWallTopSprite = null;           // Tile Wall Top Sprite.

	[Header("Corner Sprites")]
	[SerializeField] private Sprite tileOuterCornerLeftSprite = null;        // Tile Outer Corner Left Sprite
	[SerializeField] private Sprite tileInnerCornerRightSprite = null;       // Tile Inner Corner Right Sprite

	[SerializeField] private List<Room> Rooms = new List<Room>();       // List with all the rooms in the dungeon.
	[SerializeField] private List<Tile> tiles = new List<Tile>();       // List with all the tiles in the dungeon.

	private DateTime startTime; // At which time we started generating the dungeon.

	private int roomIndex = 0;      // Index of the room (Used for giving the rooms their unique ID in their names).
	private int pathwayIndex = 0;   // Index of the Pathway (Used for giving the Pathways their unique ID in their names).
	#endregion

	#region Public Properties
	public string Seed { get => seed; set => seed = value; }
	#endregion

	#region Monobehaviour Callbacks
	private void Awake()
	{
		// Only get the player prefs when not inside the editor or if the boolean is set to true
		if(!Application.isEditor || getPlayerPrefs)
		{
			seed = PlayerPrefs.GetString("Seed");

			minPathwayCount = PlayerPrefs.GetInt("MinPathwayCount");
			maxPathwayCount = PlayerPrefs.GetInt("MaxPathwayCount");

			minPathwayLength = PlayerPrefs.GetInt("MinPathwayLength");
			maxPathwayLength = PlayerPrefs.GetInt("MaxPathwayLength");

			minRoomSize = new Vector2Int(PlayerPrefs.GetInt("MinRoomSize"), PlayerPrefs.GetInt("MinRoomSize"));
			maxRoomSize = new Vector2Int(PlayerPrefs.GetInt("MaxRoomSize"), PlayerPrefs.GetInt("MaxRoomSize"));
		}

		if(seed == "")
			seed = Random.Range(0, int.MaxValue).ToString();

		Random.InitState(seed.GetHashCode());
	}
	private void Start()
	{
		startTime = DateTime.Now;
		Debug.Log("Generating Dungeon");

		GenerateDungeon();

		Debug.Log("Dungeon Generation Took: " + (DateTime.Now - startTime).Milliseconds + "ms");

		// Spawn the player when the dungeon is done generating.
		Vector2 playerSpawnPos = Rooms[0].transform.position;
		GameManager.Instance.SpawnPlayer(playerSpawnPos);
	}
	#endregion

	#region Dungeon Generation
	/// <summary>
	/// The Generate Dungeon Function in theory is just a function that calls other functions.
	/// Might sound a bit backwards, but this makes a nice readeable script layout.
	/// </summary>
	public void GenerateDungeon()
	{
		GeneratePathwaysWithRooms();

		// Highly Inefficient... But it works!
		PlaceWalls();

		SpawnEnemiesInsideTheRooms();
	}


	/// <summary>
	/// This function generates a path in a random direction. After the path is generated, a room will be generated at the end of the path.
	/// This goes on until we reach the amount of pathways chosen.
	/// </summary>
	private void GeneratePathwaysWithRooms()
	{
		// pick a random amount of pathways that need to be generated.
		int pathwayAmount = Random.Range(minPathwayCount, maxPathwayCount);
		// Store the previous pathway direction so we dont get overlapping pathways.
		int previousPathwayDir = 1;
		Vector2Int coordinates = new Vector2Int();
		Vector2Int coordinatesDir = new Vector2Int();
		for(int i = 0; i < pathwayAmount; i++)
		{
			// Decide which way the dungeon should start going in.
			// 1: Left, 2: Up, 3: Right, 4: Down.
			// Keep generating a direction as long as it's the same as the previous one.
			int pathwayStartingDirection = Random.Range(1, 5);
			while(pathwayStartingDirection == 1 && previousPathwayDir == 3 || pathwayStartingDirection == 3 && previousPathwayDir == 1 ||
				  pathwayStartingDirection == 2 && previousPathwayDir == 4 || pathwayStartingDirection == 4 && previousPathwayDir == 2)
			{
				pathwayStartingDirection = Random.Range(1, 5);
			}

			//Debug.Log(previousPathwayDir + " " + pathwayStartingDirection);

			// Set coordinates direction.
			switch(pathwayStartingDirection)
			{
				case 1:
					coordinatesDir = new Vector2Int(-1, 0);
					break;

				case 2:
					coordinatesDir = new Vector2Int(0, 1);
					break;

				case 3:
					coordinatesDir = new Vector2Int(1, 0);
					break;

				case 4:
					coordinatesDir = new Vector2Int(0, -1);
					break;

				default:
					break;
			}

			// Store current direction into the previous direction variable.
			previousPathwayDir = pathwayStartingDirection;

			// Decide how long the pathway should be before generating a new one.
			int pathwayLength = Random.Range(minPathwayLength, maxPathwayLength);

			GameObject pathwayParent = new GameObject
			{
				name = "Pathway [" + pathwayIndex + "]",
			};
			pathwayParent.transform.parent = pathwayParentTransform;

			// Generate the path for the generated length
			// Make the path 5 wide. We dont have to worry about duplicate tiles because those won't get generated anyway.
			for(int j = 0; j < pathwayLength; j++)
			{
				GenerateGroundTile("Pathway [" + pathwayIndex + "]", new Vector2Int(coordinates.x + (coordinatesDir.x * j), coordinates.y + (coordinatesDir.y * j)), pathwayParent.transform);
				GenerateGroundTile("Pathway [" + pathwayIndex + "]", new Vector2Int(coordinates.x + (coordinatesDir.x * j - 1), coordinates.y + (coordinatesDir.y * j - 1)), pathwayParent.transform);
				GenerateGroundTile("Pathway [" + pathwayIndex + "]", new Vector2Int(coordinates.x + (coordinatesDir.x * j - 2), coordinates.y + (coordinatesDir.y * j - 2)), pathwayParent.transform);
				GenerateGroundTile("Pathway [" + pathwayIndex + "]", new Vector2Int(coordinates.x + (coordinatesDir.x * j + 1), coordinates.y + (coordinatesDir.y * j + 1)), pathwayParent.transform);
				GenerateGroundTile("Pathway [" + pathwayIndex + "]", new Vector2Int(coordinates.x + (coordinatesDir.x * j + 2), coordinates.y + (coordinatesDir.y * j + 2)), pathwayParent.transform);
			}

			// Create a room at the end of each pathway.
			GenerateRoom(coordinates);
			coordinates.x += coordinatesDir.x * pathwayLength;
			coordinates.y += coordinatesDir.y * pathwayLength;

			pathwayIndex++;
		}
		// Generate one final room for the final pathway to fix the dead end.
		GenerateRoom(coordinates);
	}

	/// <summary>
	/// Generates a room at the givin coordinates with the givin min/max roomsize.
	/// </summary>
	/// <param name="coordinates"> Room Starting Coordinates. </param>
	private void GenerateRoom(Vector2Int coordinates)
	{
		roomIndex++;
		GameObject newRoomGO = new GameObject { name = "Room [" + roomIndex + "]" };

		newRoomGO.transform.parent = roomParentTransform;
		newRoomGO.transform.position = new Vector3(coordinates.x, coordinates.y, 0);
		newRoomGO.AddComponent<Room>();

		Room room = newRoomGO.GetComponent<Room>();

		int roomSizeX = Random.Range(minRoomSize.x, maxRoomSize.x);
		int roomSizeY = Random.Range(minRoomSize.y, maxRoomSize.y);

		// Force the width and height to be an Odd number
		if(roomSizeX % 2 == 0)
			roomSizeX -= 1;
		if(roomSizeY % 2 == 0)
			roomSizeY -= 1;

		room.RoomSize = new Vector2Int(roomSizeX, roomSizeY);
		Rooms.Add(room);

		for(int x = 0; x < roomSizeX; x++)
		{
			for(int y = 0; y < roomSizeY; y++)
			{
				GenerateGroundTile("Tile [" + (coordinates.x + x) + "]" + " " + "[" + (coordinates.y + y) + "]", new Vector2Int(coordinates.x - (roomSizeX / 2) + x, coordinates.y - (roomSizeY / 2) + y), newRoomGO.transform);
			}
		}
	}

	/// <summary>
	/// Generates a Ground Tile with the given name, coordinates and parrentroom (if given)
	/// </summary>
	/// <param name="tileName"> The name of the tile. </param>
	/// <param name="coordinates"> The coordinates of the tile. </param>
	/// <param name="parentTransform"> The parentTransform of the tile. (This is not necessary!). </param>
	private void GenerateGroundTile(string tileName, Vector2Int coordinates, Transform parentTransform)
	{
		// This removes a possible duplicate tile with the same coordinates.
		// We could check for duplicate tile and return the function, but this makes the hierarchy cleaner.
		for(int t = 0; t < tiles.Count; t++)
			if(tiles[t].Coordinates == coordinates)
			{
				GameObject tileOBJ = tiles[t].gameObject;
				tiles.RemoveAt(t);
				Destroy(tileOBJ);
			}

		GameObject newTileGO = new GameObject { name = tileName };

		newTileGO.AddComponent<Tile>();
		newTileGO.AddComponent<SpriteRenderer>();

		Tile tile = newTileGO.GetComponent<Tile>();
		SpriteRenderer spriteRenderer = newTileGO.GetComponent<SpriteRenderer>();

		tile.Coordinates = new Vector2Int(coordinates.x, coordinates.y);
		tile.Sprite = tileGroundSprites[Random.Range(0, tileGroundSprites.Length)];
		spriteRenderer.sprite = tile.Sprite;

		newTileGO.transform.position = (Vector2)tile.Coordinates;

		if(parentTransform)
		{
			parentTransform.GetComponent<Room>()?.Tiles.Add(tile);
			newTileGO.transform.parent = parentTransform.transform;
		}
		tiles.Add(tile);
	}

	/// <summary>
	/// The placewalls functions will loop through all the tiles in the dungeoon, look at their coordinates and the amount of neighbouring tiles.
	/// Depending on which neighbouring tiles is missing, it places a wall.
	/// </summary>
	private void PlaceWalls()
	{
		for(int t = 0; t < tiles.Count; t++)
		{
			List<Tile> neighbourTiles = new List<Tile>();
			Tile tile = tiles[t];

			// Left, Right, Top and Bottom local Tiles.
			Tile leftTile = null;
			Tile rightTile = null;
			Tile topTile = null;
			Tile bottomTile = null;

			// Top Left, Top Right, Bottom Left and Bottom Right Tiles.
			Tile topLeftTile = null;
			Tile topRightTile = null;
			Tile bottomLeftTile = null;
			Tile bottomRightTile = null;

			#region Get neighbouring tiles
			// Get all the neighbour tiles.
			for(int i = 0; i < tiles.Count; i++)
			{
				// Get Left tile
				if(tiles[i].Coordinates == new Vector2Int(tile.Coordinates.x - 1, tile.Coordinates.y))
				{
					leftTile = tiles[i];
					neighbourTiles.Add(leftTile);
				}

				// Get Right tile
				else if(tiles[i].Coordinates == new Vector2Int(tile.Coordinates.x + 1, tile.Coordinates.y))
				{
					rightTile = tiles[i];
					neighbourTiles.Add(rightTile);
				}

				// Get Up tile
				else if(tiles[i].Coordinates == new Vector2Int(tile.Coordinates.x, tile.Coordinates.y + 1))
				{
					topTile = tiles[i];
					neighbourTiles.Add(topTile);
				}

				// Get Down tile
				else if(tiles[i].Coordinates == new Vector2Int(tile.Coordinates.x, tile.Coordinates.y - 1))
				{
					bottomTile = tiles[i];
					neighbourTiles.Add(bottomTile);
				}

				// Get Top Left Tile
				else if(tiles[i].Coordinates == new Vector2Int(tile.Coordinates.x - 1, tile.Coordinates.y + 1))
				{
					topLeftTile = tiles[i];
					neighbourTiles.Add(topLeftTile);
				}

				// Get Top Right Tile
				else if(tiles[i].Coordinates == new Vector2Int(tile.Coordinates.x + 1, tile.Coordinates.y + 1))
				{
					topRightTile = tiles[i];
					neighbourTiles.Add(topRightTile);
				}

				// Get Bottom Left
				else if(tiles[i].Coordinates == new Vector2Int(tile.Coordinates.x - 1, tile.Coordinates.y - 1))
				{
					bottomLeftTile = tiles[i];
					neighbourTiles.Add(bottomLeftTile);
				}

				// Get Bottom Right
				else if(tiles[i].Coordinates == new Vector2Int(tile.Coordinates.x + 1, tile.Coordinates.y - 1))
				{
					bottomRightTile = tiles[i];
					neighbourTiles.Add(bottomRightTile);
				}
			}

			if(neighbourTiles.Count < 8)
			{
				tile.Type = Tile.TileType.Wall;
			}
			#endregion

			#region Left, Right, Top and Bottom checks
			// Check if this tile is all the way in the left of a room. a.k.a. no Left neighbour.
			if(leftTile == null && rightTile != null && topTile != null && bottomTile != null)
			{
				tile.GetComponent<SpriteRenderer>().sprite = tileWallLeftSprite;
				tile.gameObject.AddComponent<BoxCollider2D>();
			}

			// Check if this tile is all the way in the Right of a room. a.k.a. no Right neighbour.
			else if(leftTile != null && rightTile == null && topTile != null && bottomTile != null)
			{
				tile.GetComponent<SpriteRenderer>().sprite = tileWallLeftSprite;
				tile.GetComponent<SpriteRenderer>().flipX = true;
				tile.gameObject.AddComponent<BoxCollider2D>();
			}

			// Check if this tile is all the way in the Top of a room. a.k.a. no top neighbour.
			else if(leftTile != null && rightTile != null && topTile == null && bottomTile != null)
			{
				tile.GetComponent<SpriteRenderer>().sprite = tileWallTopSprite;
				tile.gameObject.AddComponent<BoxCollider2D>();
			}

			// Check if this tile is all the way in the Bottom of a room. a.k.a. no bottom neighbour.
			else if(leftTile != null && rightTile != null && topTile != null && bottomTile == null)
			{
				tile.GetComponent<SpriteRenderer>().sprite = tileWallTopSprite;
				tile.GetComponent<SpriteRenderer>().flipY = true;
				tile.gameObject.AddComponent<BoxCollider2D>();
			}
			#endregion

			#region Outer Corner Checks
			// Top Left Outer Corner.
			else if(leftTile == null && rightTile != null && topTile == null && bottomTile != null)
			{
				tile.GetComponent<SpriteRenderer>().sprite = tileOuterCornerLeftSprite;
				tile.gameObject.AddComponent<BoxCollider2D>();
			}

			// Top Right Outer Corner.
			else if(leftTile != null && rightTile == null && topTile == null && bottomTile != null)
			{
				tile.GetComponent<SpriteRenderer>().sprite = tileOuterCornerLeftSprite;
				tile.GetComponent<SpriteRenderer>().flipX = true;
				tile.gameObject.AddComponent<BoxCollider2D>();
			}

			// Bottom Left Outer Corner.
			else if(leftTile == null && rightTile != null && topTile != null && bottomTile == null)
			{
				tile.GetComponent<SpriteRenderer>().sprite = tileOuterCornerLeftSprite;
				tile.GetComponent<SpriteRenderer>().flipY = true;
				tile.gameObject.AddComponent<BoxCollider2D>();
			}

			// Bottom Right Outer Corner.
			else if(leftTile != null && rightTile == null && topTile != null && bottomTile == null)
			{
				tile.GetComponent<SpriteRenderer>().sprite = tileOuterCornerLeftSprite;
				tile.GetComponent<SpriteRenderer>().flipY = true;
				tile.GetComponent<SpriteRenderer>().flipX = true;
				tile.gameObject.AddComponent<BoxCollider2D>();
			}
			#endregion

			#region Inner Corner Checks
			// Top Left Inner Corner
			// We use a right sprite because the tile might be topleft. But the Sprite is top right because it's an inside corner.
			else if(topLeftTile == null)
			{
				tile.GetComponent<SpriteRenderer>().sprite = tileInnerCornerRightSprite;
				tile.gameObject.AddComponent<BoxCollider2D>();
			}
			else if(topRightTile == null)
			{
				tile.GetComponent<SpriteRenderer>().sprite = tileInnerCornerRightSprite;
				tile.GetComponent<SpriteRenderer>().flipX = true;
				tile.gameObject.AddComponent<BoxCollider2D>();
			}
			else if(bottomLeftTile == null)
			{
				tile.GetComponent<SpriteRenderer>().sprite = tileInnerCornerRightSprite;
				tile.GetComponent<SpriteRenderer>().flipY = true;
				tile.gameObject.AddComponent<BoxCollider2D>();
			}
			else if(bottomRightTile == null)
			{
				tile.GetComponent<SpriteRenderer>().sprite = tileInnerCornerRightSprite;
				tile.GetComponent<SpriteRenderer>().flipY = true;
				tile.GetComponent<SpriteRenderer>().flipX = true;
				tile.gameObject.AddComponent<BoxCollider2D>();
			}
			#endregion
		}
	}
	#endregion

	#region Enemy Spawning
	/// <summary>
	/// Spawns enemies inside the rooms.
	/// </summary>
	private void SpawnEnemiesInsideTheRooms()
	{
		// Ignore the first room because that's where the player spawns.
		for(int r = 1; r < Rooms.Count; r++)
		{
			if(Rooms[r].Tiles.Count == 0)
				return;

			for(int t = 0; t < Rooms[r].Tiles.Count; t++)
			{
				Tile tile = Rooms[r].Tiles[t];

				if(tile.Type == Tile.TileType.Ground)
					SpawnEnemy(tile.Coordinates, Rooms[r].transform);
			}
		}
	}

	/// <summary>
	/// Spawns an enemy at the given coordinates
	/// </summary>
	/// <param name="coordinates"> Which coordinate to spawn the enemy on. </param>
	private void SpawnEnemy(Vector2 coordinates, Transform parent = null)
	{
		int spawnPercentage = Random.Range(0, 100);
		if(spawnPercentage <= spawnChance)
		{
			int randEnemyIndex = Random.Range(0, enemyLists[0].Enemies.Count);
			GameObject newEnemyGO = Instantiate(enemyLists[0].Enemies[randEnemyIndex], coordinates, Quaternion.identity, parent);
		}
	}
	#endregion
}

[System.Serializable]
public class Tile : MonoBehaviour
{
	#region Private Variables
	public enum TileType
	{
		Ground,
		Wall
	}

	[SerializeField] private TileType type = TileType.Ground;
	[SerializeField] private Vector2Int coordinates = Vector2Int.zero;
	[SerializeField] private Sprite sprite = null;
	#endregion

	#region Public Properties
	public TileType Type { get => type; set => type = value; }
	public Vector2Int Coordinates { get => coordinates; set => coordinates = value; }
	public Sprite Sprite { get => sprite; set => sprite = value; }
	#endregion
}

[System.Serializable]
public class Room : MonoBehaviour
{
	#region Private Variables
	[SerializeField] private Vector2Int roomSize = new Vector2Int();
	[SerializeField] private List<Tile> tiles = new List<Tile>();
	#endregion

	#region Public Properties
	public List<Tile> Tiles { get => tiles; set => tiles = value; }
	public Vector2Int RoomSize { get => roomSize; set => roomSize = value; }
	#endregion
}

[System.Serializable]
public struct EnemyList
{
	[SerializeField] private new string name;
	[SerializeField] private List<GameObject> enemies;

	public List<GameObject> Enemies { get => enemies; set => enemies = value; }
}

/// <summary>
/// Each dungeon can have one of these themes.
/// Each theme dictates which kind of enemies can spawn within the dungeon.
/// The looks of the dungeon sprites dont change with the theme. (Because I lack the art :) )
/// </summary>
public enum DungeonTheme
{
	Demonic,        // Demonic Enemies. Small demonic imp types, big demonic tanks, etc.
	Orcish,         // Orcish Enemies: Big orc warriors, smaller annoying orcs, etc.
	Necrotic,       // Necrotic Enemies: Zombies, Skeletons, etc.
	Fantasy         // Fantasy Enemies: Slimes, Mages, Evil knights, etc.
}
