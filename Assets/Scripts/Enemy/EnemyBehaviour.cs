using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The enemy behaviour class is responsible for the enemy behaviour...
/// It handles target detection aswell as navigation towards it.
/// </summary>
public class EnemyBehaviour : MonoBehaviour
{
	#region Private Variables
	[Header("Movement Stats")]
	[SerializeField] private float movementSpeed = 2.5f;				// The enemy can't run, And the general movement speed is lower than the player.
	[SerializeField] private float movementDestinationInterval = 0.25f;	// How many seconds inbetween target destination is set. Better performance.	
	#endregion
}
