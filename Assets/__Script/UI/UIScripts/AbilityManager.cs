using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class AbilityManager : MonoBehaviour
{
    public static AbilityManager Instance;

	private void Awake()
	{
		Instance = this;
	}

	[SerializeField] private AbilitiesDataScriptableObject[] all_Abilities;
	[SerializeField] private bool[] all_UnlockStatus;
	[SerializeField] private int[] all_CurrentLevel;

	private int maxAbilityLevel = 9;
	private int currentAbilityUpgradePrice = 500;
	private int priceIncreaseAfterEveryUpgrade = 300; 

	// TEMP //
	private void Start()
	{
        all_UnlockStatus = new bool[all_Abilities.Length];
        //all_CurrentLevel = new int[all_Abilities.Length];

        for (int i = 0; i < all_Abilities.Length; i++) {
            all_UnlockStatus[i] = all_Abilities[i].isAbilityUnlocked;
			all_CurrentLevel[i] = 0;
        }
        SetData();
        UIManager.Instance.ui_Abilities.InstantiateAllTheAbilities();
        
       


    }

    private void SetData() {
		if (PlayerPrefs.HasKey(AbiltyData.key_CurrentPrice)) {
			// Load Data from PlayerPrefs
			for (int i = 0; i < all_Abilities.Length; i++) {

				all_CurrentLevel[i] = PlayerPrefs.GetInt(AbiltyData.key_Level + i);
				all_UnlockStatus[i] = PlayerPrefs.GetInt(AbiltyData.key_Unlocked + i) == 1;
			}
			currentAbilityUpgradePrice = PlayerPrefs.GetInt(AbiltyData.key_CurrentPrice);

		}
		else {

			// Save Data In Player Prefs
			for (int i = 0; i < all_Abilities.Length; i++) {

				PlayerPrefs.SetInt(AbiltyData.key_Unlocked + i, all_UnlockStatus[i] ? 1 : 0);
				PlayerPrefs.SetInt(AbiltyData.key_Level + i, all_CurrentLevel[i]);
			}
			PlayerPrefs.SetInt(AbiltyData.key_CurrentPrice, currentAbilityUpgradePrice);
		}
    }

    public void UpgradeOrUnlockThisAbility(int _index)
	{
		if (IsAbilityUnlocked(_index))
		{
			// this ability is already unlocked. Upgrade it
			all_CurrentLevel[_index] += 1;
            PlayerPrefs.SetInt(AbiltyData.key_Level + _index, all_CurrentLevel[_index]);
        }
		else
		{
			all_UnlockStatus[_index] = true;
			PlayerPrefs.SetInt(AbiltyData.key_Unlocked + _index, all_UnlockStatus[_index] ? 1 : 0);
		}

		IncreaseThePriceAfterUnlockOrUpgrade();
	}

	public void IncreaseThePriceAfterUnlockOrUpgrade()
	{
		currentAbilityUpgradePrice += priceIncreaseAfterEveryUpgrade;
        PlayerPrefs.SetInt(AbiltyData.key_CurrentPrice , currentAbilityUpgradePrice);
    }
    public int GetTotalAbilityCount()
    {
		return all_Abilities.Length;
	}

	public bool IsAbilityUnlocked(int _index)
	{
		return all_UnlockStatus[_index];
	}

	public Sprite GetAbilityIcon(int _index)
	{
		return all_Abilities[_index].sprite_AbilityIcon;
	}

	public string GetAbilityName(int _index)
	{
		return all_Abilities[_index].str_AbilityName;
	}

	public int GetAbilityCurrentLevel(int _index)
	{
		return all_CurrentLevel[_index];
	}

	public string GetAbilityDescription(int _index)
	{
		return all_Abilities[_index].str_Description;
	}

	public int GetCurrentPriceToUnlockAnAbility()
	{
		return currentAbilityUpgradePrice;
	}

	public bool CanAddAbilityToUpgradeList(int _index)
	{
		if(all_CurrentLevel[_index] == maxAbilityLevel)
		{
			return false; // ability is already at max level, cannot upgrade more
		}

		return true;
	}

	public bool IsUpgradeOrUnlockStillPossible()
	{
		for(int i = 0; i < all_CurrentLevel.Length; i++)
		{
			if(all_CurrentLevel[i] != maxAbilityLevel)
			{
				return true; // still there are abilities who have not reached max level so it is possible
			}
		}

		return false;
	}

	public AbilitiesDataScriptableObject GetAbliltyData(AbilityType myType) {
        int index = 0;
        for (int i = 0; i < all_Abilities.Length; i++) {
            if (all_Abilities[i].myAbiltyType == myType) {

                index = i;
                break;
            }
        }

        return all_Abilities[index];
    }

    public int GetAbilityCurrentLevelWithType(AbilityType myType) {

		int index = 0;
		for (int i = 0; i < all_Abilities.Length; i++) {
			if (all_Abilities[i].myAbiltyType == myType) {

				index =  all_CurrentLevel[i];
				break;
			}
		}

		return index;
        
    }

    public AbilitiesDataScriptableObject GetAbliltyData(int index) {
        

        return all_Abilities[index];
    }

    public bool IsAbilityUnlockedWithType(AbilityType myType) {

		int Index = 0;
		for (int i = 0; i < all_Abilities.Length; i++) {

			if (all_Abilities[i].myAbiltyType == myType) {

				Index = i;
				break;

			}
		}

		return all_UnlockStatus[Index];
	}
}
