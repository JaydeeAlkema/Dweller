using Pathfinding;
using System.Collections;
using UnityEngine;

public class Enemy_Slime : EnemyBehaviour
{
	public Enemy_Slime(EnemyStats _enemyStats, AIPath _pathfinder) : base(_enemyStats, _pathfinder)
	{
		// Custom Stuff
	}

	private void Start()
	{
		InitializeStats();

		StartCoroutine(SetActiveWhenTargetInRangeOnInterval());
	}

	private void Update()
	{
		SetAnimation();
	}

	/// <summary>
	/// A simple IEnumerator for the target detection. No need to check every frame.
	/// </summary>
	/// <returns></returns>
	private IEnumerator SetActiveWhenTargetInRangeOnInterval()
	{
		while(state != EnemyState.Chasing)
		{
			SetActiveWhenTargetInRange();
			yield return new WaitForSeconds(0.25f);
		}
	}

	private void OnDrawGizmos()
	{
		// Target Detection Range
		Gizmos.color = Color.magenta;
		Gizmos.DrawWireSphere(transform.position, enemyStats.TargetDetectionRange);

		// Target Attack Range
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, enemyStats.TargetAttackRange);
	}
}
