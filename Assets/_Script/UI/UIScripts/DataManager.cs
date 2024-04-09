
using System;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;


public class DataManager : MonoBehaviour
{
	public static DataManager Instance;

	private void Awake()
	{
      
		Instance = this;
	}


    [SerializeField] private int startGameLevelUpgrade;
    public int currentValue;
    public float nextLevelUnlocked;
    [SerializeField] private int presentageValueInCreasedUpgradeValue;
    [SerializeField] private int GameWinTimeGetUpgrade;
    [SerializeField] private bool isShownGameTutorial;
    [SerializeField] private bool isShownMiniGameTutorial;
    [SerializeField] private TutorialHandler tutorialHandler;
    [SerializeField] private Mini_GameManager mini_GameManager;


    [field :Header("PlayerData")]
    [field: SerializeField] public string playerName { get; private set; }
    [field: SerializeField] public int GameLevel { get; private set; }

    [field :SerializeField] public Sprite img_PlayerSprite { get; private set; }

    [field :Header("player ai Data")]

    [field: SerializeField] public string playerNameAI { get;  set; }
    [field: SerializeField] public int GameLevelAI { get;  set; }

    [field: SerializeField] public Sprite img_PlayerSpriteAi { get;  set; }


    [field: Header("Currency Data")]

    [field: SerializeField] public int no_OfClaimAchieveMent; 
    [field: SerializeField] public int coins { get; private set; }
	[field : SerializeField] public int Gems { get; private set; }
	[field: SerializeField] public int skipIts { get;private  set; }
	[field:SerializeField]public int trophy { get; private set; }
	[field : SerializeField]public Sprite sprite_Coin { get; private set; }
	[field : SerializeField] public Sprite sprie_Gems { get; private set; }
    [field : SerializeField] public Sprite sprite_skipIts { get; private set; }
    [field: SerializeField] public Sprite sprite_Ads { get; private set; }

    [field: SerializeField] public bool isNoadsPurcahsed;
    public delegate void SetSptite(Sprite sprite);

    public SetSptite Changesprite;



    private void Start() {

        Application.targetFrameRate = 60;
        SetData();



        if (!isShownGameTutorial) {

            GameAnalyticsManager.instance.AddNewDiesign("GameStart FirstTime");
            UIManager.Instance.panel_MainMenu.SetActive(false);
            Instantiate(tutorialHandler, transform.position, Quaternion.identity);
        }
        else if (GameLevel == 1 && !isShownMiniGameTutorial) {
            UIManager.Instance.panel_MainMenu.SetActive(false);
            GameManager.Instance.obj_GameEnvironement.gameObject.SetActive(false);
            Instantiate(mini_GameManager, transform.position, transform.rotation);
        }
        else {
            UIManager.Instance.ui_HomeScreen.gameObject.SetActive(true);
            UIManager.Instance.panel_MainMenu.gameObject.SetActive(true);
            UIManager.Instance.panel_CommanMenu.gameObject.SetActive(true);
        }
		nextLevelUnlocked = startGameLevelUpgrade;
        for (int i = 0; i < GameLevel; i++) {

            nextLevelUnlocked += (nextLevelUnlocked * 0.01f * presentageValueInCreasedUpgradeValue);
        }

        if (isNoadsPurcahsed) {

            //AdsManager.instance.LoadInterstitalAds();
            //AdsManager.instance.LoadRewardAds();

        }

        AdsManager.instance.LoadInterstitalAds();
        AdsManager.instance.LoadRewardAds();

    }

    private void Update() {

		if (Input.GetKeyDown(KeyCode.Escape)) {
			IncreasedGameLevel();
        }
    }

    private void SetData() {

		if (PlayerPrefs.HasKey(DataKeys.key_Coin)) {
			LoadDataFromPlayerPrefs();
		}
		else {
			SaveDataInPlayerPrefs();
		}
    }

    private void SaveDataInPlayerPrefs() {

        PlayerPrefs.SetInt(DataKeys.key_Coin, coins);
        PlayerPrefs.SetInt(DataKeys.key_Gems, Gems);
        PlayerPrefs.SetInt(DataKeys.key_SkipIts, skipIts);
        PlayerPrefs.SetInt(DataKeys.key_Trophy, trophy);
        PlayerPrefs.SetInt(DataKeys.key_GameLevel, GameLevel);
        PlayerPrefs.SetInt(DataKeys.key_GamelevelUpgradeValue, currentValue);


        PlayerPrefs.SetInt(DataKeys.key_ShownGameTutorial, isShownGameTutorial ? 1 : 0);
        PlayerPrefs.SetInt(DataKeys.key_ShownMinGameTutorial, isShownMiniGameTutorial?1:0);
        PlayerPrefs.SetInt(DataKeys.key_NoAdsPurchased, isNoadsPurcahsed ? 1 : 0);
        PlayerPrefs.SetInt(DataKeys.key_noOfAchieveMent, no_OfClaimAchieveMent);
       

    }

