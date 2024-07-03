using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using System;
using Random = UnityEngine.Random;



public class ChestopeningUI : MonoBehaviour {

    [Header("Currency Data")]
    [SerializeField] private GameObject obj_Currency;
    [SerializeField] private Image img_Currency;
    [SerializeField] private TextMeshProUGUI txt_CurrencyName;
    [SerializeField] private TextMeshProUGUI txt_CurrencyValue;
   
      


    [Header("Card data")]
    [SerializeField] private Image img_CardBg;
    [SerializeField] private GameObject obj_Card;
  
    [SerializeField] private TextMeshProUGUI txt_CardRarety;
    [SerializeField] private TextMeshProUGUI txt_CardName;
    [SerializeField] private TextMeshProUGUI txt_CardEarnvalue;
    [SerializeField] private TextMeshProUGUI txt_SliderProgress;
    [SerializeField] private TextMeshProUGUI txt_CurrenPlayerLevel;
    [SerializeField] private Image img_CardImage;
    [SerializeField] private Slider slider_Progress;
    [SerializeField] private ChestShopScriptableObject chest;
    [SerializeField] private ChestSlot chest_Level;
    [SerializeField] private List<int> list_Data;
    [SerializeField] private int getGems;
    [SerializeField] private int GetCoin;
    [SerializeField] private List<PlayerAllData> list_Player;


   
  
   

 

    [Header("bag")]
    [SerializeField] private RectTransform[] all_bag;
    [SerializeField] private int bagIndex;
    [SerializeField] private GameObject taptoConintue;
    private float flt_DefaultScale;
    private float flt_BagAnimtionTime = 2;  
    private float flt_SliderMotionTime = 0.25f;
    private float flt_ChestAnimation = 0.5f;
    private float flt_AnimationComapltedTime;



    [Header("Reward - Summry")]
    [SerializeField] private ChestSummryData pf_ChestSummry;
    [SerializeField] private GameObject obj_RewardSummerParent;
    [SerializeField] private Transform transform_RewardSpawn;
    [SerializeField] private Sprite[] all_Sprite;
    [SerializeField] private Sprite sprite_Currency;
    [SerializeField] private int animationRunIndex = 0;
    [SerializeField] private GameObject Obj_AdsBtn;
    [SerializeField] private GameObject obj_Bottam;
    [SerializeField] private TextMeshProUGUI txt_GemsVaue;
    [SerializeField] private int noOfGemsValueGetReward2X;


    private bool isLevelChestOpening = false;
    private bool isallRewardShown = false;

    private void OnEnable() {
        
         flt_DefaultScale = all_bag[0].localScale.x;
        DailyTaskManager.increasedNoofKit?.Invoke(1);
    }


    public void OnClick_OnClosdBtnClcik() {
        DataManager.Instance.IncreasedAchiieveMent();
        this.gameObject.SetActive(false);
    }


#region  Get Chest Level Base
   public void ActivetedLevelChestOpnened(ChestSlot _Chest) {
        this.gameObject.SetActive(true);
        this.chest_Level = _Chest;
        isallRewardShown = false;
        foreach (Transform child in transform_RewardSpawn) {
            Destroy(child.gameObject);
        }

        bagIndex = Random.Range(0, all_bag.Length);
        isallRewardShown = false;
       
      
        animationRunIndex = 0;
        flt_AnimationComapltedTime = flt_BagAnimtionTime + 1.5f * flt_ChestAnimation + flt_SliderMotionTime * 2;
        isLevelChestOpening = true;
        GetLeveBasedChestRewardInPlayerPrefs();
        obj_Card.gameObject.SetActive(false);
        obj_Card.gameObject.SetActive(false);
        obj_RewardSummerParent.SetActive(false);
        StartCoroutine(PlayChestCompleteAnimation());
    }





   

