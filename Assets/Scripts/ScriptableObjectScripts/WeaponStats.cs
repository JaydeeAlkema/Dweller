using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon Stats", menuName = "Inventory/Stats/WeaponStats")]
public class WeaponStats : ScriptableObject
{
	#region Private Variables
	[Header("Weapon Stats")]
	public int damageOnHit = 10;            // How much damage the weapon deals on hit.
	public int durability = 100;            // How many hits this weapon can dish out. a.k.a. health
	public float attackRange = 0.5f;		// How far this weapon reaches. (Only applies to Melee weapons)
	#endregion
}
