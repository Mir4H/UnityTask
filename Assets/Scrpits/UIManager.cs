using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject mainmenu;
    [SerializeField] private TextMeshProUGUI menuText;
    [SerializeField] private GameObject info;
    [SerializeField] private GameObject howTo;
    [SerializeField] private GameObject toMain;
    [SerializeField] private GameObject chooseType;
  //  [SerializeField] private Button newGame;


    // [SerializeField] private Button QuitBtn;
    // [SerializeField] private GameObject pauseMenu;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
       //newGame.onClick.AddListener(StartGame);
    }

    private void OnEnable()
    {
        GameManager.OnStateChange += OnGameStateChange;
    }

    private void OnGameStateChange(GameState state)
    {
        mainmenu.SetActive(state == GameState.MainMenu || state == GameState.PauseMenu || state == GameState.GameOver);
        howTo.SetActive(state == GameState.ShowSettings);
        chooseType.SetActive(state == GameState.ChooseType);
        if (state == GameState.MainMenu) 
        {
            info.SetActive(false);
            toMain.SetActive(false);
        }

        if (state == GameState.PauseMenu)
        {
            menuText.text = "Paused";
            info.SetActive(false);
            howTo.SetActive(false);
        }

        if (state == GameState.GameOver)
        {
            menuText.text = "Game Over";
            howTo.SetActive(false);
        }
    }

    private void OnDisable()
    {
        GameManager.OnStateChange -= OnGameStateChange;
    }
}
