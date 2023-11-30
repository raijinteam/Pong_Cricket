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

	// SCRIPTS
	[field : SerializeField]public HomeScreenUI ui_HomeScreen { get;private set; }
	[field : SerializeField]public PlayerSelectionUI ui_PlayerSelection { get;private set; }
	[field : SerializeField]public AbilitiesUI ui_Abilities { get; private set; }
	[field : SerializeField]public LevelSelectionUI ui_LevelSelection { get;private set; }
	[field : SerializeField] public GameScreenUI ui_GameScreen { get; private set; }
	[field : SerializeField] public GameOverUI ui_GameOver { get; private set; }

	

	// TEMPORARY //
	private void Start()
	{
		Invoke("ActivateHomeScreenAfter1Second", 0.5f);
	}

	private void ActivateHomeScreenAfter1Second()
	{
		ui_HomeScreen.gameObject.SetActive(true);
		//ui_PlayerSelection.gameObject.SetActive(true);
		//ui_Abilities.gameObject.SetActive(true);
	}
}