    private void GetLeveBasedChestRewardInPlayerPrefs() {


        // Coin Reward
        int minValue = chest_Level.chestInThisSlot.all_MinimumCoinRewardBasedOnLevel[chest_Level.chestLevelIndex];
        int MaxValue = chest_Level.chestInThisSlot.all_MaximumCoinRewardBasedOnLevel[chest_Level.chestLevelIndex];
        GetCoin = Random.Range(minValue, MaxValue);
        DataManager.Instance.IncresedCoin(GetCoin);

        // Gems Reward


        minValue = chest_Level.chestInThisSlot.all_MinimumGemRewardBasedOnLevel[chest_Level.chestLevelIndex];
        MaxValue = chest_Level.chestInThisSlot.all_MaximumGemRewardBasedOnLevel[chest_Level.chestLevelIndex];
        getGems = Random.Range(minValue, MaxValue);
        DataManager.Instance.Incresedgems(getGems);

        // Add available cards according to chest level. Private List

        List<PlayerAllData> list_AsPerChestIndex = CharacterManager.Instance.GetAllPlayerAsPerChestIndex(0);

        // Guranted Epic
        for (int j = 0; j < chest_Level.chestInThisSlot.guaranteedEpicCardsToReward; j++) {

            for (int p = 0; p < list_AsPerChestIndex.Count; p++) {

                if (list_AsPerChestIndex[p].player.playerRarity == PlayerType.Epic) {

                    int Card = Random.Range(chest_Level.chestInThisSlot.all_EpicCardRewardRange[0],
                            chest_Level.chestInThisSlot.all_EpicCardRewardRange[1]);

                    CharacterManager.Instance.GetCardPlayer(Card, list_AsPerChestIndex[p].MyPlayerIndex);

                    list_Player.Add(list_AsPerChestIndex[p]);
                    list_Data.Add(Card);
                    list_AsPerChestIndex.RemoveAt(p);
                    break;

                }
            }
        }

        // Guaranted rare
        for (int k = 0; k < chest_Level.chestInThisSlot.guaranteedRareCardsToReward; k++) {

            for (int p = 0; p < list_AsPerChestIndex.Count; p++) {

                if (list_AsPerChestIndex[p].player.playerRarity == PlayerType.Rare) {

                    int Card = Random.Range(chest_Level.chestInThisSlot.all_EpicCardRewardRange[0],
                            chest_Level.chestInThisSlot.all_EpicCardRewardRange[1]);

                    CharacterManager.Instance.GetCardPlayer(Card, list_AsPerChestIndex[p].MyPlayerIndex);

                    list_Player.Add(list_AsPerChestIndex[p]);
                    list_Data.Add(Card);
                    list_AsPerChestIndex.RemoveAt(p);
                    break;

                }
            }
        }


        int Raemaning_Reward = chest_Level.chestInThisSlot.maxNumberOfDifferentCardsToReward -
                                    (chest_Level.chestInThisSlot.guaranteedRareCardsToReward +
                                     chest_Level.chestInThisSlot.guaranteedEpicCardsToReward);



        for (int i = 0; i < Raemaning_Reward; i++) {

            bool isGetReward = false;
            while (!isGetReward ) {

                float index = Random.Range(0, 100);
                if (index <= chest_Level.chestInThisSlot.flt_EpicCardRewardProbability) {

                    if (CheckinIsRaretyAvalibleOrNot(list_AsPerChestIndex, PlayerType.Epic)) {
                        //Get Reward
                        for (int j = 0; j < list_AsPerChestIndex.Count; j++) {

                            if (list_AsPerChestIndex[j].player.playerRarity == PlayerType.Epic) {

                                int Card = Random.Range(chest_Level.chestInThisSlot.all_EpicCardRewardRange[0],
                               chest_Level.chestInThisSlot.all_EpicCardRewardRange[1]);

                                CharacterManager.Instance.GetCardPlayer(Card, list_AsPerChestIndex[j].MyPlayerIndex);

                                list_Player.Add(list_AsPerChestIndex[j]);
                                list_Data.Add(Card);
                                list_AsPerChestIndex.RemoveAt(j);
                                isGetReward = true;
                                break;
                            }

                        }
                    }

                }
                else if (index <= chest_Level.chestInThisSlot.flt_RareCardRewardProbability) {

                    if (CheckinIsRaretyAvalibleOrNot(list_AsPerChestIndex, PlayerType.Rare)) {
                        //Get Reward
                        for (int j = 0; j < list_AsPerChestIndex.Count; j++) {

                            if (list_AsPerChestIndex[j].player.playerRarity == PlayerType.Rare) {

                                int Card = Random.Range(chest_Level.chestInThisSlot.all_RareCardRewardRange[0],
                               chest_Level.chestInThisSlot.all_RareCardRewardRange[1]);

                                CharacterManager.Instance.GetCardPlayer(Card, list_AsPerChestIndex[j].MyPlayerIndex);

                                list_Player.Add(list_AsPerChestIndex[j]);
                                list_Data.Add(Card);
                                list_AsPerChestIndex.RemoveAt(j);
                                isGetReward = true;
                                break;
                            }

                        }
                    }
                }
                else {

                    if (CheckinIsRaretyAvalibleOrNot(list_AsPerChestIndex, PlayerType.Common)) {
                        //Get Reward
                        for (int j = 0; j < list_AsPerChestIndex.Count; j++) {

                            if (list_AsPerChestIndex[j].player.playerRarity == PlayerType.Common) {

                                int Card = Random.Range(chest_Level.chestInThisSlot.all_NormalCardRewardRange[0],
                               chest_Level.chestInThisSlot.all_NormalCardRewardRange[1]);

                                CharacterManager.Instance.GetCardPlayer(Card, list_AsPerChestIndex[j].MyPlayerIndex);

                                list_Player.Add(list_AsPerChestIndex[j]);
                                list_Data.Add(Card);
                                list_AsPerChestIndex.RemoveAt(j);
                                isGetReward = true;
                                break;
                            }

                        }
                    }
                }
            }
       
        }

    }

