using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public GameState State;
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