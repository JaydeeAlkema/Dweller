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
	[SerializeField] private int roomCount = 0;                         // How many rooms in the maps need to be made. The rooms are generated in a way they can overlap to generate unique looking rooms.
	[SerializeField] private Vector2Int minRoomSize = Vector2Int.zero;  // Min Room size.
	[SerializeField] private Vector2Int maxRoomSize = Vector2Int.zero;  // Max Room size.
	[SerializeField] private Sprite tileGroundSprite = null;            // Tile Ground Sprite.

	[SerializeField] private List<Room> Rooms = new List<Room>();       // List with all the rooms in the dungeon.
	[SerializeField] private List<Tile> tiles = new List<Tile>();       // List with all the tiles in the dungeon.

	private int coordinateOffset = 1;   // The offset between tiles. a.k.a. the tile width and height.
	private DateTime startTime;
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
		GenerateRooms();
		// Generate something else...

		Debug.Log("Dungeon Generation Took: " + (DateTime.Now - startTime).Milliseconds + "ms" + " | " + (DateTime.Now - startTime).Seconds + "sec");
	}

	/// <summary>
	/// The Generate Dungeon Function generates the rooms with the necessary tiles.
	/// </summary>
	private void GenerateRooms()
	{
		for(int i = 0; i < roomCount; i++)
		{
			GameObject newRoomGO = new GameObject { name = "Room [" + i + "]" };
			newRoomGO.AddComponent<Room>();
			Room room = newRoomGO.GetComponent<Room>();

			int roomStartCoordinateX = Random.Range(0, mapSize.x);
			int roomStartCoordinateY = Random.Range(0, mapSize.y);
			int roomSizeX = Random.Range(minRoomSize.x, maxRoomSize.x);
			int roomSizeY = Random.Range(minRoomSize.y, maxRoomSize.y);

			Rooms.Add(room);

			for(int x = 0; x < roomSizeX; x++)
			{
				for(int y = 0; y < roomSizeY; y++)
				{
					int xPos = x + roomStartCoordinateX;
					int yPos = y + roomStartCoordinateY;

					// This prevents a duplicate tile being created.
					for(int t = 0; t < tiles.Count; t++)
						if(tiles[t].Coordinates == new Vector2Int(xPos, yPos))
							return;

					GameObject newTileGO = new GameObject { name = "Tile [" + xPos * coordinateOffset + "]" + " " + "[" + yPos * coordinateOffset + "]" };
					newTileGO.transform.SetParent(newRoomGO.transform);

					newTileGO.AddComponent<Tile>();
					newTileGO.AddComponent<SpriteRenderer>();

					Tile tile = newTileGO.GetComponent<Tile>();
					SpriteRenderer spriteRenderer = newTileGO.GetComponent<SpriteRenderer>();

					tile.Coordinates = new Vector2Int(xPos * coordinateOffset, yPos * coordinateOffset);

					tile.Sprite = tileGroundSprite;
					spriteRenderer.sprite = tile.Sprite;

					newTileGO.transform.position = (Vector2)tile.Coordinates;
					room.Tiles.Add(tile);
					tiles.Add(tile);
				}
			}
		}
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
