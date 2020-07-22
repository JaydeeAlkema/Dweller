using UnityEngine;

public class GameManager : MonoBehaviour
{
	private static GameManager instance = default;

	[SerializeField] private GameObject playerInstance;

	public static GameManager Instance { get => instance; set => instance = value; }
	public GameObject PlayerInstance { get => playerInstance; set => playerInstance = value; }

	private void Awake()
	{
		if(!Instance || Instance != this)
			Instance = this;

		GetObjects();
	}

	private void GetObjects()
	{
		PlayerInstance = GameObject.FindGameObjectWithTag("Player");
	}
}
