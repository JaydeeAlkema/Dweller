using UnityEngine;

/// <summary>
/// ItemPickup is responsible for all the interaction that should happen when picking up an Interactable.
/// </summary>
public class ItemPickup : Interactable
{
	#region Private Variables
	[SerializeField] private Item item;

	Inventory inventory = null;
	UIManager uiManager = null;
	#endregion

	#region Monobehaviour Callbacks
	private void Start()
	{
		inventory = Inventory.instance;
		uiManager = UIManager.Instance;
	}

	private void Update()
	{
		if(Input.GetMouseButtonDown(1))
			if(CanInteract())
				Interact();
			else
				Debug.Log("Too far away to interact with: " + item.name);
	}

	private void OnMouseOver()
	{
		uiManager.ShowFloatingInfoPanel(item);
	}

	private void OnMouseExit()
	{
		uiManager.HideFloatingInfoPanel();
	}
	#endregion

	/// <summary>
	/// Interactable Interact Method Override
	/// </summary>
	protected override void Interact()
	{
		base.Interact();
		PickUp();
	}

	/// <summary>
	/// Pick up logic.
	/// </summary>
	private void PickUp()
	{
		uiManager.HideFloatingInfoPanel();
		Debug.Log("Picking up " + item.name);
		bool wasPickedUp = inventory.Add(item);

		if(wasPickedUp)
		{
			Destroy(gameObject);
		}
	}
}
