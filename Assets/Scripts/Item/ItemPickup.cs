using UnityEngine;

/// <summary>
/// ItemPickup is responsible for all the interaction that should happen when picking up an Interactable.
/// </summary>
public class ItemPickup : Interactable
{
	#region Private Variables
	[SerializeField] private Item item;

	Inventory inventory = null;
	#endregion

	#region Monobehaviour Callbacks
	private void Start()
	{
		inventory = Inventory.instance;
	}

	private void Update()
	{
		if(Input.GetMouseButtonDown(1))
			if(CanInteract())
				Interact();
			else
				Debug.Log("Too far away to interact with: " + item.name);
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
		Debug.Log("Picking up " + item.name);
		bool wasPickedUp = inventory.Add(item);

		if(wasPickedUp)
		{
			Destroy(gameObject);
		}
	}
}
