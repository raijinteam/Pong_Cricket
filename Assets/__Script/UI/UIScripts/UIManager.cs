using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Facebook.Unity.FB;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

	private void Awake()
	{
		Instance = this;
	}




	// GAMEOBJECTS
	[SerializeField] private panel_PopUP pf_panelPopup;
	[SerializeField] private Transform spawn_PopUp;

	[SerializeField] private Panel_Pop_Player pf_Panel_PopUpPlayer;
	[SerializeField] private Panel_Pop_Warning pf_Panel_PopUp_Warning;
  
    [SerializeField] private RectTransform canvas;
    [field: SerializeField] public GameObject panel_MainMenu { get; private set; }
	[field: SerializeField] public CommanPanel panel_CommanMenu { get; private set; }

	

	// SCRIPTS
	[field :SerializeField] public MenuSelectionUI ui_MenuSelectionScreen { get; private set; }
	[field : SerializeField] public Panel_SerachingPlayer ui_PanelSerachingPlayer { get; private set; }
	[field : SerializeField]public HomeScreenUI ui_HomeScreen { get;private set; }
	[field : SerializeField]public PlayerSelectionUI ui_PlayerSelection { get;private set; }
	[field : SerializeField]public AbilitiesUI ui_Abilities { get; private set; }
	[field : SerializeField]public LevelSelectionUI ui_LevelSelection { get;private set; }
	[field : SerializeField] public GameScreenUI ui_GameScreen { get; private set; }
	[field : SerializeField] public GameOverUI ui_GameOver { get; private set; }
	[field : SerializeField] public ShopUI ui_ShopScreen { get; private set; }
	[field : SerializeField] public ChestopeningUI ui_ChestOpping { get; private set; }

	[field : SerializeField] public TaskProgress ui_taskProgress { get; private set; }

	[field:SerializeField] public UISettingScreen ui_SettingScrenn { get; private set; }
    [field: SerializeField] public UIGameOverPopUp UiGameOverPopUp { get; set; }


    public void spawnPopup(string Message) {

		panel_PopUP current = Instantiate(pf_panelPopup, spawn_PopUp);
		current.ActvetedPanel(2, Message);

    }

	public void SpawnPopupOfPlayer(bool v1, int v2) {
		Panel_Pop_Player currnt = Instantiate(pf_Panel_PopUpPlayer, canvas.transform);
		currnt.SpwanPopUP(v1, v2);
	}

	public void ActvetedWarningMessage(string Message) {
		Panel_Pop_Warning current = Instantiate(pf_Panel_PopUp_Warning, canvas.transform);
		current.ActvetedPopUp(Message);
	}


}
