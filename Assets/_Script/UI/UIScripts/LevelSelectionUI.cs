using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LevelSelectionUI : MonoBehaviour
{
	[SerializeField] private LevelDataUI ui_LevelData;
	[SerializeField] private Transform levelDataParent;
	[SerializeField] private LevelDataUI[] all_InstantiatedLevelData;
	[SerializeField] private LevelScrollSnapSystem ui_LevelScroll;
	[SerializeField] private GameObject btn_PreviousLevel;
	[SerializeField] private GameObject btn_NextLevel;

	private void OnEnable()
	{
		HandleNextAndPreviousButton();
		SetAllLevelData();
	}

	public void HandleNextAndPreviousButton()
	{
		if (ui_LevelScroll.HasReachedFirstLevelIndex())
		{
			btn_PreviousLevel.SetActive(false);
		}
		else
		{
			btn_PreviousLevel.SetActive(true);
		}

		if (ui_LevelScroll.HasReachedMaxLevelIndex())
		{
			btn_NextLevel.SetActive(false);
		}
		else
		{
			btn_NextLevel.SetActive(true);
		}
	}

	public void SpawnAllLevels()
	{
		all_InstantiatedLevelData = new LevelDataUI[LevelManager.Instance.GetTotalNumberOfLevels()];

		for(int i = 0; i < LevelManager.Instance.GetTotalNumberOfLevels(); i++)
		{
			all_InstantiatedLevelData[i] = Instantiate(ui_LevelData, ui_LevelData.transform.position, ui_LevelData.transform.rotation, levelDataParent);		
		}

		ui_LevelScroll.SetSampleItem(all_InstantiatedLevelData[0].gameObject.GetComponent<RectTransform>());
	}

	private void SetAllLevelData()
	{
		for (int i = 0; i < LevelManager.Instance.GetTotalNumberOfLevels(); i++)
		{
			all_InstantiatedLevelData[i].SetLevelData(i);
		}
	}

	public void OnClick_NextLevel()
	{
		ui_LevelScroll.SwitchToNextLevel();

		if (!btn_PreviousLevel.activeSelf)
		{
			btn_PreviousLevel.SetActive(true);
		}

		if (ui_LevelScroll.HasReachedMaxLevelIndex())
		{
			btn_NextLevel.SetActive(false);
		}
	}

	public void OnClick_PreviousLevel()
	{
		ui_LevelScroll.SwitchToPreviousLevel();

		if (!btn_NextLevel.activeSelf)
		{
			btn_NextLevel.SetActive(true);
		}

		if (ui_LevelScroll.HasReachedFirstLevelIndex())
		{
			btn_PreviousLevel.SetActive(false);
		}
	}
}
