using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DungeonGenerator))]
public class DungeonGeneratorEditor : Editor
{
	// Temp disable. Will return to this in the future.

	//public override void OnInspectorGUI()
	//{
	//	base.OnInspectorGUI();

	//	DungeonGenerator dungeonGenerator = (DungeonGenerator)target;
	//	if(GUILayout.Button("Generate Dungeon"))
	//	{
	//		// Cleanup the rooms in the scene.
	//		dungeonGenerator.GenerateDungeon();
	//	}
	//}
}
