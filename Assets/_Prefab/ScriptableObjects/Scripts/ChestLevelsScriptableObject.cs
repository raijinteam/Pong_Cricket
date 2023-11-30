using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ChestLevels", menuName = "ScriptableObjects/ChestLevels")]
public class ChestLevelsScriptableObject : ScriptableObject
{
	public ChestType rarity;
	public string str_ChestName;
	public Sprite str_ChestIcon;
	public int[] all_MinimumCoinRewardBasedOnLevel;
	public int[] all_MaximumCoinRewardBasedOnLevel;
	public int[] all_MinimumGemRewardBasedOnLevel;
	public int[] all_MaximumGemRewardBasedOnLevel;
	public int maxNumberOfCommonCharactersToReward; // number of different characters to reward
	public int[] all_NormalCardRewardRange;
	public int maxNumberOfRareCharactersToReward; // number of different characters to reward
	public int[] all_RareCardRewardRange;
	public int maxNumberOfEpicCharactersToReward; // number of different characters to reward
	public int[] all_EpicCardRewardRange;

	[Header("Unlock Config")]
	public int rewardHours = 6;
	public int rewardMinutes = 0;
	public int rewardSeconds = 0;

	[Header("Cards Probability")]
	public float flt_RareCardRewardProbability;
	public float flt_EpicCardRewardProbability;
}

public enum ChestType
{
	Starter,
	Common,
	Rare,
	Epic,
	Legendary
}
