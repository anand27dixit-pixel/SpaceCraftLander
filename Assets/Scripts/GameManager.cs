using System;
using System.Collections.Generic;
using UnityEngine;

public enum STATE
{
    WAITING_TO_START,
    NORMAL,
    GAMEOVER
}
public class GameManager : MonoBehaviour
{
    // Singleton
    public static GameManager Instance { get; private set; }
    private static int currentLevelNumber = 1;
    private static int totalScore = 0;

    [SerializeField] private int score;
    [SerializeField] private List<GameLevel> gameLevels;
    private float elapsedTime;
    private bool isTimerActive;

    // Events
    public event EventHandler evGamePause;
    public event EventHandler evGameResume;

    public bool isDebugEnabled = false;

    void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
         if (isDebugEnabled)
            currentLevelNumber = 3;

        LoadGameLevel();
        Lander.Instance.OnCoinPickup += OnCoinPickedUp;
        Lander.Instance.OnLanded += OnSafelyLandedOnPad;
        Lander.Instance.OnStateChanged += OnStateChanged;

        GameInput.Instance.OnMenuButtonPressed += GameInput_OnMenuButtonPressed;
       
    }

    private void GameInput_OnMenuButtonPressed(object sender, EventArgs e)
    {
        OnGamePauseUnPause();
    }


    void Update()
    {
        if (isTimerActive)
            elapsedTime += Time.deltaTime;
    }

    private void OnStateChanged(object sender, Lander.StateEventArgs e)
    {
        if (e.stateChanged == STATE.NORMAL)
        {
            isTimerActive = true;
            CinemachineCameraZoom2D.Instance.SetCinemachineCameraTrackingTartget(Lander.Instance.transform);
            CinemachineCameraZoom2D.Instance.SetNormalOrthographicSizew();
        }

    }

    private void OnSafelyLandedOnPad(object sender, Lander.LandedEventArgs e)
    {
        Debug.Log("Recieved Score on Landed " + e.score);
        AddScore(e.score);
    }

    private void OnCoinPickedUp(object sender, EventArgs e)
    {
        AddScore(500);
    }

    private void AddScore(int _score)
    {
        score += _score;
    }

    private void LoadGameLevel()
    {
        GameLevel gameLevel = GetGameLevel();

        if (gameLevel != null)
        {
            var _gameLevel = Instantiate(gameLevel, transform.position, Quaternion.identity);
            CinemachineCameraZoom2D.Instance.SetCinemachineCameraTrackingTartget(_gameLevel.GetCineMachineCamTarget());
            CinemachineCameraZoom2D.Instance.SetTargetOrthographicSizew(_gameLevel.GetZoomedOutOrthographicZie());
            _gameLevel.SetLanderStartPosition();
        }
        else
        {
            SceneLoader.LoadScene(GAME_SCENE.GameOver.ToString());
        }
    }

    private void OnGamePauseUnPause()
    {
        if (Time.timeScale == 0f)
        {
            evGameResume?.Invoke(this, EventArgs.Empty);
            OnGameUnPause();
        }
        else
        {
            evGamePause?.Invoke(this, EventArgs.Empty);
            OnPauseGame();
        }

    }

    private GameLevel GetGameLevel()
    {
        foreach (GameLevel gameLevel in gameLevels)
        {
            Debug.Log("Inside Load Level Loop " + currentLevelNumber);
            if (currentLevelNumber == gameLevel.GetLevelNumber())
            {
                return gameLevel;
            }
        }

        return null;
    }

    #region PUBLIC_METHODS
    public int GetScore() => score;

    public float GetTime() => elapsedTime;

    public int GetCurrentLevel() => currentLevelNumber;

    public int GetTotatlScore() => totalScore;

    public void OnGoToNextLevel()
    {
        currentLevelNumber++;
        totalScore += score;
        SceneLoader.LoadScene(GAME_SCENE.Game.ToString());
    }

    public void OnRetryLevel()
    {
        SceneLoader.LoadScene(GAME_SCENE.Game.ToString());
    }

    public void OnGameUnPause()
    {
        Time.timeScale = 1f;
    }

    public void OnPauseGame()
    {
        Time.timeScale = 0f;
    }

    public static void ResetGameStatics()
    {
        currentLevelNumber = 1;
        totalScore = 0;
    }

    #endregion // PUBLIC_METHODS
}
