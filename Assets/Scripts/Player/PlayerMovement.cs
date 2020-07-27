using UnityEngine;

/// <summary>
/// The PlayerMovement class is responsible for moving the player.
/// This is includes walking, running and aiming the current weapon towards the mouse.
/// </summary>
public class PlayerMovement : MonoBehaviour
{
	#region Private Variables
	private enum PlayerState
	{
		Idle,
		Running,
	}

	[SerializeField] private PlayerState state = PlayerState.Idle;
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
	[SerializeField] private Transform weaponPivot = default;           // Reference to the current weapon pivot.

	[Header("Components")]
	[SerializeField] private Rigidbody2D rb2d = default;                // Reference to the Rigidbody2D component.
	[SerializeField] private Animator anim = default;                   // Reference to the Animator component.
	[SerializeField] private SpriteRenderer sprite = default;           // Reference to the SpriteRenderer Component.
	#endregion

	#region Monobehaviour Callbacks
	private void Update()
	{
		// Return when the game is over.
		if(GameManager.Instance.GameState == GameState.GameOver) return;

		ProcessPlayerInput();
		FlipSprite();
		SetPlayerState();
		SetAnimation();
	}

	private void FixedUpdate()
	{
		// Return when the game is over.
		if(GameManager.Instance.GameState == GameState.GameOver) return;

		Move();
		RotateWeaponPivotTowardsMouse();
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
	}

	/// <summary>
	/// Set's the PlayerState depending on the movementvector.
	/// </summary>
	private void SetPlayerState()
	{
		if(movementInputVector == Vector2.zero)
			state = PlayerState.Idle;
		else
			state = PlayerState.Running;
	}

	/// <summary>
	/// Moves the Rigidbody2D according to the input giving by the player.
	/// </summary>
	private void Move()
	{
		rb2d.MovePosition(rb2d.position + movementInputVector.normalized * movementVelocity * Time.fixedDeltaTime);
	}

	/// <summary>
	/// Flips the sprite depending on the direction the player is moving.
	/// </summary>
	private void FlipSprite()
	{
		if(movementInputVector.x > 0f)
			sprite.flipX = false;
		else if(movementInputVector.x < 0f)
			sprite.flipX = true;

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

	#region Animations
	/// <summary>
	/// Sets the animation appropriately.
	/// </summary>
	private void SetAnimation()
	{
		anim.SetInteger("State", (int)state);
	}
	#endregion
}
