using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LevelScrollSnapSystem : MonoBehaviour, IEndDragHandler
{
	[SerializeField] private ScrollRect scroll_Level;
	[SerializeField] private RectTransform contentPanel;
	[SerializeField] private HorizontalLayoutGroup hlg;
	[SerializeField] private float snapForce;
	private RectTransform sampleItem;
	private int maxLevelCount;

	private float currentSnapSpeed;
	private bool isSnapping = false;
	private int currentItemIndex;

	private void Start()
	{
		maxLevelCount = contentPanel.childCount;
	}

	private void Update()
	{		
		if (!isSnapping)
		{
			return;
		}

		scroll_Level.velocity = Vector2.zero;
		currentSnapSpeed += snapForce * Time.deltaTime;
		contentPanel.localPosition = new Vector3(Mathf.MoveTowards(contentPanel.localPosition.x, 0 - (currentItemIndex * (sampleItem.rect.width + hlg.spacing)), currentSnapSpeed),
												contentPanel.localPosition.y, contentPanel.localPosition.z);

		if (contentPanel.localPosition.x == 0 - (currentItemIndex * (sampleItem.rect.width + hlg.spacing)))
		{
			isSnapping = false;
		}
	}

	public void SetSampleItem(RectTransform _sample)
	{
		sampleItem = _sample;
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		currentSnapSpeed = 0;
		currentItemIndex = Mathf.RoundToInt(0 - contentPanel.localPosition.x / (sampleItem.rect.width + hlg.spacing));
		isSnapping = true;

		UIManager.Instance.ui_LevelSelection.HandleNextAndPreviousButton();
	}

	public void SwitchToNextLevel()
	{
		currentSnapSpeed = 0;
		currentItemIndex += 1;
		isSnapping = true;
	}

	public void SwitchToPreviousLevel()
	{
		currentSnapSpeed = 0;
		currentItemIndex -= 1;
		isSnapping = true;
	}

	public bool HasReachedFirstLevelIndex()
	{
		if(currentItemIndex == 0)
		{
			return true;
		}

		return false;
	}

	public bool HasReachedMaxLevelIndex()
	{
		if(currentItemIndex == (maxLevelCount - 1))
		{
			return true;
		}

		return false;
	}
}