using UnityEngine;

/// <summary>
/// The Inventory UI class handles everything that is related to the UI.
/// </summary>
public class InventoryUI : MonoBehaviour
{
	#region Private Variables
	[SerializeField] private Transform inventoryTransform = null;		// Reference to the entire Inventory UI Transform.
	[SerializeField] private Transform itemsParent = null;				// Parent Transform of all the inventory slots.
	[SerializeField] private KeyCode inventoryShowKey = KeyCode.I;		// Which key to press to show the inventory UI.

	private Inventory inventory = null;
	private InventorySlot[] slots = null;
	private bool inventoryIsVisible = true;
	#endregion

	#region Monobehaviour Callbacks
	private void Update()
	{
		if(Input.GetKeyDown(inventoryShowKey))
		{
			ShowInventory();
		}
	}

	private void Start()
	{
		ShowInventory();
		inventory = Inventory.instance;
		inventory.onItemChangedCallBack += UpdateUI;

		slots = itemsParent.GetComponentsInChildren<InventorySlot>();
	}
	#endregion
	/// <summary>
	/// Shows the Inventory UI.
	/// </summary>
	void ShowInventory()
	{
		inventoryIsVisible = !inventoryIsVisible;

		inventoryTransform.gameObject.SetActive(inventoryIsVisible);
	}

	/// <summary>
	/// Updates the Inventory UI.
	/// </summary>
	private void UpdateUI()
	{
		Debug.Log("Updating UI");

		for(int i = 0; i < slots.Length; i++)
		{
			if(i < inventory.Items.Count)
			{
				slots[i].AddItem(inventory.Items[i]);
			}
			else
			{
				slots[i].ClearSlot();
			}
		}
	}
}
