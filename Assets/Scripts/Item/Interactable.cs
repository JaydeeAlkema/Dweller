using UnityEngine;

public class Interactable : MonoBehaviour
{
	#region Private/Protected Variables
	public float interactionRadius = 0.5f;         // Radius which whitin the playertransform has to be to interact with it.
	public Transform interactionTransform = default;     // Reference to the interaction transform. i.e. the item itself.
	public Transform playerTransform = default;    // Reference to the Player Transform.
	#endregion

	/// <summary>
	/// Base Interaction function. This is meant to be overwritten.
	/// </summary>
	protected virtual void Interact()
	{

	}

	/// <summary>
	/// Checks if the player transform is within range of the item to interact with it.
	/// </summary>
	/// <returns></returns>
	protected bool CanInteract()
	{
		return Vector2.Distance(transform.position, playerTransform.position) < interactionRadius ? true : false;
	}

	private void OnDrawGizmosSelected()
	{
		if(!interactionTransform)
			interactionTransform = transform;

		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere(interactionTransform.position, interactionRadius);
	}
}
