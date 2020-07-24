using UnityEngine;

/// <summary>
/// This is most likely a temp class. Just to test out 
/// </summary>
public class WeaponBehaviour : MonoBehaviour
{
	#region Private Variables
	[Header("Weapon Properties")]
	[SerializeField] private WeaponStats stats = default;
	[SerializeField] private Animator anim = default;
	[SerializeField] private Collider2D weaponCollider = default;
	#endregion

	#region Monobehaviour Callbacks
	private void Update()
	{
		if(Input.GetMouseButtonDown(0))
		{
			anim.SetTrigger("Strike_0");
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if(weaponCollider.IsTouching(collision))
			if(collision.gameObject.CompareTag("Enemy"))
				collision.gameObject.GetComponent<IDamageable>()?.Damage(stats.DamageOnHit);
	}
	#endregion
}
