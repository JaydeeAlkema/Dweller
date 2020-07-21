using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The PlayerMovement class is responsible for moving the player.
/// This is includes walking, running and aiming towards the mouse.
/// </summary>
public class PlayerMovement : MonoBehaviour
{
	#region Private Variables
	[Header("Movement Stats")]
	[SerializeField] private float walkSpeed = 5f;						// How fast you walk
	[SerializeField] private float runSpeed = 8f;						// How fast you run
	[SerializeField] private float movementVelocity = default;          // The final velocity. Depending on player input.

	[Header("Movement Input")]
	[SerializeField] private string horizontalAxis = "Horizontal";		// the name of the Horizontal Input Axis.
	[SerializeField] private string verticalAxis = "Vertical";          // the name of the Vertical Input Axis.
	[SerializeField] private KeyCode runKey = KeyCode.LeftShift;        // Which key to press to start running.
	#endregion

	#region Monobehaviour Callbacks
	private void Update()
	{
		Vector3 dir = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
		float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.AngleAxis(angle - 90f, Vector3.forward);
	}
	#endregion
}
