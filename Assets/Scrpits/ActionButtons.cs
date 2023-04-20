using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionButtons : MonoBehaviour
{
    public void StartGame()
    {
        GameManager.Instance.UpdateGameState(GameState.ChooseType);
    }

    public void ShowHowTo()
    {
        GameManager.Instance.UpdateGameState(GameState.ShowSettings);
    }

    public void Close()
    {
        Application.Quit();
    }

    public void Cancel()
    {
        GameManager.Instance.UpdateGameState(GameState.MainMenu);
    }

    public void Chase()
    {
        GameManager.Instance.UpdateGameState(GameState.Follow);
        Time.timeScale = 1;
    }

    public void Flee()
    {
        GameManager.Instance.UpdateGameState(GameState.RunAway);
        Time.timeScale = 1;
    }

    public void ChaseNear()
    {
        GameManager.Instance.UpdateGameState(GameState.IdleChase);
        Time.timeScale = 1;
    }
}
