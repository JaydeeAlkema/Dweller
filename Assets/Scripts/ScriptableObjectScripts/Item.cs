using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
	[Header("Base Item Properties")]
	public new string name = "New Item";
	public string description = "Item Info";
	public Sprite icon = null;
	public bool isDefaultItem = false;

	[Header("Custom Stats")]
	[Tooltip("Leave this field empty when not necessary!")] public WeaponStats weaponStats = null;
	// Add Gear Stats

	public virtual void Use()
	{
		// use item.

		Debug.Log("Using " + name);
	}
}
