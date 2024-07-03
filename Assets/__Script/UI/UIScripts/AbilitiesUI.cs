using JetBrains.Annotations;
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
	[SerializeField] private GameObject obj_Scroll;
	[SerializeField] private SelctedAbiltyAnimation selctedAbiltyAnimation;
	[SerializeField] private Panel_SelctedSummry panel_SelctedSummry;


	

	
	

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
        ActivetIconPanel(randomUpgradeIndex);
       


    }

   

    public void OnClick_Upgrade()
	{
		if (DataManager.Instance.coins < AbilityManager.Instance.GetCurrentPriceToUnlockAnAbility())
		{
			UIManager.Instance.spawnPopup("No Enough Coins");
			// NOT ENOUGH COINS
			return;
		}

		AudioManager.insatance.PlayBtnClickSFX();
		DailyTaskManager.upgradePowerup?.Invoke();
		DataManager.Instance.DecresedCoin(AbilityManager.Instance.GetCurrentPriceToUnlockAnAbility());
		UpgradeProcedure();
	}

    private void ActivetIconPanel(int randomUpgradeIndex) {
        UIManager.Instance.ui_MenuSelectionScreen.gameObject.SetActive(false);
        UIManager.Instance.panel_CommanMenu.gameObject.SetActive(false);
        obj_Scroll.gameObject.SetActive(false);
        panel_UpgradeButton.gameObject.SetActive(false);
        selctedAbiltyAnimation.ActivtedAnimation(randomUpgradeIndex);

    }

    public void CompltedSelectedPanel(int selctedSprite) {

		selctedAbiltyAnimation.gameObject.SetActive(false);
		
		panel_SelctedSummry.ActivtedSummry(selctedSprite);
      
    }
	public void CompltedSelctedSummry() {
        SetUpgradeButtonInfo();
        SetAbilityInfoPanels();
        panel_SelctedSummry.gameObject.SetActive(false);
        UIManager.Instance.ui_MenuSelectionScreen.gameObject.SetActive(true);
        UIManager.Instance.panel_CommanMenu.gameObject.SetActive(true);
        obj_Scroll.gameObject.SetActive(true);
    }
}
