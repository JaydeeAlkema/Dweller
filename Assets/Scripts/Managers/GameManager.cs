using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState
{
	Playing,
	Paused,
	GameOver
}

public class GameManager : MonoBehaviour
{
	private static GameManager instance = default;

	#region Private Variables
	[SerializeField] private GameState gameState = GameState.Playing;

	[Header("Managers and Spawners")]
	[SerializeField] private UIManager uiManager = null;

	[Header("Player Properties")]
	[SerializeField] private GameObject playerPrefab;
	[SerializeField] private GameObject playerInstance;
	#endregion

	#region Public Properties
	public static GameManager Instance { get => instance; set => instance = value; }
	public GameState GameState { get => gameState; set => gameState = value; }
	public GameObject PlayerInstance { get => playerInstance; set => playerInstance = value; }
	#endregion

	#region Monobehaviour Callbacks
	private void Awake()
	{
		if(!Instance || Instance != this)
			Instance = this;

		uiManager.enabled = false;
	}
	#endregion

	#region Public Voids
	public void SpawnPlayer(Vector3 position)
	{
		if(PlayerInstance)
			Destroy(playerInstance);

		playerInstance = Instantiate(playerPrefab, position, Quaternion.identity);
		PlayerInstance.GetComponent<Rigidbody2D>().position = position;

		uiManager.enabled = true;
	}

	/// <summary>
	/// Everything that should be done when GameOver.
	/// </summary>
	public void GameOverEvent()
	{
		gameState = GameState.GameOver;
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}
	#endregion
}
