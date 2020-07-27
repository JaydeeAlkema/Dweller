using UnityEngine;

/// <summary>
/// This is most likely a temp class. Just to test out 
/// </summary>
public class WeaponBehaviour : MonoBehaviour
{
	#region Private Variables
	[Header("Weapon Properties")]
	[SerializeField] private WeaponStats stats = default;			// Reference to the Stats Scriptable Object.
	[SerializeField] private Animator anim = default;				// Reference to the animator component.
	[SerializeField] private Transform attackPoint;					// From which point we check for entities within the attack range.
	[SerializeField] private LayerMask targetLayer = default;		// What layers to check for entities to damage.
	#endregion

	#region Monobehaviour Callbacks
	private void Update()
	{
		if(Input.GetMouseButtonDown(0))
		{
			anim.SetTrigger("Strike_0");
			HitEntitiesWithinAttackRange();
		}
	}

	/// <summary>
	/// Get's all the entities from the same layer that are within the attack range.
	/// Calls the IDamageable Damage function if it has the interface implemented.
	/// </summary>
	private void HitEntitiesWithinAttackRange()
	{
		Collider2D[] hitEntities = Physics2D.OverlapCircleAll(attackPoint.position, stats.AttackRange, targetLayer);

		foreach(Collider2D entity in hitEntities)
		{
			entity.GetComponent<IDamageable>()?.Damage(stats.DamageOnHit);
		}
	}
	#endregion

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(attackPoint.position, stats.AttackRange);
	}
}
