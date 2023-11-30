using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AbilitiesUI : MonoBehaviour
{
    [SerializeField] private AbilityInfoUI info_Ability;
    [SerializeField] private Transform abilityInfoUIParent;
    [SerializeField] private AbilityInfoUI[] all_AbilitiesWhichGotInstantiated;
	[SerializeField] private GameObject panel_UpgradeButton;
	[SerializeField] private TextMeshProUGUI txt_UpgradePrice;

	private void OnEnable()
	{
		DisableAllInfoPanel();
	}

	private void Start()
	{
		SetUpgradeButtonInfo();
	}

	public void InstantiateAllTheAbilities()
	{
		all_AbilitiesWhichGotInstantiated = new AbilityInfoUI[AbilityManager.Instance.GetTotalAbilityCount()];

		for (int i = 0; i < all_AbilitiesWhichGotInstantiated.Length; i++)
		{
			all_AbilitiesWhichGotInstantiated[i] = Instantiate(info_Ability, info_Ability.transform.position, info_Ability.transform.rotation, abilityInfoUIParent);
		}

		SetAbilityInfoPanels();
	}

	private void SetAbilityInfoPanels()
	{
		for(int i = 0; i < all_AbilitiesWhichGotInstantiated.Length; i++)
		{
			bool isUnlocked = AbilityManager.Instance.IsAbilityUnlocked(i);
			Sprite abilityIcon = AbilityManager.Instance.GetAbilityIcon(i);
			string abilityName = AbilityManager.Instance.GetAbilityName(i);
			int abilityLevel = AbilityManager.Instance.GetAbilityCurrentLevel(i);
			string description = AbilityManager.Instance.GetAbilityDescription(i);

			all_AbilitiesWhichGotInstantiated[i].SetMyPanel(i, isUnlocked, abilityIcon, abilityName, abilityLevel, description);
		}	
	}

	public void DisableAllInfoPanel()
	{
		// DISABLE OTHER INFO PANEL IF THEY ARE ACTIVE
		for(int i = 0; i < all_AbilitiesWhichGotInstantiated.Length; i++)
		{
			all_AbilitiesWhichGotInstantiated[i].TurnOffDescriptionPanel();
		}
	}

	private void SetUpgradeButtonInfo()
	{
		if (AbilityManager.Instance.IsUpgradeOrUnlockStillPossible())
		{
			Debug.Log("here 1");
			panel_UpgradeButton.SetActive(true);
			txt_UpgradePrice.text = AbilityManager.Instance.GetCurrentPriceToUnlockAnAbility().ToString();
		}
		else
		{
			Debug.Log("here 2");
			panel_UpgradeButton.SetActive(false);
		}
	}

	//public List<int> list_AbilitiesIndexesWhichWeCanUpgrade = new List<int>();

	private void UpgradeProcedure()
	{
		List<int> list_AbilitiesIndexesWhichWeCanUpgrade = new List<int>();

		// Get all the abilities we can upgrade first    
		for (int i = 0; i < all_AbilitiesWhichGotInstantiated.Length; i++)
		{
			if (AbilityManager.Instance.CanAddAbilityToUpgradeList(i))
			{
				list_AbilitiesIndexesWhichWeCanUpgrade.Add(i);
			}
		}

		// Got the list now randomize the index
		int randomUpgradeIndex = Random.Range(0, list_AbilitiesIndexesWhichWeCanUpgrade.Count);
		Debug.Log("random Upgrade index : " + randomUpgradeIndex);
		AbilityManager.Instance.UpgradeOrUnlockThisAbility(list_AbilitiesIndexesWhichWeCanUpgrade[randomUpgradeIndex]);
		SetUpgradeButtonInfo();
		all_AbilitiesWhichGotInstantiated[list_AbilitiesIndexesWhichWeCanUpgrade[randomUpgradeIndex]].UpdateMyPanel();
	}

	public void OnClick_Upgrade()
	{
		if (DataManager.Instance.coins < AbilityManager.Instance.GetCurrentPriceToUnlockAnAbility())
		{
			// NOT ENOUGH COINS
			return;
		}

		DataManager.Instance.coins -= AbilityManager.Instance.GetCurrentPriceToUnlockAnAbility();
		UpgradeProcedure();
	}
}
