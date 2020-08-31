using System.Collections;
using System.Threading;
using UnityEngine;

/// <summary>
/// The Player Behaviour class is responsible for all the behaviour of the Player EXCEPT movement.
/// </summary>
public class PlayerBehaviour : MonoBehaviour, IDamageable
{
	#region Private Variables
	[Header("Player Stats")]
	[SerializeField] private PlayerStats stats = default;           // Reference to the Player Stats Scriptable Object. This holds all the stats for the player, this differs per character.
	[SerializeField] private int currentHitPoints = default;        // Current amount of health.
	[Space]
	[SerializeField] private int attack = 0;        // Attack Stat
	[SerializeField] private int defense = 0;       // Defense Stat
	[SerializeField] private int speed = 0;         // Speed Stat
	[SerializeField] private int evasion = 0;       // Evasion Stat
	[SerializeField] private int accuracy = 0;      // Accuracy Stat
	[SerializeField] private int critical = 0;      // Critical Stat

	[Header("Components")]
	[SerializeField] private Rigidbody2D rb2d = default;            // Reference to the Rigidbody2D component.
	[SerializeField] private Animator anim = default;               // Reference to the animator component.
	[SerializeField] private PlayerMovement movement = default;     // Reference to the PlayerMovement Component.
	[SerializeField] private Collider2D playerCollider = default;   // Reference to the Player collider to avoid double OnTriggerEnter events.
	[SerializeField] private SpriteRenderer spriteRenderer = default;   // Reference to the Sprite Renderer.
	[SerializeField] private WeaponBehaviour weaponBehaviour = default;      // Rerference to the WeaponBehaviour Component.
	#endregion

	#region Public Properties
	public PlayerStats Stats => stats;
	#endregion

	#region Monobehaviour Callbacks
	private void Start()
	{
		InitializeStats();
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if(playerCollider.IsTouching(collision))
			if(collision.CompareTag("Enemy"))
				StartCoroutine(OnEnemyCollision(collision));
	}
	#endregion

	#region Stats Initialisation
	/// <summary>
	/// Initializes all the player stats. 
	/// The stats calculations are very straightforward and nothing specials. But they have to be done :)
	/// </summary>
	public void InitializeStats()
	{
		// Hitpoints Callculations
		currentHitPoints = stats.HitPoints;

		// Attack Stat
		attack = stats.Attack + weaponBehaviour.Weapon.weaponStats.Attack;

		// Defense Stat
		defense = stats.Defense + weaponBehaviour.Weapon.weaponStats.Defense;

		// Speed Stat
		speed = stats.Speed + weaponBehaviour.Weapon.weaponStats.Speed;

		// Evasion Stat
		evasion = stats.Evasion + weaponBehaviour.Weapon.weaponStats.Evasion;

		// Accuracy Stat
		accuracy = stats.Accuracy + weaponBehaviour.Weapon.weaponStats.Accuracy;

		// Critical Hit Chance Stat
		critical = stats.Critical + weaponBehaviour.Weapon.weaponStats.Critical;
	}
	#endregion

	#region IDamageable Implementation
	public void Damage(int value)
	{
		anim.SetTrigger("Hit");
		currentHitPoints -= value;
		if(currentHitPoints <= 0)
			GameManager.Instance.GameOverEvent();

		UIManager.Instance.UpdatePlayerHealthUI(stats.HitPoints, currentHitPoints);
	}
	#endregion

	#region Private IEnumerators
	/// <summary>
	/// Everything that should happen upon collision with an enemy object.
	/// </summary>
	/// <param name="collision"></param>
	/// <returns></returns>
	private IEnumerator OnEnemyCollision(Collider2D collision)
	{
		movement.enabled = false;

		rb2d.AddForce((transform.position - collision.transform.position).normalized * 200f * Time.deltaTime, ForceMode2D.Impulse);
		Damage(collision.GetComponent<EnemyBehaviour>().EnemyStats.DamageOnHit);

		FloatingDamageNumberSpawner.Instance.InstantiateFloatingDamageNumberAtPos(transform.position, collision.GetComponent<EnemyBehaviour>().EnemyStats.DamageOnHit.ToString(), false);

		StartCoroutine(SpriteBlinkOnHit());

		yield return new WaitForSeconds(0.5f);

		movement.enabled = true;
	}

	/// <summary>
	/// A simple sprite blinking event when the player get's hit.
	/// </summary>
	/// <returns></returns>
	private IEnumerator SpriteBlinkOnHit()
	{
		int count = 0;
		while(count < 3)
		{
			spriteRenderer.enabled = false;
			yield return new WaitForSeconds(0.1f);
			spriteRenderer.enabled = true;
			yield return new WaitForSeconds(0.1f);
			count++;
		}
	}
	#endregion
}
