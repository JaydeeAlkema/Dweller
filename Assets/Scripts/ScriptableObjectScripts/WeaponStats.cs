using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon Stats", menuName = "ScriptableObjects/New Weapon Stats")]
public class WeaponStats : ScriptableObject
{
	#region Private Variables
	[Header("Weapon Name and Description")]
	[SerializeField] private new string name = default;         // Name of the Weapon.
	[SerializeField] private string description = default;      // Description of the Weapon.

	[Header("Weapon Stats")]
	[SerializeField] private int damageOnHit = 10;              // How much damage the weapon deals on hit.
	[SerializeField] private int durability = 100;              // How many hits this weapon can dish out. a.k.a. health
	#endregion

	#region Public Properties
	public string Name { get => name; set => name = value; }
	public string Description { get => description; set => description = value; }

	public int DamageOnHit { get => damageOnHit; set => damageOnHit = value; }
	public int Durability { get => durability; set => durability = value; }
	#endregion
}
