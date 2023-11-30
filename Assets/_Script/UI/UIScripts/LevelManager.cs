using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
	public static LevelManager Instance;

	private void Awake()
	{
		Instance = this;
	}

	[SerializeField] private LevelDataScriptableObject[] all_LevelData;
	public int currentLevelIndex;

	private void Start()
	{
		UIManager.Instance.ui_LevelSelection.SpawnAllLevels();
	}

	public int GetTotalNumberOfLevels()
	{
		return all_LevelData.Length;
	}

	public string GetLevelName(int _index)
	{
		return all_LevelData[_index].str_LevelName;
	}

	public Sprite GetLevelIcon(int _index)
	{
		return all_LevelData[_index].sprite_LevelIcon;
	}

	public int GetWinTrophyAmount(int _index)
	{
		return all_LevelData[_index].winTrophy;
	}

	public int GetLoseTrophyAmount(int _index)
	{
		return all_LevelData[_index].loseTrophy;
	}

	public int GetTrophyRequiredToPlayLevel(int _index)
	{
		return all_LevelData[_index].requiredTrophyToPlay;
	}

	public int GetLevelWinAmount(int _index)
	{
		return all_LevelData[_index].prize;
	}

	public int GetLevelEntryFee(int _index)
	{
		return all_LevelData[_index].entryFee;
	}
}
