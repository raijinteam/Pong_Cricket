using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
	public static DataManager Instance;

	private void Awake()
	{
		Instance = this;
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Tab))
		{
			PlayerPrefs.DeleteAll();
		}
	}

	[field: SerializeField] public int coins { get; set; }
	[field: SerializeField] public int skipIts { get; set; }
	[field:SerializeField]public int trophy { get; set; }

	public void UpdateSkipItsValue(int _amount)
	{
		skipIts += _amount;		

		if(skipIts > 0)
		{
			RewardsManager.Instance.wheelRouletteRewardData.ActivateWheelRoulette();
		}
	}

    public void WonGame() {

		int levelIndex = LevelManager.Instance.currentLevelIndex;
		coins += LevelManager.Instance.GetLevelWinAmount(levelIndex);
		trophy += LevelManager.Instance.GetWinTrophyAmount(levelIndex);

	}

	public void LoseGame() {
		int levelIndex = LevelManager.Instance.currentLevelIndex;
		trophy -= LevelManager.Instance.GetWinTrophyAmount(levelIndex);
        if (trophy <0) {
			trophy = 0;
        }
	}
}