    private bool CheckinIsRaretyAvalibleOrNot(List<PlayerAllData> list_AsPerChestIndex ,PlayerType Current) {

        bool IsFind = false;

        for (int i = 0; i < list_AsPerChestIndex.Count; i++) {

            if (list_AsPerChestIndex[i].player.playerRarity == Current) {

                IsFind = true;
                break;
            }
        }

        return IsFind;
        
    }

    #endregion

    #region Chest Shop Base
    /// <summary>
    ///  this Function When Shop Panel Purchase Chest At That Time Called
    /// </summary>
    /// <param name="_Chest"></param>
    public void ActavetedShopBaseChest(ChestShopScriptableObject _Chest , int Bagindex) {

        this.gameObject.SetActive(true); 
        this.chest = _Chest; 
        isallRewardShown = false; 
        foreach (Transform child in transform_RewardSpawn) {
            Destroy(child.gameObject);
        }

       
        bagIndex = Bagindex;
        animationRunIndex = 0;
        flt_AnimationComapltedTime = flt_BagAnimtionTime + 1.5f * flt_ChestAnimation + flt_SliderMotionTime*2;
        isLevelChestOpening = false;
        SetShopBaseChestInPlayerPrefs();
        obj_Card.gameObject.SetActive(false);
        obj_RewardSummerParent.SetActive(false);
      
        StartCoroutine(PlayChestCompleteAnimation());
    }

    private void SetShopBaseChestInPlayerPrefs() {

        list_Data.Clear();
        list_Player.Clear();

        // get Coin Coin Reward
        GetCoin = Random.Range(chest.coinRewardRange[0], chest.coinRewardRange[1]);
        DataManager.Instance.IncresedCoin(GetCoin);
        // get Gems Reward
        getGems = Random.Range(chest.gemRewardRange[0], chest.gemRewardRange[1]);
        DataManager.Instance.Incresedgems(getGems);
        // Get Comman Reward
        for (int i = 0; i < chest.numberOfCommonCharactersToReward; i++) {
            int GetCard = Random.Range(chest.commonCardRewardRange[0], chest.commonCardRewardRange[1]);
            list_Data.Add(GetCard);
            PlayerAllData Player = CharacterManager.Instance.GetRandomPlayerAsPerRarety(PlayerType.Common);
            list_Player.Add(Player);
            CharacterManager.Instance.GetCardPlayer(GetCard, Player.MyPlayerIndex);
        }
        // Get Rare Reward
        for (int i = 0; i < chest.numberOfRareCharactersToReward; i++) {

            int GetCard = Random.Range(chest.rareCardRewardRange[0], chest.rareCardRewardRange[1]);
            list_Data.Add(GetCard);
            PlayerAllData Player = CharacterManager.Instance.GetRandomPlayerAsPerRarety(PlayerType.Rare);
            list_Player.Add(Player);
            CharacterManager.Instance.GetCardPlayer(GetCard, Player.MyPlayerIndex);

        }
        // Epic Reward
        for (int i = 0; i < chest.numberOfEpicCharactersToReward; i++) {
            int GetCard = Random.Range(chest.epicCardRewardRange[0], chest.epicCardRewardRange[1]);
            list_Data.Add(GetCard);
            PlayerAllData Player = CharacterManager.Instance.GetRandomPlayerAsPerRarety(PlayerType.Epic);
            list_Player.Add(Player);
            CharacterManager.Instance.GetCardPlayer(GetCard, Player.MyPlayerIndex);

        }


    }

