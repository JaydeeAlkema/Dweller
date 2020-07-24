using UnityEngine;

/// <summary>
/// This is most likely a temp class. Just to test out 
/// </summary>
public class WeaponBehaviour : MonoBehaviour
{
	#region Private Variables
	[Header("Weapon Properties")]
	[SerializeField] private WeaponStats stats = default;
	#endregion

	#region Monobehaviour Callbacks
	private void OnTriggerEnter2D(Collider2D collision)
	{
		collision.gameObject.GetComponent<IDamageable>()?.Damage(stats.DamageOnHit);
	}
	#endregion
}
