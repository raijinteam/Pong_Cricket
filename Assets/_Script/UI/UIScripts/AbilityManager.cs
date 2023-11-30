using System.Collections;
using System.Collections.Generic;
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

		for (int i = 0; i < all_Abilities.Length; i++)
		{
			all_UnlockStatus[i] = all_Abilities[i].isAbilityUnlocked;
		}

		UIManager.Instance.ui_Abilities.InstantiateAllTheAbilities();
	}

	public void UpgradeOrUnlockThisAbility(int _index)
	{
		if (IsAbilityUnlocked(_index))
		{
			// this ability is already unlocked. Upgrade it
			all_CurrentLevel[_index] += 1;
		}
		else
		{
			all_UnlockStatus[_index] = true;
		}

		IncreaseThePriceAfterUnlockOrUpgrade();
	}

	public void IncreaseThePriceAfterUnlockOrUpgrade()
	{
		currentAbilityUpgradePrice += priceIncreaseAfterEveryUpgrade;
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
}