    #endregion


 #region when User Continue SetNext Animation

    public void ActivateTapContinue() {

        Debug.Log("Tap To Conintue");
       
        StopAllCoroutines();
        DOTween.KillAll();
        img_CardBg.transform.localScale = Vector3.one * flt_DefaultScale;
        obj_Card.gameObject.SetActive(false);
        obj_Currency.gameObject.SetActive(false);
       
        if (animationRunIndex == 1) {

            StartCoroutine(GemsPlayAnimtion());
        }
        else {

            StartCoroutine(CardPlayAnimation());
        }
        
       
    }

    private IEnumerator CardPlayAnimation() {

        int index = animationRunIndex - 1;
        for (int i = index; i < list_Player.Count; i++) {


            RewardHandler(list_Data[i], list_Player[i],i,false);
            animationRunIndex++;
            yield return new WaitForSeconds(flt_AnimationComapltedTime);
        }


        isallRewardShown = true;
        ActvetedSummryPanel();


        Debug.Log("CompltedAnimation");
    }

    private IEnumerator GemsPlayAnimtion() {
        GemsAnimtion();
        animationRunIndex++;
        yield return new WaitForSeconds(flt_AnimationComapltedTime);
        for (int i = 0; i < list_Player.Count; i++) {


            RewardHandler(list_Data[i], list_Player[i],i,false);
            animationRunIndex++;
            yield return new WaitForSeconds(flt_AnimationComapltedTime);
        }

        isallRewardShown = true;
        ActvetedSummryPanel();

        Debug.Log("CompltedAnimation");
    }

    #endregion


 #region ChestAnimationHandler

    private IEnumerator PlayChestCompleteAnimation() {
        CoinGetting();
        animationRunIndex++;
        yield return new WaitForSeconds(flt_AnimationComapltedTime);
        GemsAnimtion();
        animationRunIndex++;
        yield return new WaitForSeconds(flt_AnimationComapltedTime);



        for (int i = 0; i < list_Player.Count; i++) {


            RewardHandler(list_Data[i], list_Player[i],i,false);
            animationRunIndex++;
            yield return new WaitForSeconds(flt_AnimationComapltedTime);
        }



        //panelSlote.gameObject.SetActive(true);
        isallRewardShown = true;
        ActvetedSummryPanel();

      


        Debug.Log("CompltedAnimation");

    }

    private void SetrawardSummryPanel() {

        taptoConintue.gameObject.SetActive(false);
       
        

        if (obj_RewardSummerParent.activeSelf) {
            return;
        }
        obj_RewardSummerParent.gameObject.SetActive(true);
        ChestSummryData current = Instantiate(pf_ChestSummry, transform.position, transform.rotation, transform_RewardSpawn);
        current.SetChestSummryPanel("++" + GetCoin, "Coins", DataManager.Instance.sprite_Coin,sprite_Currency);
         current = Instantiate(pf_ChestSummry, transform.position, transform.rotation, transform_RewardSpawn);
        current.SetChestSummryPanel("++" + getGems, "Gems", DataManager.Instance.sprie_Gems, sprite_Currency);
        for (int i = 0; i < list_Player.Count; i++) {

             current = Instantiate(pf_ChestSummry, transform.position, transform.rotation, transform_RewardSpawn);
            current.SetChestSummryPanel("++" + list_Data[i], list_Player[i].player.str_PlayerName,
                list_Player[i].player.sprite_PlayerIcon, all_Sprite[((int)list_Player[i].player.playerRarity)]);

        }

        Obj_AdsBtn.SetActive(AdsManager.instance.IsRewardAdLoad);
    }