    private void LoadDataFromPlayerPrefs() {

        coins = PlayerPrefs.GetInt(DataKeys.key_Coin);
        Gems = PlayerPrefs.GetInt(DataKeys.key_Gems);
        skipIts = PlayerPrefs.GetInt(DataKeys.key_SkipIts);
        trophy = PlayerPrefs.GetInt(DataKeys.key_Trophy);
		GameLevel = PlayerPrefs.GetInt(DataKeys.key_GameLevel);
		currentValue = PlayerPrefs.GetInt(DataKeys.key_GamelevelUpgradeValue);
        isShownGameTutorial = (PlayerPrefs.GetInt(DataKeys.key_ShownGameTutorial) == 1);
        isShownMiniGameTutorial = (PlayerPrefs.GetInt(DataKeys.key_ShownMinGameTutorial) == 1);
        isNoadsPurcahsed = (PlayerPrefs.GetInt(DataKeys.key_NoAdsPurchased) == 1);
        no_OfClaimAchieveMent = PlayerPrefs.GetInt(DataKeys.key_noOfAchieveMent);

    }



    public void DecresedCoin(int _coin) {
		coins -= _coin;
		PlayerPrefs.SetInt(DataKeys.key_Coin, coins);

    }

	public void IncresedCoin(int _Coin) {
		coins += _Coin;
        PlayerPrefs.SetInt(DataKeys.key_Coin, coins);
    }
    public void Incresedgems(int _Gems) {
        Gems += _Gems;
        PlayerPrefs.SetInt(DataKeys.key_Gems, Gems);
    }

	public void DecresedGems(int _Gems) {
		Gems -= _Gems;
        PlayerPrefs.SetInt(DataKeys.key_Gems, Gems);
    }





    public void UpdateSkipItsValue(int _amount) {

        skipIts += _amount;
        PlayerPrefs.SetInt(DataKeys.key_SkipIts, skipIts);
        if (skipIts > 0) {
            RewardsManager.Instance.wheelRouletteRewardData.ActivateWheelRoulette();
        }
    }
	


	public void LoseGame() {
		int levelIndex = LevelManager.Instance.currentLevelIndex;
		trophy -= LevelManager.Instance.GetLoseTrophyAmount(levelIndex);
        if (trophy <0) {
			trophy = 0;
        }

		PlayerPrefs.SetInt(DataKeys.key_Trophy, trophy);
	}

    

    public void WonGame() {

        int levelIndex = LevelManager.Instance.currentLevelIndex;
        trophy += LevelManager.Instance.GetWinTrophyAmount(levelIndex);
        IncresedCoin(LevelManager.Instance.GetLevelWinAmount(levelIndex));
		IncreasedGameLevel();
        PlayerPrefs.SetInt(DataKeys.key_Trophy, trophy);
    }

    private void IncreasedGameLevel() {

        currentValue += GameWinTimeGetUpgrade;
        if (currentValue >= nextLevelUnlocked) {
            GameLevel++;
            UIManager.Instance.spawnPopup("Level Increased");
            if (GameLevel == 1) {
                UIManager.Instance.panel_MainMenu.gameObject.SetActive(false);
                GameManager.Instance.obj_GameEnvironement.gameObject.SetActive(false);
                Instantiate(mini_GameManager, transform.position, transform.rotation);
            }
            PlayerPrefs.SetInt(DataKeys.key_GameLevel, GameLevel);
            currentValue = 0;
            nextLevelUnlocked = startGameLevelUpgrade;
            for (int i = 0; i < GameLevel; i++) {

                nextLevelUnlocked += (nextLevelUnlocked * 0.01f * presentageValueInCreasedUpgradeValue);
            }

        }

        PlayerPrefs.SetInt(DataKeys.key_GamelevelUpgradeValue, currentValue);
    }

    public void RemoveSkipIts() {

        skipIts--;
        if (skipIts <= 0) {
            skipIts = 0;
            Changesprite?.Invoke(sprite_Ads);
        }
        else {
            Changesprite?.Invoke(sprite_skipIts);
        }

    }
    public Sprite GetSprite() {
        if (skipIts <= 0) {
            return sprite_Ads;
        }
        else {
            return sprite_skipIts;
        }
    }

    public void ShownGameTutorial() {
        isShownGameTutorial = true;
        GameAnalyticsManager.instance.AddNewDiesign("Tutorial Shown In Game");
        PlayerPrefs.SetInt(DataKeys.key_ShownGameTutorial, 1);

    }

    public void ShownMinGameTutotal() {
        if (!isShownMiniGameTutorial) {
            PlayerPrefs.SetInt(DataKeys.key_ShownMinGameTutorial, 1);
            GameAnalyticsManager.instance.AddNewDiesign("Show Mini Game Tutorial");
            isShownMiniGameTutorial = true;
        }
    }

    public void SetAiLevel() {
        GameLevelAI = Random.Range(GameLevel, GameLevel + 5);
    }

    public void IncreasedAchiieveMent() {
        no_OfClaimAchieveMent++;
        PlayerPrefs.SetInt(DataKeys.key_noOfAchieveMent, no_OfClaimAchieveMent);
        GameAnalyticsManager.instance.AddNewEventWithData("IncresedAchevement" , no_OfClaimAchieveMent);

    }

}


