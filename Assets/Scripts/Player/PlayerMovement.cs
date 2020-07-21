using UnityEngine;

/// <summary>
/// The PlayerMovement class is responsible for moving the player.
/// This is includes walking, running and aiming the current weapon towards the mouse.
/// </summary>
public class PlayerMovement : MonoBehaviour
{
	#region Private Variables
	[Header("Movement Stats")]
	[SerializeField] private float walkSpeed = 2f;                      // How fast you walk
	[SerializeField] private float runSpeed = 5.5f;                     // How fast you run
	[SerializeField] private float movementVelocity = default;          // The final velocity. Depending on player input.

	[Header("Movement Input")]
	[SerializeField] private string horizontalAxis = "Horizontal";      // the name of the Horizontal Input Axis.
	[SerializeField] private string verticalAxis = "Vertical";          // the name of the Vertical Input Axis.
	[SerializeField] private KeyCode runKey = KeyCode.LeftShift;        // Which key to press to start running.
	[SerializeField] private Vector2 movementInputVector = default;     // Vector with the movement input variables.

	[Header("Weapon Atributes")]
	[SerializeField] private Transform weaponPivot = default;           // Refernce to the current weapon pivot.

	private Rigidbody2D rb2d = default;                                 // Reference to the Rigidbody2D component.
	#endregion

	#region Monobehaviour Callbacks
	private void Awake()
	{
		rb2d = GetComponent<Rigidbody2D>();
	}

	private void Update()
	{
		ProcessPlayerInput();
		RotateWeaponPivotTowardsMouse();
	}

	private void FixedUpdate()
	{
		Move();
	}
	#endregion

	#region Movement
	/// <summary>
	/// Processes player input. Not just for movement but for attacking in the future aswell.
	/// </summary>
	private void ProcessPlayerInput()
	{
		// Basic Movement
		movementInputVector.x = Input.GetAxisRaw(horizontalAxis);
		movementInputVector.y = Input.GetAxisRaw(verticalAxis);
		// Add some lerp between walk and run?
		movementVelocity = Input.GetKey(runKey) ? runSpeed : walkSpeed;

		// Handle other input like attacking, interaction, etc.
	}

	/// <summary>
	/// Moves the Rigidbody2D according to the input giving by the player.
	/// </summary>
	private void Move()
	{
		rb2d.MovePosition(rb2d.position + movementInputVector.normalized * movementVelocity * Time.fixedDeltaTime);
	}
	#endregion

	#region Weapon Rotation
	/// <summary>
	/// Rotates the WeaponPivot towards the mouse posiution on screen.
	/// This is done to indicate to the player which way the character is aiming and thus can see what they could potentialy hit.
	/// </summary>
	private void RotateWeaponPivotTowardsMouse()
	{
		Vector3 dir = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
		float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
		weaponPivot.rotation = Quaternion.AngleAxis(angle - 90f, Vector3.forward);
	}
	#endregion
}
