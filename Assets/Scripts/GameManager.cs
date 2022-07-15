using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //Set Up Singleton
    private static GameManager _instance;
    public static GameManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameManager>();
                if (_instance == null)
                {
                    GameObject obj = new GameObject("GameManager");
                    _instance = obj.AddComponent<GameManager>();
                }
            }
            return _instance;
        }
    }
    //Reference to the PlayerController
    public PlayerController playerController { get; private set; }

    //Set Up Events
    public delegate void GameStarted();
    public event GameStarted GameStartedEvent;
    public delegate void GameOver();
    public event GameOver GameOverEvent;
    public delegate void PauseGame(bool isPaused);
    public event PauseGame PauseGameEvent;
    public delegate void LoopChanged(int value);
    public event LoopChanged LoopChangedEvent;
    public delegate void HealthChanged(int value);

    public event HealthChanged HealthChangedEvent;

    //Set Up Callbacks
    //Start Game
    public void StartGame()
    {
        if (GameStartedEvent != null)
        {
            GameStartedEvent();
        }
    }
    //Initiate GameOver
    public void GameOverSequence()
    {
        if (GameOverEvent != null)
        {
            GameOverEvent();
        }
    }
    //Pause Game
    public void GamePaused(bool isPaused)
    {
        if (PauseGameEvent != null)
        {
            PauseGameEvent(isPaused);
        }
    }
    //Update Loop Amount
    public void UpdateLoops(int amount)
    {
        if (LoopChangedEvent != null)
        {
            LoopChangedEvent(amount);
        }
    }
    //Update Health
    public void UpdateHealth(int health)
    {
        if (HealthChangedEvent != null)
        {
            HealthChangedEvent(health);
        }
    }

    //Set the player controller
    public void SetPlayerController(PlayerController controller)
    {
        playerController = controller;
    }
    //Application Methods
    //Restart Game
    public void RestartGame()
    {
        //Reload the Scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    //Quit Game
    public void QuitGame()
    {
        Application.Quit();
    }
}
