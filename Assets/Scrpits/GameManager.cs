using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public GameState State;
    [SerializeField] private SetUpMenu setUpMenu;

    public static event UnityAction<GameState> OnStateChange;
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        UpdateGameState(GameState.MainMenu);
    }

    public void UpdateGameState(GameState newState)
    {
        State = newState;
        if (newState != GameState.Follow || newState != GameState.RunAway || newState != GameState.IdleChase)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
/*
        switch (newState != GameState.Follow || GameState.RunAway || GameState.IdleChase)
        {
            case GameState.MainMenu:
                Time.timeScale = 0;
                break;
            case GameState.ChooseType:
                Time.timeScale = 0;
                break;
            case GameState.ShowSettings:
                Time.timeScale = 0;
                break;
            case GameState.Follow:
                break;
            case GameState.RunAway:
                break;
            case GameState.IdleChase:
                break;
            case GameState.PauseMenu:
                break;
            case GameState.GameOver:
                break;
            default:
                UpdateGameState(GameState.MainMenu);
                break;
        }*/
        OnStateChange?.Invoke(newState);
    }

}
public enum GameState
{
    MainMenu,
    ShowSettings,
    ChooseType,
    Follow,
    RunAway,
    IdleChase,
    GameOver,
    PauseMenu
}