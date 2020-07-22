using Pathfinding;
using UnityEngine;

/// <summary>
/// The enemy behaviour class is responsible for the enemy behaviour...
/// It handles target detection aswell as navigation towards it.
/// </summary>
public class EnemyBehaviour : MonoBehaviour
{
	#region Private Variables
	protected enum EnemyState
	{
		Idle,
		Chasing,
		Attacking
	}

	[Header("Enemy Properties")]
	[SerializeField] protected EnemyState state = EnemyState.Idle;
	[SerializeField] protected EnemyStats enemyStats = default;
	[SerializeField] protected AIPath pathfinder = default;
	[SerializeField] protected Transform targetTransform = default;       // Target Transform.
	[Space]
	[SerializeField] protected new string name = "";                      // Name of the Enemy. Also uses this name on the gameobject.
	[SerializeField] protected string description = "";                   // Description of this enemy. Maybe used later for in a journal kind of menu?
	[SerializeField] protected int health = 100;                       // Health of this enemy.
	[SerializeField] protected int damageOnHit = 10;                   // How much damage the enemy deals when hitting it's target.

	private float movementSpeed = 2.5f;                // The enemy can't run, And the general movement speed is lower than the player.
	private float movementDestinationInterval = 0.25f; // How many seconds inbetween target destination is set. Better performance.	

	private float targetDetectionRange = 6.5f;         // Range within the enemy can detect targets.
	private float targetAttackRange = 2f;              // Range within the enemy can attack the target.

	private Animator anim = default;
	private LayerMask targetDetectionMask = default;
	#endregion

	public EnemyBehaviour(EnemyStats _enemyStats, AIPath _pathfinder)
	{
		enemyStats = _enemyStats;
		pathfinder = _pathfinder;
	}

	#region Monobehaviour Callbacks
	private void Start()
	{
		InitializeStats();
	}

	private void Update()
	{
		SetActiveWhenTargetInRange();
		SetAnimation();
	}
	#endregion

	#region Protected Voids
	/// <summary>
	/// Initializes all the stats from the EnemyStats object.
	/// </summary>
	protected virtual void InitializeStats()
	{
		targetTransform = GameManager.Instance.PlayerInstance.transform;
		anim = GetComponentInChildren<Animator>();
		targetDetectionMask = enemyStats.TargetDetectionMask;

		name = enemyStats.Name;
		description = enemyStats.Description;
		health = enemyStats.Health;
		damageOnHit = enemyStats.DamageOnHit;

		movementSpeed = enemyStats.MovementSpeed;
		movementDestinationInterval = enemyStats.MovementDestinationInterval;

		targetDetectionRange = enemyStats.TargetDetectionRange;
		targetAttackRange = enemyStats.TargetAttackRange;

		pathfinder.canSearch = false;
		pathfinder.maxSpeed = movementSpeed;
		pathfinder.repathRate = movementDestinationInterval;
	}

	/// <summary>
	/// The target will be acquired through the GameManager.
	/// This will check if the target is within the detection range to set this enemy active.
	/// </summary>
	protected virtual void SetActiveWhenTargetInRange()
	{
		// First check if the target is in range.
		if(Vector2.Distance(transform.position, targetTransform.position) < targetDetectionRange)
		{
			// Check if there are no obstacle in the way, Avoiding enemies detecting the target through walls etc.
			RaycastHit2D hit = Physics2D.Linecast(transform.position, targetTransform.position, targetDetectionMask);
			if(hit.collider != null)
			{
				if(hit.collider.gameObject.CompareTag("Player"))
				{
					pathfinder.canSearch = true;
					state = EnemyState.Chasing;
					Debug.DrawLine(transform.position, targetTransform.position, Color.green);
				}
				else
				{
					Debug.DrawLine(transform.position, targetTransform.position, Color.red);
				}
			}
		}
	}

	/// <summary>
	/// Sets the enemy animation depending on it's state.
	/// </summary>
	protected virtual void SetAnimation()
	{
		anim.SetInteger("State", (int)state);
	}
	#endregion

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.blue;
		Gizmos.DrawWireSphere(transform.position, targetDetectionRange);
	}

}