    private void CoinGetting() {
        // Coin Animation
      

        img_Currency.sprite = DataManager.Instance.sprite_Coin;
        txt_CurrencyName.text = "Coin";
        txt_CurrencyValue.text = ("+" + GetCoin);
        obj_Card.SetActive(false);
        obj_Currency.transform.localScale = Vector3.zero;
        obj_Card.transform.localScale = Vector3.zero;
        obj_Currency.SetActive(false);
        PlayCurrencyAnimation();
    }

    private void GemsAnimtion() {

       
       
        img_Currency.sprite = DataManager.Instance.sprie_Gems;
        txt_CurrencyName.text = "Gems";
        txt_CurrencyValue.text = ("+" + getGems);
        obj_Card.SetActive(false);
        obj_Currency.transform.localScale = Vector3.zero;
        obj_Card.transform.localScale = Vector3.zero;
        obj_Currency.SetActive(false);
        PlayCurrencyAnimation();

    }
    private void RewardHandler(int gettingCards, PlayerAllData Player,int index,bool isactvted) {



        img_CardBg.sprite = all_Sprite[((int)Player.player.playerRarity)];
        txt_CardEarnvalue.text = "+ " + gettingCards.ToString();
        txt_CardName.text = Player.player.str_PlayerName;
        txt_CardRarety.text = Player.player.playerRarity.ToString();
        img_CardImage.sprite = Player.player.sprite_PlayerIcon;
        txt_CurrenPlayerLevel.text = Player.CurrentLevel.ToString();
        txt_SliderProgress.text = (Player.CardValue + "/" + (Player.CardValue + gettingCards)).ToString();
       
        PlayCardAnimation(index );

    }

   

    private void PlayCurrencyAnimation() {

        Sequence seq = DOTween.Sequence();
        taptoConintue.SetActive(false);
      

        for (int i = 0; i < all_bag.Length; i++) {
            all_bag[i].gameObject.SetActive(false);
        }
        all_bag[bagIndex].gameObject.SetActive(true);
        Animator current = all_bag[bagIndex].GetComponent<Animator>();
        current.SetBool("isBagAnimation", true);
        AudioManager.insatance.PlayBagOpenedSfx();
        seq.AppendInterval(flt_BagAnimtionTime).AppendCallback(() => current.SetBool("isBagAnimation", false))
            .AppendCallback(() => obj_Currency.SetActive(true)).AppendCallback(() => taptoConintue.SetActive(true)).
            AppendCallback(AudioManager.insatance.PlayCardShowSFX).
            Append(obj_Currency.transform.DOScale(1, flt_ChestAnimation).SetEase(Ease.OutBack)).AppendInterval(flt_SliderMotionTime * 2)
            .Append(obj_Currency.transform.DOScale(0, flt_ChestAnimation / 2));


    }

    private void ActvetedSummryPanel() {
       
        if (isallRewardShown) {

           
            SetrawardSummryPanel();
        }
       
    }

    private void PlayCardAnimation(int playerIndex) {

       
        slider_Progress.value = list_Player[playerIndex].CardValue;
        slider_Progress.maxValue = list_Player[playerIndex].CardValue + list_Data[playerIndex];

        Sequence seq = DOTween.Sequence();
        taptoConintue.SetActive(false);
        //seq.Append(bag.DOScale(Vector3.one * (flt_DefaultScale + flt_bagScale), flt_BagAnimationTime)).
        //    Append(bag.DOScale(Vector3.one * (flt_DefaultScale - flt_bagScale), flt_BagAnimationTime).SetLoops(4, LoopType.Yoyo))
        //    .
        //    Append(bag.DOScale(Vector3.one * (flt_DefaultScale), flt_BagAnimationTime)).Append(bag.DOShakePosition(flt_BagAnimationTime, 25, 40))
        //    .AppendCallback(() => obj_Card.SetActive(true)).AppendCallback(() => taptoConintue.SetActive(true)).
        //    Append(obj_Card.transform.DOScale(1, flt_ChestAnimation).SetEase(Ease.OutBack)).AppendCallback(()=>SliderAnimation(playerIndex)).AppendInterval(flt_BagAnimationTime * 2)
        //    .Append(obj_Card.transform.DOScale(0, flt_ChestAnimation / 2)).AppendCallback(ActvetedSummryPanel);


        for (int i = 0; i < all_bag.Length; i++) {
            all_bag[i].gameObject.SetActive(false);
        }
        all_bag[bagIndex].gameObject.SetActive(true);

        Animator current = all_bag[bagIndex].GetComponent<Animator>();
        current.SetBool("isBagAnimation", true);
        AudioManager.insatance.PlayBagOpenedSfx();
        seq.AppendInterval(flt_BagAnimtionTime).AppendCallback(() => current.SetBool("isBagAnimation", false))
             .AppendCallback(() => obj_Card.SetActive(true)).AppendCallback(() => taptoConintue.SetActive(true)).
             AppendCallback(AudioManager.insatance.PlayCardShowSFX).
            Append(obj_Card.transform.DOScale(1, flt_ChestAnimation).SetEase(Ease.OutBack)).AppendCallback(() => SliderAnimation(playerIndex)).AppendInterval(flt_SliderMotionTime * 2)
            .Append(obj_Card.transform.DOScale(0, flt_ChestAnimation / 2));
    }

