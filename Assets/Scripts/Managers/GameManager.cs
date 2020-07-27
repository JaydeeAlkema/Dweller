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

		GetComponentsAndObjects();
	}
	#endregion

	#region Private Voids
	/// <summary>
	/// Get's all the required components and objects from the scene. This is a temp function.
	/// The GameManager will handle the scene loading later on in the project.
	/// </summary>
	private void GetComponentsAndObjects()
	{
		PlayerInstance = GameObject.FindGameObjectWithTag("Player");
	}
	#endregion

	#region Public Voids
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
