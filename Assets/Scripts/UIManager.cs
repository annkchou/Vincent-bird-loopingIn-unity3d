using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class UIManager : MonoBehaviour
{
    //UI Elements
    //Loop TextMeshPro
    [SerializeField] private TextMeshProUGUI loopText;
    [SerializeField] private TextMeshProUGUI loopToWinText;
   // [SerializeField] private TextMeshProUGUI LoopTextFinal;
    //Default Loop Text
    [SerializeField] private string defaultLoopText;
    [SerializeField] private string defaultToWinText;
    //Health Slider
    [SerializeField] private Slider healthSlider;
    //Start Menu
    //StartButton
    [SerializeField] private Button startButton;
    //ExitButton
    [SerializeField] private Button exitButton;
    //GameOver Menu
    //RestartButton
    [SerializeField] private Button restartButton;
    //GameOverExitButton
    [SerializeField] private Button gameOverExitButton;
    //StartMenu Panel
    //Resume Button
    [SerializeField] private Button resumeButton;
    //GameOverExitButton
    [SerializeField] private Button pauseExitButton;
    //Pause Menu
    [SerializeField] private GameObject startMenuPanel;
    //GameOver panel
    [SerializeField] private GameObject gameOverPanel;
    //GameOver panel
    [SerializeField] private GameObject pausePanel;
    //private GameManager.LoopChanged UpdateLoopText;

    //Set Up Events
    public delegate void AudioChange(SoundController.Channel channel, float volume,
   bool isMuted);
    public static event AudioChange AudioChangeEvent;

    public void UpdateAudio(SoundController.Channel channel, float value, bool isOn)
    {
        if (AudioChangeEvent != null)
        {
            AudioChangeEvent(channel, value, !isOn);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
       
        //if (gameOverPanel != null)
        //{
        //    //Deactivate GameOver Panel
            gameOverPanel.SetActive(false);
        //}

        //if (startMenuPanel != null)
        //{
            //Activate the StartMenu Panel
            startMenuPanel.SetActive(true);
        //}

        //if (pausePanel != null)
        //{
            //Deactivate the Pause Panel
            pausePanel.SetActive(false);
        //}

        //set the loop text to 0
        loopText.text = defaultLoopText + 0;
        loopToWinText.text = defaultToWinText;
        //Setup Button Behaviours
        startButton.onClick.AddListener(StartGame);
        exitButton.onClick.AddListener(ExitGame);

        pauseExitButton.onClick.AddListener(ExitGame);
        resumeButton.onClick.AddListener(Resume);

        gameOverExitButton.onClick.AddListener(ExitGame);
        restartButton.onClick.AddListener(RestartGame);

        //remember to uncomment line 73089
        //Subscribe to the Gamemanager's LoopChanged event
        GameManager.instance.LoopChangedEvent += UpdateLoopText;
        //Subscribe to the Gamemanager's HealthChanged event
        GameManager.instance.HealthChangedEvent += UpdateHealthSlider;
        //Subscribe to the Gamemanager's GameOver event
        GameManager.instance.GameOverEvent += GameOver;
        //Subscribe to the Gamemanager's Pause event
        GameManager.instance.PauseGameEvent += OnPause;

    }
    //Button Actions
    //Start Game
    private void StartGame()
    {  // problem : buttons on mystartMenu is not reacting to my mouse click, and the panel stays on 
        //Lock the cursor
        Cursor.lockState = CursorLockMode.Locked;
        //Deactivate the StartMenu Panel
        startMenuPanel.SetActive(false);
        //Activate the GameManager
        GameManager.instance.StartGame();
        //Set the Max Health
        healthSlider.maxValue = 100;
        ///      GameManager.instance.playerController.GetHealth().GetMaxHealth();
        //Set the health value
        healthSlider.value = 100;
  //     GameManager.instance.playerController.GetHealth().GetCurrentHealth();
    }
    //Exit Game
    private void ExitGame()
    {
        GameManager.instance.QuitGame();
    }
    //Restart Game
    private void RestartGame()
    {
        GameManager.instance.RestartGame();
    }
    //ResumeGame
    private void Resume()
    {
        GameManager.instance.GamePaused(false);
    }
    //Update the Loop Text
    private void UpdateLoopText(int loopAmount)
    {
        loopText.text = defaultLoopText + loopAmount;
    //    loopTextFinal.text = loopText.text;
    }
    //private void UpdateCoinText(int coinAmount)
    //{
    //    coinText.text = defaultCoinText + coinAmount;
    //}
    //Update the Health Slider
    private void UpdateHealthSlider(int healthNormalized)
    {
        healthSlider.value = healthNormalized;
    }

    //Game Over Sequence
    private void GameOver()
    {
        //Release the mouse
        Cursor.lockState = CursorLockMode.None;
        //Activate the GameOver Panel
        gameOverPanel.SetActive(true);
    }
    //Pause Screen
    private void OnPause(bool isPaused)
    {
        if (isPaused)
        {
            //Release the mouse
            Cursor.lockState = CursorLockMode.None;
            //Activate the GameOver Panel
            pausePanel.SetActive(true);
        }
        else
        {
            //Release the mouse
            Cursor.lockState = CursorLockMode.Locked;
            //Activate the GameOver Panel
            pausePanel.SetActive(false);
        }
    }
}
