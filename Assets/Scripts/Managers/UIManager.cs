using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// The UI manager handles everything related to the UI. Input, menu's, Player UI, etc.
/// </summary>
public class UIManager : MonoBehaviour
{
	private static UIManager instance = default;

	#region Private Variables
	[Header("Dungeon UI Properties")]
	[SerializeField] private DungeonGenerator dungeonGenerator = default;   // Reference to the Dungeon Generator class.
	[SerializeField] private TextMeshProUGUI seedText = default;            // Reference to the seed text UI element.

	[Header("Player UI Properties")]
	[SerializeField] private Transform playerHeartLayoutGroup = default;    // Reference to the player heart layout group
	[SerializeField] private GameObject playerHeartPrefab = default;        // Prefab of the heart UI element.
	[SerializeField] private Sprite playerHeartFullSprite = default;        // Full version of the player heart image.
	[SerializeField] private Sprite playerHeartEmptySprite = default;       // Empty version of the player heart image.
	[SerializeField] private List<GameObject> heartsOnUI = new List<GameObject>();  // List with all the heart objects in the scene.

	[Header("Floating UI Panel")]
	[SerializeField] private Transform floatingPanelTransform = default;
	[SerializeField] private TextMeshProUGUI floatingInfoPanelTitle = default;
	[SerializeField] private TextMeshProUGUI floatingInfoPanelDescription = default;
	[SerializeField] private TextMeshProUGUI floatingInfoPanelAttackStat = default;
	[SerializeField] private TextMeshProUGUI floatingInfoPanelDefenseStat = default;
	[SerializeField] private TextMeshProUGUI floatingInfoPanelSpeedStat = default;
	[SerializeField] private TextMeshProUGUI floatingInfoPanelEvasionStat = default;
	[SerializeField] private TextMeshProUGUI floatingInfoPanelAccuracyStat = default;
	[SerializeField] private TextMeshProUGUI floatingInfoPanelCriticalStat = default;


	private Vector2 screenBounds = Vector2.zero;                            // Will use this later to clamp the info panel inside the screen.
	private PlayerBehaviour playerBehaviour = default;                      // Reference to the player behaviour component.
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
		screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
		playerBehaviour = GameManager.Instance.PlayerInstance.GetComponent<PlayerBehaviour>();

		seedText.text = "Seed: " + dungeonGenerator.Seed;

		InitializePlayerHealthUI();
		HideFloatingInfoPanel();
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

	#region Floating Info Panel
	/// <summary>
	/// Shows the Floating Info Panel and clamps it's position to the mouse position.
	/// This info panel shows all the necessary information of the item that's currently under the cursor.
	/// </summary>
	/// <param name="item"> What item to get the properties from to display. </param>
	public void ShowFloatingInfoPanel(Item item)
	{
		Cursor.visible = false;
		floatingPanelTransform.gameObject.SetActive(true);
		floatingPanelTransform.position = Input.mousePosition;

		floatingInfoPanelTitle.text = item.name;
		floatingInfoPanelDescription.text = item.description;
		floatingInfoPanelAttackStat.text = "- Attack: " + item.weaponStats.Attack.ToString();
		floatingInfoPanelDefenseStat.text = "- Defense: " + item.weaponStats.Defense.ToString();
		floatingInfoPanelSpeedStat.text = "- Speed: " + item.weaponStats.Speed.ToString();
		floatingInfoPanelEvasionStat.text = "- Evasion: " + item.weaponStats.Evasion.ToString();
		floatingInfoPanelAccuracyStat.text = "- Accuracy: " + item.weaponStats.Accuracy.ToString();
		floatingInfoPanelCriticalStat.text = "- Critical: " + item.weaponStats.Critical.ToString();
	}

	/// <summary>
	/// Hides the Floating Info Panel and resets it's position.
	/// </summary>
	public void HideFloatingInfoPanel()
	{
		Cursor.visible = true;
		floatingPanelTransform.gameObject.SetActive(false);
		floatingPanelTransform.position = Vector3.zero;
	}
	#endregion
}
