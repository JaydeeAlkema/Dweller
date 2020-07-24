using System.Collections;
using UnityEngine;

/// <summary>
/// The Player Behaviour class is responsible for all the behaviour of the Player EXCEPT movement.
/// </summary>
public class PlayerBehaviour : MonoBehaviour, IDamageable
{
	#region Private Variables
	[Header("Player Stats")]
	[SerializeField] private PlayerStats stats = default;		// Reference to the Player Stats Scriptable Object. This holds all the stats for the player, this differs per character.
	[SerializeField] private int currentHitPoints = default;	// Current amount of health.

	[Header("Components")]
	[SerializeField] private Rigidbody2D rb2d = default;		// Reference to the Rigidbody2D component.
	[SerializeField] private Animator anim = default;			// Reference to the animator component.
	[SerializeField] private PlayerMovement movement = default; // Reference to the PlayerMovement Component.
	#endregion

	#region Public Properties
	public PlayerStats Stats => stats;
	#endregion

	#region Monobehaviour Callbacks
	private void Start()
	{
		currentHitPoints = stats.HitPoints;
	}

	private void Update()
	{

	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if(collision.CompareTag("Enemy"))
		{
			StartCoroutine(OnEnemyCollision(collision));
		}
	}
	#endregion

	#region IDamageable Implementation
	public void Damage(int value)
	{
		anim.SetTrigger("Hit");
		currentHitPoints -= value;
		UIManager.Instance.UpdatePlayerHealthUI(stats.HitPoints, currentHitPoints);
	}
	#endregion


	private IEnumerator OnEnemyCollision(Collider2D collision)
	{
		movement.enabled = false;
		rb2d.AddForce((transform.position - collision.transform.position).normalized * 200f * Time.deltaTime, ForceMode2D.Impulse);
		Damage(collision.GetComponent<EnemyBehaviour>().EnemyStats.DamageOnHit);
		yield return new WaitForSeconds(0.5f);
		movement.enabled = true;
	}
}
