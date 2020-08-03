using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The Inventory class holds all the items.
/// </summary>
public class Inventory : MonoBehaviour
{
	#region Singleton
	public static Inventory instance = null;

	private void Awake()
	{
		if(!instance || instance != this)
			instance = this;
	}
	#endregion

	#region Private Variables
	[SerializeField] private int space = 2;    // How much space the inventory has. a.k.a. slots.
	[SerializeField] private List<Item> items = new List<Item>();   // List with all the items in the inventory..
	#endregion

	#region Public Properties
	public List<Item> Items { get => items; set => items = value; }
	#endregion

	#region Public Voids
	public delegate void OnItemChanged();
	public OnItemChanged onItemChangedCallBack;

	/// <summary>
	/// Add an item to the Items list.
	/// </summary>
	/// <param name="item"> Which Item to add to the list. </param>
	public bool Add(Item item)
	{
		if(!item.isDefaultItem)
		{
			if(items.Count >= space)
			{
				Debug.Log("Not enough room!");
				return false;
			}
			items.Add(item);

			if(onItemChangedCallBack != null)
				onItemChangedCallBack.Invoke();
		}
		return true;
	}

	/// <summary>
	/// 
	/// Removes and item from the Items list.
	/// </summary>
	/// <param name="item"> Which Item to remove from the list. </param>
	public void Remove(Item item)
	{
		items.Remove(item);

		if(onItemChangedCallBack != null)
			onItemChangedCallBack.Invoke();
	}
	#endregion
}
