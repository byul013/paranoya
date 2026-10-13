using UnityEngine;

public enum GameState
{
    // can add and switch between different game states when needed
    TitleScreen,
    PlayingGame,
    GameOver,
    GettingCalled,
    AnsweredThePhone
}

public class CurrentGameState : MonoBehaviour
{
    public static CurrentGameState Instance { get; private set; }

    public GameState state = GameState.TitleScreen; 

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ChangeState(GameState newState)
    {
        state = newState;
    }
}