using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
	public static CharacterManager Instance;

	private void Awake()
	{
		Instance = this;
	}

	[field: SerializeField]public int currentSelectedCharacter { get;  set; }
	[SerializeField] private int maxCharacterLevel;
	[SerializeField] private int[] all_CurrentCardsValue;
	[SerializeField] private int[] all_CurrentLevelOfCharacters;
	[SerializeField] private bool[] all_UnlockStatus;
	[SerializeField] private PlayerDataScriptableObject[] all_PlayerData;

	// TEMPORARY;
	private void Start()
	{
		for(int i = 0; i < all_PlayerData.Length; i++)
		{
			all_UnlockStatus[i] = all_PlayerData[i].isUnlocked;
		}

		UIManager.Instance.ui_PlayerSelection.InstantiatePlayersInScreen();
	}

	public void CharacterUpgradeComplete(int _index)
	{
		all_CurrentCardsValue[_index] -= GetCardsRequiredToUpgrade(_index);
		all_CurrentLevelOfCharacters[_index] = all_CurrentLevelOfCharacters[_index] + 1;
	}

	public int GetTotalPlayerCount()
	{
		return all_PlayerData.Length;
	}

	public PlayerType GetPlayerRarity(int _index)
	{
		return all_PlayerData[_index].playerRarity;
	}

	public bool IsCharacterUnlocked(int _index)
	{
		return all_UnlockStatus[_index];
	}

	public int GetCurrentCardsOfCharacter(int _index)
	{
		return all_CurrentCardsValue[_index];
	}

	public int GetCardsRequiredToUpgrade(int _index)
	{
		return all_PlayerData[_index].all_RequiredCardsToUpgrade[all_CurrentLevelOfCharacters[_index]];
	}

	public int GetCardsRequiredToUnlock(int _index)
	{
		return all_PlayerData[_index].cardsRequiredToUnlock;
	}

	public Sprite GetCharacterIcon(int _index)
	{
		return all_PlayerData[_index].sprite_PlayerIcon;
	}

	public int GetCharacterCurrentLevel(int _index)
	{
		return all_CurrentLevelOfCharacters[_index];
	}

	public string GetCharacterName(int _index)
	{
		return all_PlayerData[_index].str_PlayerName;
	}

	public float GetCharacterBattingPowerForCurrentLevel(int _index)
	{
		return all_PlayerData[_index].all_BattingPower[all_CurrentLevelOfCharacters[_index]];
	}

	public float GetCharacterBattingPowerForNextLevel(int _index)
	{
		return all_PlayerData[_index].all_BattingPower[all_CurrentLevelOfCharacters[_index] + 1];
	}

	public float GetCharacterBowlingPowerForCurrentLevel(int _index)
	{
		return all_PlayerData[_index].all_BowlingPower[all_CurrentLevelOfCharacters[_index]];
	}

	public float GetCharacterBowlingPowerForNextLevel(int _index)
	{
		return all_PlayerData[_index].all_BowlingPower[all_CurrentLevelOfCharacters[_index] + 1];
	}

	public float GetCharacterSpinForceForCurrentLevel(int _index)
	{
		return all_PlayerData[_index].all_SpinForce[all_CurrentLevelOfCharacters[_index]];
	}

	public float GetCharacterSpinForceForNextLevel(int _index)
	{
		return all_PlayerData[_index].all_SpinForce[all_CurrentLevelOfCharacters[_index] + 1];
	}

	public bool IsCurrentCharacterAtMaxLevel(int _index)
	{
		if(all_CurrentLevelOfCharacters[_index] >= maxCharacterLevel)
		{
			return true; // is at max level
		}

		return false; // can increase level
	}

	public bool DoesUserHaveEnoughPlayerCardsToUpgrade(int _index)
	{
		if(all_CurrentCardsValue[_index] >= GetCardsRequiredToUpgrade(_index))
		{
			// has enough cards for upgrading the character
			return true;
		}

		return false;
	}

	public int GetUpgradePriceForSelectedCharacter(int _index)
	{
		return all_PlayerData[_index].all_UpgradePrice[all_CurrentLevelOfCharacters[_index]];
	}
}
