using System.Collections;
using UnityEngine;

/// <summary>
/// This is most likely a temp class. Just to test out 
/// </summary>
public class WeaponBehaviour : MonoBehaviour
{
	#region Private Variables
	[Header("Weapon Properties")]
	[SerializeField] private Item weapon = default;                   // Reference to the Item Scriptable Object.
	[Space]
	[SerializeField] private Animator anim = default;                       // Reference to the animator component.
	[SerializeField] private SpriteRenderer spriteRenderer = default;       // Reference to the SpriteRenderer of the Weapon.
	[SerializeField] private Transform attackPoint = default;               // From which point we check for entities within the attack range.
	[SerializeField] private Transform weaponRotPivot = default;            // Reference to the weapon rotation pivot.
	[SerializeField] private LayerMask targetLayer = default;               // What layers to check for entities to damage.

	private bool canAttack = true;
	#endregion

	#region Public Properties
	public Item Weapon { get => weapon; set => weapon = value; }
	#endregion

	#region Monobehaviour Callbacks
	private void Start()
	{
		spriteRenderer.sprite = Weapon.icon;
	}

	private void Update()
	{
		if(canAttack) RotateWeaponPivotTowardsMouse();
		if(Input.GetMouseButtonDown(0) && canAttack)
		{
			StartCoroutine(WeaponAttackCooldown());
			anim.SetTrigger("Strike_0");
			HitEntitiesWithinAttackRange();
		}
	}
	#endregion

	#region Private Voids
	/// <summary>
	/// Get's all the entities from the same layer that are within the attack range.
	/// Calls the IDamageable Damage function if it has the interface implemented.
	/// </summary>
	private void HitEntitiesWithinAttackRange()
	{
		Collider2D[] hitEntities = Physics2D.OverlapCircleAll(attackPoint.position, Weapon.weaponStats.attackRange, targetLayer);

		foreach(Collider2D entity in hitEntities)
		{
			Debug.Log("Hit: " + entity.name);
			int critChance = Random.Range(0, 100);
			int damage = Weapon.weaponStats.damageOnHit;

			if(critChance < Weapon.weaponStats.Critical)
			{
				damage *= 2;
				FloatingDamageNumberSpawner.Instance.InstantiateFloatingDamageNumberAtPos(entity.transform.position, damage.ToString(), true);
			}
			else
			{
				FloatingDamageNumberSpawner.Instance.InstantiateFloatingDamageNumberAtPos(entity.transform.position, damage.ToString(), false);
			}

			entity.GetComponent<IDamageable>()?.Damage(damage);
		}
	}
	#endregion

	#region Private IEnumerators
	private IEnumerator WeaponAttackCooldown()
	{
		canAttack = false;
		yield return new WaitForSeconds(0.5f);
		canAttack = true;
	}
	#endregion

	#region Weapon Rotation
	/// <summary>
	/// Rotates the WeaponPivot towards the mouse posiution on screen.
	/// This is done to indicate to the player which way the character is aiming and thus can see what they could potentialy hit.
	/// </summary>
	private void RotateWeaponPivotTowardsMouse()
	{
		Vector3 dir = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
		float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
		weaponRotPivot.rotation = Quaternion.AngleAxis(angle - 90f, Vector3.forward);
	}
	#endregion

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(attackPoint.position, Weapon.weaponStats.attackRange);
	}
}
