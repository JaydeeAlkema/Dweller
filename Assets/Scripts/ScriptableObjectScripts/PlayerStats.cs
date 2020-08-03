using UnityEngine;

[CreateAssetMenu(fileName = "New Player Stats", menuName = "ScriptableObjects/New Player Stats")]
public class PlayerStats : ScriptableObject
{
	#region Private Variables
	[Header("Base Properties")]
	[SerializeField] private new string name = "";
	[SerializeField] private string description = "";
	[SerializeField] private int hitPoints = 10;

	[Header("Player Stats")]
	// source: https://tvtropes.org/pmwiki/pmwiki.php/Main/TheSixStats
	// 12-15 - Above Average. Characters whose stats fall into this stat-tier are better than most "normals", meaning they're better than, say, 50% of everyone else out there.
	// 16-19 - Genius-level.Characters whose stats fall into this stat-tier are considered geniuses, savants, "gifted," what have you.These are your Olympic-level athletes, chess masters, etc.
	// 20-23 - Superhuman.Characters who stats fall into this stat-tier are pushing the boundaries of "realistic" to the extreme.Expect movie physics to be evoked for physical feats, while characters on the more intellectual end fall into Magnificent Bastard and Manipulative Bastard territories.
	// 24+ - These characters are just too uber to be real.These are characters who are the paragons of their stats, and who first come to mind when mentioning a stat. Of course, statted characters from e.g.roleplaying textbooks easily go up to 40.
	// 40+ - cosmic-scale or Eldritch Abomination monsters, demon overlords, The Archmage, and so forth: these kind of epic characters will usually have a 40 strength, charisma, or intelligence respectively.
	[SerializeField]
	[Tooltip("This stat determines how much damage certain actions do. Weapon based attacks use the attack stat.")]
	private int attack = 10;

	[SerializeField]
	[Tooltip("This stat reduces the amount of damage taken from opposing attacks.")]
	private int defense = 10;

	[SerializeField]
	[Tooltip("This stat determines literally how fast the player walks.")]
	private int speed = 10;

	[SerializeField]
	[Tooltip("This reduces the odds that an oncoming attack will hit.")]
	private int evasion = 10;

	[SerializeField]
	[Tooltip("This increases the odds the user’s attack will hit. If this is lower than the Defense of the Target, it has a chance to miss the attack.")]
	private int accuracy = 10;

	[SerializeField]
	[Tooltip("This improves the odds of dealing extra damage when using certain attacks. This is a percentage amount.")]
	private int critical = 10;
	#endregion

	#region Public Properties
	public string Name { get => name; set => name = value; }
	public string Description { get => description; set => description = value; }
	public int HitPoints { get => hitPoints; set => hitPoints = value; }

	public int Attack { get => attack; }
	public int Defense { get => defense; }
	public int Speed { get => speed; }
	public int Evasion { get => evasion; }
	public int Accuracy { get => accuracy; }
	public int Critical { get => critical; }
	#endregion
}
