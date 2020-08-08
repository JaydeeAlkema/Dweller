using Pathfinding;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Management.Instrumentation;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// The Dungeon Generator class is responsible for generatin the entire level a.k.a. Dungeon.
/// This dungeon involves rooms and pathways to those rooms. Each room can contain: Enemies, Treasures, NPC and more.
/// </summary>
public class DungeonGenerator : MonoBehaviour
{
	#region Private Variables
	[SerializeField] private string seed = "";
	[Space]
	[SerializeField] private DungeonTheme theme = DungeonTheme.Demonic; // The theme of the dungeon. Since each dungeon is an entire level, we can define the theme high up.
	[SerializeField] private Vector2Int mapSize = Vector2Int.zero;      // The size of the maps. How many tiles wide and high.
	[Space]
	[SerializeField] private Vector2Int minRoomSize = Vector2Int.zero;  // Min Room size.
	[SerializeField] private Vector2Int maxRoomSize = Vector2Int.zero;  // Max Room size.
	[Space]
	[SerializeField] private int minPathwayCount = 15;                  // Minimum amount of pathways that will be generated.
	[SerializeField] private int maxPathwayCount = 30;                  // Maximum amount of pathways that will be generated.
	[SerializeField] private int minPathwayLength = 10;                 // Minimum length of the pathway before making a turn.
	[SerializeField] private int maxPathwayLength = 20;                 // Maximum length of the pathway before making a turn.
	[SerializeField] private Sprite tileGroundSprite = null;            // Tile Ground Sprite.

	[SerializeField] private List<Room> Rooms = new List<Room>();       // List with all the rooms in the dungeon.
	[SerializeField] private List<Tile> tiles = new List<Tile>();       // List with all the tiles in the dungeon.

	private int coordinateOffset = 1;   // The offset between tiles. a.k.a. the tile width and height.
	private DateTime startTime;

	private int roomIndex = 0;
	private int pathwayIndex = 0;
	private int tileIndex = 0;
	#endregion

	#region Monobehaviour Callbacks
	private void Start()
	{
		GenerateDungeon();
	}
	#endregion

	#region Dungeon Generation
	/// <summary>
	/// The Generate Dungeon Function in theory is just a function that calls other functions.
	/// Might sound a bit backwards, but this makes a nice readeable script layout.
	/// </summary>
	public void GenerateDungeon()
	{
		Random.InitState(seed.GetHashCode());

		startTime = DateTime.Now;
		Debug.Log("Generating Dungeon");
		GeneratePathways();
		// Generate something else...

		Debug.Log("Dungeon Generation Took: " + (DateTime.Now - startTime).Milliseconds + "ms" + " | " + (DateTime.Now - startTime).Seconds + "sec");
	}

	/// <summary>
	/// Generates a room at the givin coordinates with the givin min/max roomsize.
	/// </summary>
	/// <param name="coordinates"> Room Starting Coordinates. </param>
	private void GenerateRoom(Vector2Int coordinates)
	{
		roomIndex++;
		GameObject newRoomGO = new GameObject { name = "Room [" + roomIndex + "]" };
		newRoomGO.transform.position = new Vector3(coordinates.x, coordinates.y, 0);
		newRoomGO.AddComponent<Room>();
		Room room = newRoomGO.GetComponent<Room>();

		int roomSizeX = Random.Range(minRoomSize.x, maxRoomSize.x);
		int roomSizeY = Random.Range(minRoomSize.y, maxRoomSize.y);

		Rooms.Add(room);

		for(int x = 0; x < roomSizeX; x++)
		{
			for(int y = 0; y < roomSizeY; y++)
			{
				// This prevents a duplicate tile being created.
				//for(int t = 0; t < tiles.Count; t++)
				//	if(tiles[t].Coordinates == new Vector2Int(coordinates.x + x, coordinates.y + y))
				//		return;

				GenerateTile("Tile [" + (coordinates.x + x) + "]" + " " + "[" + (coordinates.y + y) + "]", new Vector2Int(coordinates.x + x, coordinates.y + y), room);
			}
		}
	}

	/// <summary>
	/// This function makes it so each room makes a path to the next room in the list.
	/// For now This is bruteforced, but later on a different technique might get used.
	/// </summary>
	private void GeneratePathways()
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

			Debug.Log(previousPathwayDir + " " + pathwayStartingDirection);

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

			// Generate the path for the generated length
			for(int j = 0; j < pathwayLength; j++)
			{
				GenerateTile("Pathway [" + pathwayIndex + "]", new Vector2Int(coordinates.x + (coordinatesDir.x * j), coordinates.y + (coordinatesDir.y * j)), null);
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
	/// Generates a tile with the given name, coordinates and parrentroom (if given)
	/// </summary>
	/// <param name="tileName"> The name of the tile. </param>
	/// <param name="coordinates"> The coordinates of the tile. </param>
	/// <param name="parentRoom"> The parentRoom of the tile. (This is not necessary!). </param>
	private void GenerateTile(string tileName, Vector2Int coordinates, Room parentRoom)
	{
		GameObject newTileGO = new GameObject { name = tileName };

		newTileGO.AddComponent<Tile>();
		newTileGO.AddComponent<SpriteRenderer>();

		Tile tile = newTileGO.GetComponent<Tile>();
		SpriteRenderer spriteRenderer = newTileGO.GetComponent<SpriteRenderer>();

		tile.Coordinates = new Vector2Int(coordinates.x, coordinates.y);
		tile.Sprite = tileGroundSprite;
		spriteRenderer.sprite = tile.Sprite;

		newTileGO.transform.position = (Vector2)tile.Coordinates;
		if(parentRoom)
		{
			parentRoom.Tiles.Add(tile);
			newTileGO.transform.parent = parentRoom.transform;
		}
		tiles.Add(tile);
	}
	#endregion
}

[System.Serializable]
public class Tile : MonoBehaviour
{
	#region Private Variables
	[SerializeField] private Vector2Int coordinates = Vector2Int.zero;
	[SerializeField] private Sprite sprite = null;
	#endregion

	#region Public Properties
	public Vector2Int Coordinates { get => coordinates; set => coordinates = value; }
	public Sprite Sprite { get => sprite; set => sprite = value; }
	#endregion
}

[System.Serializable]
public class Room : MonoBehaviour
{
	#region Private Variables
	[SerializeField] private List<Tile> tiles = new List<Tile>();
	#endregion

	#region Public Properties
	public List<Tile> Tiles { get => tiles; set => tiles = value; }
	#endregion
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
