using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "ScriptableObjects/PlayerData")]
public class PlayerDataScriptableObject : ScriptableObject
{
	[Header("Player Stats")]
	public float[] all_BattingPower;
	public float[] all_BowlingPower;
	public float[] all_SpinForce;

	[Header("Player Information")]
	public string str_PlayerName;
	public PlayerType playerRarity;
	public Sprite sprite_PlayerIcon;
	public bool isUnlocked;
	//public int unlockPrice;
	public int cardsRequiredToUnlock;
	public int[] all_UpgradePrice;
	public int[] all_RequiredCardsToUpgrade;
	public int chestIndex; // from this chest index onwards, player can be unlocked;
}

public enum PlayerType
{
	Common,
	Rare,
	Epic
}
