using Pathfinding;
using System.Collections;
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
		Chasing
	}

	[Header("Enemy Properties")]
	[SerializeField] protected LayerMask LinecastHitMask;
	[SerializeField] protected EnemyState state = EnemyState.Idle;
	[SerializeField] protected EnemyStats enemyStats = default;
	[SerializeField] protected AIPath pathfinder = default;
	[SerializeField] protected Transform targetTransform = default;       // Target Transform.

	private new string name = "";                      // Name of the Enemy. Also uses this name on the gameobject.
	private string description = "";                   // Description of this enemy. Maybe used later for in a journal kind of menu?
	private int health = 100;                       // Health of this enemy.
	private int damageOnHit = 10;                   // How much damage the enemy deals when hitting it's target.

	private float movementSpeed = 2.5f;                // The enemy can't run, And the general movement speed is lower than the player.
	private float movementDestinationInterval = 0.25f; // How many seconds inbetween target destination is set. Better performance.	

	private float targetDetectionRange = 6.5f;         // Range within the enemy can detect targets.

	private Animator anim = default;
	private RaycastHit2D hit = default;
	private AIDestinationSetter destinationSetter = default;
	private SpriteRenderer spriteRenderer = default;

	#endregion

	#region Public Properties
	public EnemyStats EnemyStats { get => enemyStats; }
	#endregion

	public EnemyBehaviour(EnemyStats _enemyStats, AIPath _pathfinder)
	{
		enemyStats = _enemyStats;
		pathfinder = _pathfinder;
	}

	#region Protected Voids
	/// <summary>
	/// Initializes the EnemyBehaviour Class with all the required EnemyStats, Components, etc.
	/// </summary>
	protected virtual void Initialize()
	{
		targetTransform = GameManager.Instance.PlayerInstance.transform;
		anim = GetComponentInChildren<Animator>();
		LinecastHitMask = enemyStats.TargetDetectionMask;

		destinationSetter = GetComponent<AIDestinationSetter>();
		destinationSetter.target = targetTransform;

		spriteRenderer = GetComponentInChildren<SpriteRenderer>();

		name = enemyStats.Name;
		description = enemyStats.Description;
		health = enemyStats.Health;
		damageOnHit = enemyStats.DamageOnHit;

		movementSpeed = enemyStats.MovementSpeed;
		movementDestinationInterval = enemyStats.MovementDestinationInterval;

		targetDetectionRange = enemyStats.TargetDetectionRange;

		pathfinder.canSearch = false;
		pathfinder.maxSpeed = movementSpeed;
		pathfinder.repathRate = movementDestinationInterval;
	}

	/// <summary>
	/// The target will be acquired through the GameManager.
	/// This will check if the target is within the detection range to set this enemy active.
	/// </summary>
	protected virtual void FollowTargetWhenInRange()
	{
		// Return when the game is over.
		if(GameManager.Instance.GameState == GameState.GameOver) return;

		if(state == EnemyState.Idle)
		{
			if(CanFollowTarget())
			{
				if(CanSeeTargetWithTag("Player"))
				{
					state = EnemyState.Chasing;
					pathfinder.canSearch = true;
				}
			}
		}
	}

	/// <summary>
	/// Sets the enemy animation depending on it's state.
	/// </summary>
	protected virtual void SetAnimation()
	{
		// Return when the game is over.
		if(GameManager.Instance.GameState == GameState.GameOver) return;

		anim.SetInteger("State", (int)state);
	}

	/// <summary>
	/// Flips the sprite depending on which way the enemy SHOULD be facing the target.
	/// </summary>
	protected void FlipSprite()
	{
		// Return when the game is over.
		if(GameManager.Instance.GameState == GameState.GameOver) return;

		// Get direction. Flip sprite accordingly.
		Vector2 direction = transform.position - targetTransform.position;
		spriteRenderer.flipX = direction.x < 0 ? false : true;
	}
	#endregion

	#region Private Voids
	/// <summary>
	/// Checks of the enemy can see the target with the given tag.
	/// Also Checks if there are no obstacle in the way, Avoiding the problem where the enemy detects the target through walls etc.
	/// </summary>
	/// <param name="tag"></param>
	/// <returns></returns>
	private bool CanSeeTargetWithTag(string tag)
	{
		hit = Physics2D.Linecast(transform.position, targetTransform.position, LinecastHitMask);
		if(hit.collider != null)
			return hit.collider.gameObject.CompareTag(tag) ? true : false;
		else
			return false;
	}

	/// <summary>
	/// Checks if the enemy is close enough to the target to follow, but not too close to attack.
	/// </summary>
	/// <returns></returns>
	private bool CanFollowTarget()
	{
		return Vector2.Distance(transform.position, targetTransform.position) < targetDetectionRange ? true : false;
	}
	#endregion
}
