using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// The InventorySlot Class is responsible for it's own Inventory Slot in the UI.
/// It handles showing the necessary information to the player and interaction.
/// </summary>
public class InventorySlot : MonoBehaviour
{
	#region Private Variables
	[SerializeField] private Item item = null;
	[SerializeField] private Image icon = null;
	[SerializeField] private Button removeButton = null;
	#endregion

	/// <summary>
	/// Add item to the slot and display the item icon.
	/// </summary>
	/// <param name="newItem"></param>
	public void AddItem(Item newItem)
	{
		item = newItem;
		icon.sprite = item.icon;
		icon.enabled = true;
		removeButton.interactable = true;
	}

	/// <summary>
	/// Clears the slot of it's item and disable the icon and remove button.
	/// </summary>
	public void ClearSlot()
	{
		item = null;
		icon.sprite = null;
		icon.enabled = false;
		removeButton.interactable = false;
	}

	/// <summary>
	/// A simple Use Item void on button click.
	/// </summary>
	public void UseItem()
	{
		if(item != null)
		{
			item.Use();
		}
	}

	/// <summary>
	/// On Remove Button Click Event.
	/// </summary>
	public void OnRemoveButtonClick()
	{
		Inventory.instance.Remove(item);
	}
}
