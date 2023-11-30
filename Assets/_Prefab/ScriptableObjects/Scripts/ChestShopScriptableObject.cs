using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ChestShop", menuName = "ScriptableObjects/ChestShopData")]
public class ChestShopScriptableObject : ScriptableObject
{
    public string str_ChestName;
    public Sprite sprite_ChestIcon;
    public int costToOpenTheChest;
    public int[] coinRewardRange;
    public int[] gemRewardRange;
    public int numberOfCommonCharactersToReward;
    public int[] commonCardRewardRange;
    public int numberOfRareCharactersToReward;
    public int[] rareCardRewardRange;
    public int numberOfEpicCharactersToReward;
    public int[] epicCardRewardRange;
}
