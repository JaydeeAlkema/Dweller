using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DungeonGeneratorSettings : MonoBehaviour
{
	#region Private Variables
	[Header("Private Variables")]
	private string seed = "";                          // Seed of the dungeon
	private int minPathwayCount = 15;                  // Minimum amount of pathways that will be generated.
	private int maxPathwayCount = 30;                  // Maximum amount of pathways that will be generated.
	private int minPathwayLength = 10;                 // Minimum length of the pathway before making a turn.
	private int maxPathwayLength = 20;                 // Maximum length of the pathway before making a turn.
	private int minRoomSize = 14;                      // Min Room size.
	private int maxRoomSize = 26;                      // Max Room size.

	[Header("UI Elements")]
	[SerializeField] private TMP_InputField seedInputField = default;   // Seed Inputfield Reference.
	[Space]

	[SerializeField] private Slider minPathwayCountSlider = default;    // Min Pathway Count Slider Reference.
	[SerializeField] private TextMeshProUGUI minPathwayCountText = default;
	[Space]

	[SerializeField] private Slider maxPathwayCountSlider = default;    // Max Pathway Count Slider Reference.
	[SerializeField] private TextMeshProUGUI maxPathwayCountText = default;
	[Space]

	[SerializeField] private Slider minPathwayLengthSlider = default;   // Min Pathway Length Slider Reference.
	[SerializeField] private TextMeshProUGUI minPathwayLengthText = default;
	[Space]

	[SerializeField] private Slider maxPathwayLengthSlider = default;   // Max Pathway Length Slider Reference.
	[SerializeField] private TextMeshProUGUI maxPathwayLengthText = default;
	[Space]

	[SerializeField] private Slider minRoomsizeSlider = default;        // Min Room Size Slider Reference.
	[SerializeField] private TextMeshProUGUI minRoomSizeText = default;
	[Space]

	[SerializeField] private Slider maxRoomsizeSlider = default;        // Max Room Size Slider Reference.
	[SerializeField] private TextMeshProUGUI maxRoomSizeText = default;

	#endregion

	#region Monobehaviour Callback
	private void Start()
	{
		seed = seedInputField.text;

		OnPathwayCountMinSliderValueChanged();
		OnPathwayCountMaxSliderValueChanged();

		OnPathwayLengthMinSliderValueChanged();
		OnPathwayLengthMaxSliderValueChanged();

		OnMinRoomSizeSliderValueChanged();
		OnMaxRoomSizeSliderValueChanged();
	}
	#endregion

	#region On Slider Value Changed
	public void OnPathwayCountMinSliderValueChanged()
	{
		minPathwayCount = (int)minPathwayCountSlider.value;
		minPathwayCountText.text = "Minimum: " + minPathwayCount;
	}
	public void OnPathwayCountMaxSliderValueChanged()
	{
		maxPathwayCount = (int)maxPathwayCountSlider.value;
		maxPathwayCountText.text = "Maximum: " + maxPathwayCount;
	}

	public void OnPathwayLengthMinSliderValueChanged()
	{
		minPathwayLength = (int)minPathwayLengthSlider.value;
		minPathwayLengthText.text = "Minimum: " + minPathwayLength;
	}
	public void OnPathwayLengthMaxSliderValueChanged()
	{
		maxPathwayLength = (int)maxPathwayLengthSlider.value;
		maxPathwayLengthText.text = "Maximum: " + maxPathwayLength;
	}

	public void OnMinRoomSizeSliderValueChanged()
	{
		minRoomSize = (int)minRoomsizeSlider.value;
		minRoomSizeText.text = "Minimum: " + minRoomSize;
	}
	public void OnMaxRoomSizeSliderValueChanged()
	{
		maxRoomSize = (int)maxRoomsizeSlider.value;
		maxRoomSizeText.text = "Maximum: " + maxRoomSize;
	}
	#endregion

	#region On Button Clicks
	public void PlayButton_OnClick()
	{
		PlayerPrefs.SetString("Seed", seedInputField.text);

		PlayerPrefs.SetInt("MinPathwayCount", minPathwayCount);
		PlayerPrefs.SetInt("MaxPathwayCount", maxPathwayCount);

		PlayerPrefs.SetInt("MinPathwayLength", minPathwayLength);
		PlayerPrefs.SetInt("MaxPathwayLength", maxPathwayLength);

		PlayerPrefs.SetInt("MinRoomSize", minRoomSize);
		PlayerPrefs.SetInt("MaxRoomSize", maxRoomSize);

		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
	}
	#endregion
}
