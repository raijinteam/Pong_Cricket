using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSelectionUI : MonoBehaviour
{
    [SerializeField] private float increaseSizeValue = 0.1f; // How much the selected button's anchor in X-axis increases
    [SerializeField] private RectTransform[] all_MenuOptions;
    [SerializeField] private GameObject[] all_SelectedMenuPanels;
    [SerializeField] private GameObject[] all_DefaultMenuPanels;
    private Vector2[] originalAnchors;
    private float yAnchorSelectedValue = 1f;
    private float yAnchorDefaultValue = 0.8f;

    private void Start()
    {

        originalAnchors = new Vector2[all_MenuOptions.Length];

        for (int i = 0; i < all_MenuOptions.Length; i++)
        {
            originalAnchors[i] = all_MenuOptions[i].anchorMax - all_MenuOptions[i].anchorMin;          
        }

        OnClick_Menu(2);
    }

    public void OnClick_Menu(int _menuIndex)
	{
        float incrementPerButton = increaseSizeValue / (all_MenuOptions.Length - 1);
        float currentMinAnchor = 0;
   
        for (int i = 0; i < all_MenuOptions.Length; i++)
        {
            float originalWidth = originalAnchors[i].x;
			if (i == _menuIndex)
			{
				all_MenuOptions[i].anchorMin = new Vector2(currentMinAnchor, all_MenuOptions[i].anchorMin.y);
				all_MenuOptions[i].anchorMax = new Vector2(currentMinAnchor + originalWidth + increaseSizeValue, yAnchorSelectedValue);
				currentMinAnchor += originalWidth + increaseSizeValue;

                all_SelectedMenuPanels[i].SetActive(true);
                all_DefaultMenuPanels[i].SetActive(false);
                if (i == 4) {
                    GameManager.Instance.StartMinigame();
                }
               
            }

			else
			{
				all_MenuOptions[i].anchorMin = new Vector2(currentMinAnchor, all_MenuOptions[i].anchorMin.y);
				all_MenuOptions[i].anchorMax = new Vector2(currentMinAnchor + originalWidth - incrementPerButton, yAnchorDefaultValue);
				currentMinAnchor += originalWidth - incrementPerButton;
                all_SelectedMenuPanels[i].SetActive(false);
                all_DefaultMenuPanels[i].SetActive(true);
            }
		}
    }
}
