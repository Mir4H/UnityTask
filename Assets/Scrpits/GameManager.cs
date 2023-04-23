using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public GameState State;
    public static event UnityAction<GameState> OnStateChange;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if(Input.GetKeyUp(KeyCode.Escape)) 
        {
            Application.Quit();
        }
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
}