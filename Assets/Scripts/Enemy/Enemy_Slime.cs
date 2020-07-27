using Pathfinding;
using System.Collections;
using UnityEngine;

public class Enemy_Slime : EnemyBehaviour, IDamageable
{
	[Header("Custom Properties")]
	[SerializeField] private int currentHealth = default;             // The current health of the Enemy.
	[SerializeField] private float targetDetectionInterval = default; // How often the Enemy will check for an target that is within range.
	[SerializeField] private float targetAttackCheckInterval = default;    // Time checks if the enemy can attack the player.
	[SerializeField] private float targetAttackInterval = default;    // Time between attacks.
	[SerializeField] private Rigidbody2D rb2d = default;

	public Enemy_Slime(EnemyStats _enemyStats, AIPath _pathfinder) : base(_enemyStats, _pathfinder)
	{

	}

	#region Monobehaviour Callbacks
	private void Start()
	{
		Initialize();

		StartCoroutine(SetActiveWhenTargetInRangeOnInterval());
	}

	private void Update()
	{
		SetAnimation();
		FlipSprite();
	}
	#endregion

	#region Overrides
	/// <summary>
	/// Override of InitializeStats
	/// </summary>
	protected override void Initialize()
	{
		base.Initialize();
		currentHealth = enemyStats.Health;
	}
	#endregion

	#region Target Detection & Target Attacking implementation
	/// <summary>
	/// A simple IEnumerator for the target detection. No need to check every frame.
	/// </summary>
	/// <returns></returns>
	private IEnumerator SetActiveWhenTargetInRangeOnInterval()
	{
		while(true)
		{
			FollowTargetWhenInRange();
			yield return new WaitForSeconds(targetDetectionInterval);
		}
	}
	#endregion

	#region IDamageable Interface Implemenation
	/// <summary>
	/// IDamageable Interface Damage Implementation.
	/// </summary>
	/// <param name="value"></param>
	public void Damage(int value)
	{
		currentHealth -= value;
		StartCoroutine(OnDamageTakenEvent());
		if(currentHealth <= 0) Destroy(gameObject);
	}

	/// <summary>
	/// The event that happens when the enemy get's hit.
	/// This is primarely used to simulate a knockback effect.
	/// Works pretty good if you ask me :)
	/// </summary>
	/// <returns></returns>
	private IEnumerator OnDamageTakenEvent()
	{
		pathfinder.enabled = false;
		rb2d.AddForce((transform.position - targetTransform.position).normalized * 500f * Time.deltaTime, ForceMode2D.Impulse);

		yield return new WaitForSeconds(0.5f);

		pathfinder.enabled = true;
		rb2d.velocity = Vector2.zero;
	}
	#endregion

	private void OnDrawGizmosSelected()
	{
		// Target Detection Range
		Gizmos.color = Color.magenta;
		Gizmos.DrawWireSphere(transform.position, enemyStats.TargetDetectionRange);

		// Target Attack Range
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, enemyStats.TargetAttackRange);
	}
}
