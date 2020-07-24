using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// The UI manager handles everything related to the UI. Input, menu's, Player UI, etc.
/// </summary>
public class UIManager : MonoBehaviour
{
	private static UIManager instance = default;

	#region Private Variables
	[Header("Player UI Properties")]
	private PlayerBehaviour playerBehaviour = default;						// Reference to the player behaviour component.
	[SerializeField] private Transform playerHeartLayoutGroup = default;	// Reference to the player heart layout group
	[SerializeField] private GameObject playerHeartPrefab = default;		// Prefab of the heart UI element.
	[SerializeField] private Sprite playerHeartFullSprite = default;		// Full version of the player heart image.
	[SerializeField] private Sprite playerHeartEmptySprite = default;       // Empty version of the player heart image.
	[SerializeField] private List<GameObject> heartsOnUI = new List<GameObject>();	// List with all the heart objects in the scene.
	#endregion

	#region Public Properties
	public static UIManager Instance { get => instance; set => instance = value; }
	#endregion

	#region Monobehaviour Callbacks
	private void Awake()
	{
		if(!Instance || Instance != this)
			Instance = this;
	}

	private void Start()
	{
		playerBehaviour = GameManager.Instance.PlayerInstance.GetComponent<PlayerBehaviour>();

		InitializePlayerHealthUI();
	}
	#endregion

	#region Player UI
	/// <summary>
	/// Initializes the Player Health UI depending on the amount of hitpoints the player has.
	/// </summary>
	private void InitializePlayerHealthUI()
	{
		for(int i = 0; i < playerBehaviour.Stats.HitPoints; i++)
		{
			GameObject newHeart = Instantiate(playerHeartPrefab, playerHeartLayoutGroup);
			newHeart.GetComponent<Image>().sprite = playerHeartFullSprite;
			heartsOnUI.Add(newHeart);
		}
	}

	/// <summary>
	/// Updates the Player Health UI.
	/// </summary>
	/// <param name="startingHitPoints"></param>
	/// <param name="currentHitPoints"></param>
	public void UpdatePlayerHealthUI(int startingHitPoints, int currentHitPoints)
	{
		for(int i = 0; i < startingHitPoints; i++)
		{
			if(i > currentHitPoints - 1)
			{
				heartsOnUI[i].GetComponent<Image>().sprite = playerHeartEmptySprite;
			}
		}
	}
	#endregion
}
