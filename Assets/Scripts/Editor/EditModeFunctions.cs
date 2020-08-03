using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class EditModeFunctions : EditorWindow
{
	[MenuItem("Window/Edit Mode Functions")]
	public static void ShowWindow()
	{
		GetWindow<EditModeFunctions>("Edit Mode Functions");
	}

	private void OnGUI()
	{
		if(GUILayout.Button("Initialize Player Stats"))
		{
			PlayerBehaviour player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerBehaviour>();
			player.InitializeStats();
		}
	}
}
