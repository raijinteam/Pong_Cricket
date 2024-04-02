using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

	private void Awake()
	{
		Instance = this;
	}


	// GAMEOBJECTS
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

	

	
	

	
}
