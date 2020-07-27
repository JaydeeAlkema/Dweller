﻿using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon Stats", menuName = "ScriptableObjects/New Weapon Stats")]
public class WeaponStats : ScriptableObject
{
	#region Private Variables
	[Header("Weapon Name and Description")]
	[SerializeField] private Sprite sprite = default;			// Sprite of the Weapon.
	[SerializeField] private new string name = default;         // Name of the Weapon.
	[SerializeField] private string description = default;      // Description of the Weapon.

	[Header("Weapon Stats")]
	[SerializeField] private int damageOnHit = 10;              // How much damage the weapon deals on hit.
	[SerializeField] private int durability = 100;              // How many hits this weapon can dish out. a.k.a. health
	[SerializeField] private float attackRange = 0.5f;			// How far this weapon reaches. (Only applies to Melee weapons)
	#endregion

	#region Public Properties
	public Sprite Sprite { get => sprite; set => sprite = value; }
	public string Name { get => name; set => name = value; }
	public string Description { get => description; set => description = value; }

	public int DamageOnHit { get => damageOnHit; set => damageOnHit = value; }
	public int Durability { get => durability; set => durability = value; }
	public float AttackRange { get => attackRange; set => attackRange = value; }
	#endregion
}
