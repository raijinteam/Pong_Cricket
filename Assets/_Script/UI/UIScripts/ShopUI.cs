using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopUI : MonoBehaviour
{
	[Header("Chests")]
	[SerializeField] private ShopChestInfoUI ui_ShopChestInfo;
	[SerializeField] private ChestShopScriptableObject[] all_Chests;
	[SerializeField] private TextMeshProUGUI[] all_txt_ChestNames;
	[SerializeField] private TextMeshProUGUI[] all_txt_ChestPrices;
	[SerializeField] private Image[] all_img_ChestIcons;
	

    [Header("Gems")]
    [SerializeField] private float[] all_GemPackPrices;
    [SerializeField] private int[] all_GemPackRewards;
	[SerializeField] private TextMeshProUGUI[] all_txt_GemPackRewards;
	[SerializeField] private TextMeshProUGUI[] all_txt_GemPackPrices;
	[SerializeField] private GameObject[] all_panel_BestValueGem;
	[SerializeField] private int bestValueIndexGem;
    public float flt_GemsPostion;

    [Header("Skip-It")]
	[SerializeField] private float[] all_SkipItPrices;
	[SerializeField] private int[] all_SkipItRewards;
	[SerializeField] private TextMeshProUGUI[] all_txt_SkipItRewards;
	[SerializeField] private TextMeshProUGUI[] all_txt_SkipItPrices;
	[SerializeField] private GameObject[] all_panel_BestValueSkipIt;
	[SerializeField] private int bestValueIndexSkipIt;
    public float flt_SkipItsPostion;


    [Header("Coins")]
	[SerializeField] private int[] all_CoinsPrices;
	[SerializeField] private int[] all_CoinsRewards;
	[SerializeField] private TextMeshProUGUI[] all_txt_CoinsRewards;
	[SerializeField] private TextMeshProUGUI[] all_txt_CoinsPrices;
	[SerializeField] private GameObject[] all_panel_BestValueCoins;
	[SerializeField] private int bestValueIndexCoins;
	public float flt_GoldPostion;


	[SerializeField] private RectTransform rect_Scroll;


    

    private void Start()
	{
		SetChestPanel();
		SetGemsPanel();
		SetSkipItPanel();
		SetCoinsPanel();
		

    }

	private void SetChestPanel()
	{
		for(int i = 0; i < all_Chests.Length; i++)
		{
			all_txt_ChestNames[i].text = all_Chests[i].str_ChestName;
			all_txt_ChestPrices[i].text = all_Chests[i].costToOpenTheChest.ToString();
			all_img_ChestIcons[i].sprite = all_Chests[i].sprite_ChestIcon;
		}
	}

	private void SetGemsPanel()
	{
		for(int i = 0; i < all_GemPackPrices.Length; i++)
		{
			string formatedHeaderValue = UtilityManager.Instance.FormatIntegerValueToStringWithComma(all_GemPackRewards[i]);
			all_txt_GemPackRewards[i].text = formatedHeaderValue;
			all_txt_GemPackPrices[i].text = "$" + all_GemPackPrices[i].ToString();

			all_panel_BestValueGem[i].SetActive(false);
		}

		all_panel_BestValueGem[bestValueIndexGem].SetActive(true);
	}

	private void SetSkipItPanel()
	{
		for (int i = 0; i < all_SkipItPrices.Length; i++)
		{
			string formatedHeaderValue = UtilityManager.Instance.FormatIntegerValueToStringWithComma(all_SkipItRewards[i]);
			all_txt_SkipItRewards[i].text = formatedHeaderValue;
			all_txt_SkipItPrices[i].text = "$" + all_SkipItPrices[i].ToString();

			all_panel_BestValueSkipIt[i].SetActive(false);
		}

		all_panel_BestValueSkipIt[bestValueIndexSkipIt].SetActive(true);
	}

	private void SetCoinsPanel()
	{
		for (int i = 0; i < all_CoinsPrices.Length; i++)
		{
			string formatedHeaderValue = UtilityManager.Instance.FormatIntegerValueToStringWithComma(all_CoinsRewards[i]);
			all_txt_CoinsRewards[i].text = formatedHeaderValue;
			all_txt_CoinsPrices[i].text = all_CoinsPrices[i].ToString();

			all_panel_BestValueCoins[i].SetActive(false);
		}

		all_panel_BestValueCoins[bestValueIndexCoins].SetActive(true);
	}

	public void OnClick_ChestInfo(int _index)
	{
		ui_ShopChestInfo.SetChestPanel(all_Chests[_index]);	
	}

    public void SetPostion(float postion) {

		rect_Scroll.anchoredPosition = new Vector2(rect_Scroll.anchoredPosition.x, postion);
    }
}
