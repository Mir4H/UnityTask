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
    [SerializeField] private GameObject howToBtn;
    [SerializeField] private GameObject toMain;
    [SerializeField] private GameObject chooseType;
    [SerializeField] private GameObject howToPlay;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void OnEnable()
    {
        GameManager.OnStateChange += OnGameStateChange;
    }

    private void OnGameStateChange(GameState state)
    {

        mainmenu.SetActive(state == GameState.MainMenu || state == GameState.PauseMenu || state == GameState.GameOver);
        howToPlay.SetActive(state == GameState.ShowSettings);
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
            howToBtn.SetActive(false);
        }

        if (state == GameState.GameOver)
        {
            menuText.text = "Game Over";
            info.SetActive(true);
            howToBtn.SetActive(false);
        }
    }

    private void OnDisable()
    {
        GameManager.OnStateChange -= OnGameStateChange;
    }
}
