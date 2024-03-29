using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerSelectionUI : MonoBehaviour
{
	private int currentSelectedPlayer = 0;

	[Header("Current Selected Player Stats")]
	[SerializeField] private TextMeshProUGUI txt_BattingPower;
	[SerializeField] private TextMeshProUGUI txt_BowlingPower;
	[SerializeField] private TextMeshProUGUI txt_SpinPower;
	[SerializeField] private TextMeshProUGUI txt_BattlingPowerDifferenceOnUpgrade;
	[SerializeField] private TextMeshProUGUI txt_BowlingPowerDifferenceOnUpgrade;
	[SerializeField] private TextMeshProUGUI txt_SpinPowerDifferenceOnUpgrade;
	[SerializeField] private GameObject btn_Upgrade;
	[SerializeField] private TextMeshProUGUI txt_UpgradePrice;

	[Header("Player Instantiation")]
	[SerializeField] private PlayerSelectionButtonUI player_Common;
	[SerializeField] private PlayerSelectionButtonUI player_Rare;
	[SerializeField] private PlayerSelectionButtonUI player_Epic;
	[SerializeField] private PlayerSelectionButtonUI[] all_PlayersWhichGotInstantiated;
	[SerializeField] private Transform parentToInstantiatePlayersOn;

    public void InstantiatePlayersInScreen()
	{
		int totalPlayerCount = CharacterManager.Instance.GetTotalPlayerCount();
		all_PlayersWhichGotInstantiated = new PlayerSelectionButtonUI[totalPlayerCount];

		for(int i = 0; i < totalPlayerCount; i++)
		{
			PlayerSelectionButtonUI currentPlayer;

			if(CharacterManager.Instance.GetPlayerRarity(i) == PlayerType.Common)
			{
				currentPlayer = Instantiate(player_Common, player_Common.transform.position, player_Common.transform.rotation, 
											parentToInstantiatePlayersOn);
			}
			else if (CharacterManager.Instance.GetPlayerRarity(i) == PlayerType.Rare)
			{
				currentPlayer = Instantiate(player_Rare, player_Rare.transform.position, player_Rare.transform.rotation,
											parentToInstantiatePlayersOn);
			}
			else
			{
				currentPlayer = Instantiate(player_Epic, player_Epic.transform.position, player_Epic.transform.rotation,
											parentToInstantiatePlayersOn);
			}

			all_PlayersWhichGotInstantiated[i] = currentPlayer;
		}
	}

	private void OnEnable()
	{
		currentSelectedPlayer = CharacterManager.Instance.currentSelectedCharacter;
		SetEveryPlayerData();
	}

	private void Start()
	{
		all_PlayersWhichGotInstantiated[currentSelectedPlayer].SelectThisCharacter();
	}

	private void SetEveryPlayerData()
	{
		for(int i = 0; i < all_PlayersWhichGotInstantiated.Length; i++)
		{
			bool isUnlocked = CharacterManager.Instance.IsCharacterUnlocked(i);
			int currentCardsValue = CharacterManager.Instance.GetCurrentCardsOfCharacter(i);
			int maxCards = 0;

			if (isUnlocked)
			{
				// Get cards required To Upgrade
				maxCards = CharacterManager.Instance.GetCardsRequiredToUpgrade(i);
			}
			else
			{
				// Get cards required To Unlock
				maxCards = CharacterManager.Instance.GetCardsRequiredToUnlock(i);
			}

			Sprite charIcon = CharacterManager.Instance.GetCharacterIcon(i);
			int currentCharLevel = CharacterManager.Instance.GetCharacterCurrentLevel(i);
			string charName = CharacterManager.Instance.GetCharacterName(i);

			all_PlayersWhichGotInstantiated[i].SetMyData(i, isUnlocked, currentCardsValue, maxCards, charIcon, currentCharLevel, charName);
		}
		
		SetStatsPanelForCurrentSelectedPlayer();
	}

	private void SetStatsPanelForCurrentSelectedPlayer()
	{
		float currentBattingPower = CharacterManager.Instance.GetCharacterBattingPowerForCurrentLevel(currentSelectedPlayer);
		string formattedValue = UtilityManager.Instance.FormatFloatToTrimDecimalPlacesIfTheyAreNotNeeded(currentBattingPower);
		txt_BattingPower.text = formattedValue;

		float currentBowlingPower = CharacterManager.Instance.GetCharacterBowlingPowerForCurrentLevel(currentSelectedPlayer);
		formattedValue = UtilityManager.Instance.FormatFloatToTrimDecimalPlacesIfTheyAreNotNeeded(currentBowlingPower);
		txt_BowlingPower.text = formattedValue;

		float currentSpinForce = CharacterManager.Instance.GetCharacterSpinForceForCurrentLevel(currentSelectedPlayer);
		formattedValue = UtilityManager.Instance.FormatFloatToTrimDecimalPlacesIfTheyAreNotNeeded(currentSpinForce);
		txt_SpinPower.text = formattedValue;

		// Reset all upgrade/ unlock values
		txt_BattlingPowerDifferenceOnUpgrade.gameObject.SetActive(false);
		txt_BowlingPowerDifferenceOnUpgrade.gameObject.SetActive(false);
		txt_SpinPowerDifferenceOnUpgrade.gameObject.SetActive(false);
		btn_Upgrade.SetActive(false);

		if (!CharacterManager.Instance.IsCharacterUnlocked(currentSelectedPlayer))
		{
			// player is no unlocked. 
			return;
		}
		else if (CharacterManager.Instance.IsCurrentCharacterAtMaxLevel(currentSelectedPlayer))
		{
			// player is already at max level
			return;
		}

		float battingPowerForNextLevel = CharacterManager.Instance.GetCharacterBattingPowerForNextLevel(currentSelectedPlayer);
		float difference = battingPowerForNextLevel - currentBattingPower;
		if (difference > 0)
		{
			// There is an upgrade to batting power, show it.
			formattedValue = UtilityManager.Instance.FormatFloatToTrimDecimalPlacesIfTheyAreNotNeeded(difference);
			txt_BattlingPowerDifferenceOnUpgrade.text = "+" + formattedValue;
			txt_BattlingPowerDifferenceOnUpgrade.gameObject.SetActive(true);
		}

		float bowlingPowerForNextLevel = CharacterManager.Instance.GetCharacterBowlingPowerForNextLevel(currentSelectedPlayer);
		difference = bowlingPowerForNextLevel - currentBowlingPower;
		if(difference > 0)
		{
			// There is an upgrade to bowling power, show it.
			formattedValue = UtilityManager.Instance.FormatFloatToTrimDecimalPlacesIfTheyAreNotNeeded(difference);
			txt_BowlingPowerDifferenceOnUpgrade.text = "+" + formattedValue;
			txt_BowlingPowerDifferenceOnUpgrade.gameObject.SetActive(true);
		}

		float spinForceForNextLevel = CharacterManager.Instance.GetCharacterSpinForceForNextLevel(currentSelectedPlayer);
		difference = spinForceForNextLevel - currentSpinForce;
		if (difference > 0)
		{
			// There is an upgrade to spin force, show it.
			formattedValue = UtilityManager.Instance.FormatFloatToTrimDecimalPlacesIfTheyAreNotNeeded(difference);
			txt_SpinPowerDifferenceOnUpgrade.text = "+" + formattedValue;
			txt_SpinPowerDifferenceOnUpgrade.gameObject.SetActive(true);
		}

		if (CharacterManager.Instance.DoesUserHaveEnoughPlayerCardsToUpgrade(currentSelectedPlayer))
		{
			// player has enough cards. Show Upgrade button with price text
			txt_UpgradePrice.text = CharacterManager.Instance.GetUpgradePriceForSelectedCharacter(currentSelectedPlayer).ToString();
			btn_Upgrade.SetActive(true);
		}
	}

	public void ClickedOnACharacter(int _index)
	{
		if (CharacterManager.Instance.IsCharacterUnlocked(_index)){

			// Character is unlocked, so deselect previous one and select this one.
			all_PlayersWhichGotInstantiated[currentSelectedPlayer].DeSelectThisCharacter();
			currentSelectedPlayer = _index;
			all_PlayersWhichGotInstantiated[currentSelectedPlayer].SelectThisCharacter();
			CharacterManager.Instance.SetSelctedPlayer(currentSelectedPlayer);
		}

		currentSelectedPlayer = _index;
		SetStatsPanelForCurrentSelectedPlayer();
	}

	private void UpdateUpgradedPlayerData()
	{
		int currentCardsValue = CharacterManager.Instance.GetCurrentCardsOfCharacter(currentSelectedPlayer);

		// Get cards required To Upgrade
		int maxCards = CharacterManager.Instance.GetCardsRequiredToUpgrade(currentSelectedPlayer);
		int currentCharLevel = CharacterManager.Instance.GetCharacterCurrentLevel(currentSelectedPlayer);
		all_PlayersWhichGotInstantiated[currentSelectedPlayer].UpdateMyData(currentCardsValue, maxCards, currentCharLevel);
		SetStatsPanelForCurrentSelectedPlayer();
	}

	public void OnClick_UpgradeCharacter()
	{
		if(DataManager.Instance.coins < CharacterManager.Instance.GetUpgradePriceForSelectedCharacter(currentSelectedPlayer))
		{
			// Not Enough coins to upgrade
			return;
		}

		DataManager.Instance.DecresedCoin(CharacterManager.Instance.GetUpgradePriceForSelectedCharacter(currentSelectedPlayer));
		CharacterManager.Instance.CharacterUpgradeComplete(currentSelectedPlayer);

		UpdateUpgradedPlayerData();
	}
}
