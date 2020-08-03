using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// This class handles the floating damage numbers that should be displayed when an entitiy is hit.
/// </summary>
public class FloatingDamageNumberSpawner : MonoBehaviour
{
	#region Singleton
	private static FloatingDamageNumberSpawner instance = null;

	private void Awake()
	{
		if(!Instance || Instance != this)
			Instance = this;
	}
	#endregion

	#region Private variables
	[Header("Base Properties")]
	[SerializeField] private GameObject prefabObj = default;            // Reference to the prefab object that holds all the components needed.
	[SerializeField] private float impulseForce = 1f;                   // How much force is applied to the rigidbody when the number is activated.
	[SerializeField] private float destroyTimer = 1f;                   // How long the number will stay alive for. a.k.a. how long until it's destroyed.
	#endregion

	#region Public Properties
	public static FloatingDamageNumberSpawner Instance { get => instance; set => instance = value; }
	#endregion

	#region Public Voids
	/// <summary>
	/// Instantiate a floating number object at the given location with the given string.
	/// This object will get a single impulse force. After some time the object will be destroyed.
	/// </summary>
	/// <param name="pos"> The postion where the damage number spawns from. </param>
	/// <param name="text"> What text to display. </param>
	/// <param name="isCritical"> If the damage number should be displayed as a critical hit. </param>
	public void InstantiateFloatingDamageNumberAtPos(Vector2 pos, string text, bool isCritical)
	{
		GameObject newFloatingNumberGO = Instantiate(prefabObj, pos, Quaternion.identity);
		Rigidbody2D rb2d = newFloatingNumberGO.GetComponent<Rigidbody2D>();
		TextMeshPro textMesh = newFloatingNumberGO.GetComponent<TextMeshPro>();

		rb2d.AddForce(newFloatingNumberGO.transform.up * impulseForce, ForceMode2D.Impulse);
		textMesh.text = text;

		if(isCritical)
		{
			textMesh.fontSize = 4f;
			textMesh.color = Color.red;
		}

		Destroy(newFloatingNumberGO, destroyTimer);
	}
	#endregion
}
