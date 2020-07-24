using UnityEngine;

[CreateAssetMenu(fileName = "New Player Stats", menuName = "ScriptableObjects/New Player Stats")]
public class PlayerStats : ScriptableObject
{
	#region Private Variables
	[SerializeField] private new string name = "";
	[SerializeField] private string description = "";
	[SerializeField] private int hitPoints = 10;
	#endregion

	#region Public Properties
	public string Name { get => name; set => name = value; }
	public string Description { get => description; set => description = value; }
	public int HitPoints { get => hitPoints; set => hitPoints = value; }
	#endregion
}
