using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy Stats", menuName = "ScriptableObjects/EnemyStats")]
public class EnemyStats : ScriptableObject
{
	#region Private Variables
	[Header("Enemy Properties")]
	[SerializeField] private new string name = "";                      // Name of the Enemy. Also uses this name on the gameobject.
	[SerializeField] private string description = "";                   // Description of this enemy. Maybe used later for in a journal kind of menu?
	[SerializeField] private int health = 100;                       // Health of this enemy.
	[SerializeField] private int damageOnHit = 10;                   // How much damage the enemy deals when hitting it's target.
	[SerializeField] private LayerMask targetDetectionMask = default;	// Which mask to check for the target.

	[Header("Movement Stats")]
	[SerializeField] private float movementSpeed = 2.5f;                // The enemy can't run, And the general movement speed is lower than the player.
	[SerializeField] private float movementDestinationInterval = 0.25f; // How many seconds inbetween target destination is set. Better performance.	

	[Header("Target Detection Stats")]
	[SerializeField] private float targetDetectionRange = 6.5f;         // Range within the enemy can detect targets.
	[SerializeField] private float targetAttackRange = 2f;              // Range within the enemy can attack the target.
	#endregion

	#region Public Properties
	public string Name { get => name; set => name = value; }
	public string Description { get => description; set => description = value; }
	public int Health { get => health; set => health = value; }
	public int DamageOnHit { get => damageOnHit; set => damageOnHit = value; }
	public LayerMask TargetDetectionMask { get => targetDetectionMask; set => targetDetectionMask = value; }

	public float MovementSpeed { get => movementSpeed; set => movementSpeed = value; }
	public float MovementDestinationInterval { get => movementDestinationInterval; set => movementDestinationInterval = value; }

	public float TargetDetectionRange { get => targetDetectionRange; set => targetDetectionRange = value; }
	public float TargetAttackRange { get => targetAttackRange; set => targetAttackRange = value; }
	#endregion
}
