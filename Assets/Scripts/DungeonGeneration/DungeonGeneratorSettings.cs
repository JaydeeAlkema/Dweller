using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DungeonGeneratorSettings : MonoBehaviour
{
	#region Private Variables
	private static DungeonGeneratorSettings instance = null;

	[Header("Private Variables")]
	[SerializeField] private string seed = "";                          // Seed of the dungeon
	[SerializeField] private int minPathwayCount = 15;                  // Minimum amount of pathways that will be generated.
	[SerializeField] private int maxPathwayCount = 30;                  // Maximum amount of pathways that will be generated.
	[SerializeField] private int minPathwayLength = 10;                 // Minimum length of the pathway before making a turn.
	[SerializeField] private int maxPathwayLength = 20;                 // Maximum length of the pathway before making a turn.
	[SerializeField] private int minRoomSize = 14;                      // Min Room size.
	[SerializeField] private int maxRoomSize = 26;                      // Max Room size.

	[Header("UI Elements")]
	[SerializeField] private TMP_InputField seedInputField = default;   // Seed Inputfield Reference.
	[SerializeField] private Slider minPathwayCountSlider = default;    // Min Pathway Count Slider Reference.
	[SerializeField] private Slider maxPathwayCountSlider = default;    // Max Pathway Count Slider Reference.
	[SerializeField] private Slider minPathwayLengthSlider = default;   // Min Pathway Length Slider Reference.
	[SerializeField] private Slider maxPathwayLengthSlider = default;   // Max Pathway Length Slider Reference.
	[SerializeField] private Slider minRoomsizeSlider = default;        // Min Room Size Slider Reference.
	[SerializeField] private Slider maxRoomsizeSlider = default;        // Max Room Size Slider Reference.
	#endregion

	#region Public Properties
	public string Seed { get => seed; set => seed = value; }
	public int MinPathwayCount { get => minPathwayCount; set => minPathwayCount = value; }
	public int MaxPathwayCount { get => maxPathwayCount; set => maxPathwayCount = value; }
	public int MinPathwayLength { get => minPathwayLength; set => minPathwayLength = value; }
	public int MaxPathwayLength { get => maxPathwayLength; set => maxPathwayLength = value; }
	public int MinRoomSize { get => minRoomSize; set => minRoomSize = value; }
	public int MaxRoomSize { get => maxRoomSize; set => maxRoomSize = value; }
	#endregion

	#region Monobehaviour Callback
	private void Awake()
	{
		if(!instance || instance != this)
			instance = this;
	}

	private void Start()
	{
		seed = seedInputField.text;
		minPathwayCount = (int)minPathwayCountSlider.value;
		maxPathwayCount = (int)maxPathwayCountSlider.value;
		minPathwayLength = (int)minPathwayLengthSlider.value;
		maxPathwayLength = (int)maxPathwayLengthSlider.value;
	}
	#endregion

	#region On Slider Value Changed
	public void OnPathwayCountMinSliderValueChanged()
	{
		minPathwayCount = (int)minPathwayCountSlider.value;
	}
	public void OnPathwayCountMaxSliderValueChanged()
	{
		maxPathwayCount = (int)maxPathwayCountSlider.value;
	}

	public void OnPathwayLengthMinSliderValueChanged()
	{
		minPathwayLength = (int)minPathwayLengthSlider.value;
	}
	public void OnPathwayLengthMaxSliderValueChanged()
	{
		maxPathwayLength = (int)maxPathwayLengthSlider.value;
	}

	public void OnMinRoomSizeSliderValueChanged()
	{
		minRoomSize = (int)minRoomsizeSlider.value;
	}
	public void OnMaxRoomSizeSliderValueChanged()
	{
		maxRoomSize = (int)maxRoomsizeSlider.value;
	}
	#endregion

	#region On Button Clicks
	public void PlayButton_OnClick()
	{
		PlayerPrefs.SetString("Seed", Seed);

		PlayerPrefs.SetInt("MinPathwayCount", MinPathwayCount);
		PlayerPrefs.SetInt("MaxPathwayCount", MaxPathwayCount);

		PlayerPrefs.SetInt("MinPathwayLength", MinPathwayLength);
		PlayerPrefs.SetInt("MaxPathwayLength", MaxPathwayLength);

		PlayerPrefs.SetInt("MinRoomSize", MinRoomSize);
		PlayerPrefs.SetInt("MaxRoomSize", MaxRoomSize);

		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
	}
	#endregion
}