    private void SliderAnimation(int index) {
     
        StartCoroutine(SetText(flt_SliderMotionTime * 2, slider_Progress.maxValue));
    }

    private IEnumerator SetText(float time, float maxValue) {

        float flt_CurrentTime = 0;
        float startData = slider_Progress.value;

        while (flt_CurrentTime < 1) {

            flt_CurrentTime += Time.deltaTime / time;

            float targetValue = Mathf.Lerp(startData, maxValue, flt_CurrentTime);
            slider_Progress.value = targetValue;
            txt_SliderProgress.text = targetValue.ToString("f0")+ "/" + maxValue.ToString();


           

            yield return null;
            
        }
        txt_SliderProgress.text = maxValue.ToString() +"/" + maxValue.ToString();
        slider_Progress.value = maxValue;
       
    }


    #endregion

    public void Onclick_AdsBtn() {

        if (DataManager.Instance.skipIts <= 0) {
            AdsManager.instance.ShowRewardAds(AdsRewardType.chestOpning);
        }
        else {
            DataManager.Instance.RemoveSkipIts();
        }
        
    }

    public void OnClick_GemsReduceBtn() {
        if (DataManager.Instance.Gems <= noOfGemsValueGetReward2X) {
            UIManager.Instance.spawnPopup("No Enough Gems");
            return;
        }

        DataManager.Instance.DecresedGems(noOfGemsValueGetReward2X);
        GetAllReward2X();
    
    }


    public void GetAllReward2X() {
     
        isallRewardShown = true;
        SetrawardSummryPanel();
        obj_Bottam.SetActive(false);
        DataManager.Instance.IncresedCoin(GetCoin);
        GetCoin *=2;
        DataManager.Instance.Incresedgems(getGems);
        getGems *= 2;
        for (int i = 0; i < list_Player.Count; i++) {

            CharacterManager.Instance.GetCardPlayer(list_Data[i], list_Player[i].MyPlayerIndex);
            list_Data[i] *= 2;
           
        }

        ChestSummryData current = transform_RewardSpawn.GetChild(0).GetComponent<ChestSummryData>();
        current.SetChestSummryPanel("++" + GetCoin, "Coins", DataManager.Instance.sprite_Coin, sprite_Currency);
        current = transform_RewardSpawn.GetChild(1).GetComponent<ChestSummryData>();
        current.SetChestSummryPanel("++" + getGems, "Gems", DataManager.Instance.sprie_Gems, sprite_Currency);
        for (int i = 0; i < list_Player.Count; i++) {

            CharacterManager.Instance.GetCardPlayer(list_Data[i], list_Player[i].MyPlayerIndex);
            current = transform_RewardSpawn.GetChild(i + 2).GetComponent<ChestSummryData>();
            current.SetChestSummryPanel("++" + list_Data[i], list_Player[i].player.str_PlayerName,
                list_Player[i].player.sprite_PlayerIcon, all_Sprite[((int)list_Player[i].player.playerRarity)]);
        }

       
       

        // Coin Reward 2X;



        // Gems Reward 2X


    }

   
   
}
