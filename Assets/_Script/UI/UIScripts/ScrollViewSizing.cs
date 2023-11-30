using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollViewSizing : MonoBehaviour
{

    private RectTransform rectTransform;


    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

	private void Start()
	{
		float aspectRatio = (float)Screen.width / Screen.height;

		if (aspectRatio >= 0.7f)
		{
			rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, 1100);
		}
		else if (aspectRatio >= 0.6f)
		{
			rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, 1900);
		}
		else if(aspectRatio >= 0.55f)
		{
			rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, 2150);
		}
		else if(aspectRatio >= 0.48f)
		{
			rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, 2650);
		}
		else
		{
			rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, 2900);
		}
	}

	//private void Start()
	//{
	//    // Calculate the height for the ScrollView to fit to the bottom edge of the screen
	//    float scrollViewHeight = Screen.height - 2000;

	//    // Set the height of the ScrollView
	//    rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, scrollViewHeight);
	//}

	private void Update()
	{
		
	}
}
