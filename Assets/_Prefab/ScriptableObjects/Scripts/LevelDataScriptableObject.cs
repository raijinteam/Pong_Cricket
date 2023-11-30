using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "ScriptableObjects/LevelData")]
public class LevelDataScriptableObject : ScriptableObject
{
	public string str_LevelName;
	public Sprite sprite_LevelIcon;
	public int requiredTrophyToPlay;
	public int entryFee;
	public int prize;
	public int winTrophy;
	public int loseTrophy;	
}
