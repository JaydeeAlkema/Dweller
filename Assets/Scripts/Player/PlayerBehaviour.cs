using System.Collections;
using UnityEngine;

/// <summary>
/// The Player Behaviour class is responsible for all the behaviour of the Player EXCEPT movement.
/// </summary>
public class PlayerBehaviour : MonoBehaviour, IDamageable
{
	[SerializeField] private Rigidbody2D rb2d = default;
	[SerializeField] private Animator anim = default;
	[SerializeField]
	private PlayerMovement movement = default;

	public void Damage(int value)
	{

	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if(collision.CompareTag("Enemy"))
		{
			StartCoroutine(OnEnemyCollision(collision));
		}
	}

	private IEnumerator OnEnemyCollision(Collider2D collision)
	{
		movement.enabled = false;
		rb2d.AddForce((transform.position - collision.transform.position).normalized * 200f * Time.deltaTime, ForceMode2D.Impulse);
		yield return new WaitForSeconds(0.5f);
		movement.enabled = true;
	}
}
