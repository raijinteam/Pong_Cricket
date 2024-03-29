
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CharacterManager : MonoBehaviour
{
	public static CharacterManager Instance;

	private void Awake()
	{
		Instance = this;
	}

	[field: SerializeField]public int currentSelectedCharacter { get; private set; }
	[SerializeField] private int maxCharacterLevel;
	[SerializeField] private int[] all_CurrentCardsValue;
	[SerializeField] private int[] all_CurrentLevelOfCharacters;
	[SerializeField] private bool[] all_UnlockStatus;
	[SerializeField] private PlayerDataScriptableObject[] all_PlayerData;

    //// TEMPORARY;
    //private void Start()
    //{
    //	for(int i = 0; i < all_PlayerData.Length; i++)
    //	{
    //		all_UnlockStatus[i] = all_PlayerData[i].isUnlocked;
    //	}

    //	UIManager.Instance.ui_PlayerSelection.InstantiatePlayersInScreen();
    //}


    private void Start() {
       
		SetData();
        
    }

    private void SetData() {
		if (PlayerPrefs.HasKey(CharactersKeys.key_CharacterLevel + 0)) {
			LoadDataFromPlayerPrefs();
		}
		else {
			SaveDataInPlayerPrefs();
		}

        UIManager.Instance.ui_PlayerSelection.InstantiatePlayersInScreen();
    }

    private void SaveDataInPlayerPrefs() {
		for (int i = 0; i < all_UnlockStatus.Length; i++) {

			if (all_UnlockStatus[i]) {
				PlayerPrefs.SetInt(CharactersKeys.key_CharacterUnlocked + i, 1);
			}
			else {
                PlayerPrefs.SetInt(CharactersKeys.key_CharacterUnlocked + i, 0);
            }

			
			PlayerPrefs.SetInt(CharactersKeys.key_CharacterAmmountCard + i, all_CurrentCardsValue[i]);
			PlayerPrefs.SetInt(CharactersKeys.key_CharacterLevel + i, all_CurrentLevelOfCharacters[i]);
			
		}
		PlayerPrefs.SetInt(CharactersKeys.key_CharacterSelected, currentSelectedCharacter);
    }

    private void LoadDataFromPlayerPrefs() {

		for (int i = 0; i < all_UnlockStatus.Length; i++) {
			all_UnlockStatus[i] = (PlayerPrefs.GetInt(CharactersKeys.key_CharacterUnlocked + i) == 1);
			all_CurrentLevelOfCharacters[i] = PlayerPrefs.GetInt(CharactersKeys.key_CharacterLevel + i);
			all_CurrentCardsValue[i] = PlayerPrefs.GetInt(CharactersKeys.key_CharacterAmmountCard + i);
		}

		currentSelectedCharacter = PlayerPrefs.GetInt(CharactersKeys.key_CharacterSelected);
    }

    public void CharacterUpgradeComplete(int _index)
	{
		all_CurrentCardsValue[_index] -= GetCardsRequiredToUpgrade(_index);
		PlayerPrefs.SetInt(CharactersKeys.key_CharacterAmmountCard + _index, all_CurrentCardsValue[_index]);
		all_CurrentLevelOfCharacters[_index] = all_CurrentLevelOfCharacters[_index] + 1;
		PlayerPrefs.SetInt(CharactersKeys.key_CharacterLevel + _index, all_CurrentLevelOfCharacters[_index]);
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
		Debug.Log("Upgrade Value" + GetCardsRequiredToUpgrade(_index));
		Debug.Log("Current" + all_CurrentCardsValue[_index]);
		if(all_CurrentCardsValue[_index] >= GetCardsRequiredToUpgrade(_index))
		{
			Debug.Log("True");
			// has enough cards for upgrading the character
			return true;
		}

		return false;
	}

	public int GetUpgradePriceForSelectedCharacter(int _index)
	{
		return all_PlayerData[_index].all_UpgradePrice[all_CurrentLevelOfCharacters[_index]];
	}

   

    public void SetSelctedPlayer(int currentSelectedPlayer) {
		currentSelectedCharacter = currentSelectedPlayer;
		PlayerPrefs.SetInt(CharactersKeys.key_CharacterSelected, currentSelectedCharacter);
    }

	public PlayerAllData GetRandomPlayerAsPerRarety(PlayerType Type) {

		List<PlayerAllData> list_Player = new List<PlayerAllData>();

		for (int i = 0; i < all_PlayerData.Length; i++) {
			if (all_PlayerData[i].playerRarity == Type) {

				PlayerAllData current = new PlayerAllData { CurrentLevel = all_CurrentLevelOfCharacters[i],
					player = all_PlayerData[i],
					isUnlocked = all_UnlockStatus[i],
					CardValue = all_CurrentCardsValue[i],
					MyPlayerIndex = i
					
				};

				list_Player.Add(current);
			}
		}


		int index = Random.Range(0, list_Player.Count);

		return list_Player[index];

	}

    public void GetCardPlayer(int getCard, int playerIndex) {


        all_CurrentCardsValue[playerIndex] += getCard;
        PlayerPrefs.SetInt(CharactersKeys.key_CharacterAmmountCard + playerIndex, all_CurrentCardsValue[playerIndex]);
		if (!all_UnlockStatus[playerIndex]) {
			if (all_CurrentCardsValue[playerIndex] >= all_PlayerData[playerIndex].cardsRequiredToUnlock) {
				all_UnlockStatus[playerIndex] = true;
				PlayerPrefs.SetInt(CharactersKeys.key_CharacterUnlocked + playerIndex , 1);
			}
		}
    }

    public List<PlayerAllData> GetAllPlayerAsPerChestIndex(int _ChestIndex) {

		List<PlayerAllData> current = new List<PlayerAllData>();
		for (int i = 0; i < all_PlayerData.Length; i++) {


            if (all_PlayerData[i].chestIndex <= _ChestIndex) {

                PlayerAllData current_Player = new PlayerAllData {
                    CurrentLevel = all_CurrentLevelOfCharacters[i],
                    player = all_PlayerData[i],
                    isUnlocked = all_UnlockStatus[i],
                    CardValue = all_CurrentCardsValue[i],
                    MyPlayerIndex = i

                };

                current.Add(current_Player);
            }
        }

		return current; 
       
    }
}


[System.Serializable]
public struct PlayerAllData {

	public PlayerDataScriptableObject player;
	public bool isUnlocked;
	public int CurrentLevel;
	public int CardValue;
	public int MyPlayerIndex;
}
