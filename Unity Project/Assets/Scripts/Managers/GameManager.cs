using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public event Action OnLevelComplete;
    public event Action OnLevelStart;
    public event Action OnLevelFail;
    public event Action<bool> OnPauseStateChanged;

    public CoinsManager CoinsManager => coinsManager;
    public bool IsGameStarted { get; private set; }

    public int CurrentLevelIndex
    {
        get => PlayerPrefs.GetInt("CurrentLevel", 1);
        private set => PlayerPrefs.SetInt("CurrentLevel", SceneManager.GetActiveScene().buildIndex);
    }

    public int BestScore
    {
        get => PlayerPrefs.GetInt("BestScore", 0);
        private set => PlayerPrefs.SetInt("BestScore", value);
    }

    public int CurrentScore
    {
        get => PlayerPrefs.GetInt("CurrentScore", 0);
        private set => PlayerPrefs.SetInt("CurrentScore", value);
    }

    public bool IsGameOnPause { get; private set; }

    [field: SerializeField] public LevelManager LevelManager { get; private set; }
    [field: SerializeField] public CoroutineTimer CoroutineTimer { get; private set; }

    [SerializeField] private GameObject clickOnButtonSound;
    [SerializeField] private CoinsManager coinsManager;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }

    public void LevelStart()
    {
        CurrentLevelIndex = SceneManager.GetActiveScene().buildIndex;

        OnLevelStart?.Invoke();
    }

    public void LevelComplete()
    {
        CoroutineTimer.StopMe();
        CoroutineTimer.HideText();

        OnLevelComplete?.Invoke();
    }

    public void StartGame()
    {
        IsGameStarted = true;
    }

    public void LevelFail()
    {
        OnLevelFail?.Invoke();
    }

    public void PlayClickOnButtonSound()
    {
        SoundManager.Instance.PlaySound(clickOnButtonSound);
    }

    public void RestartLevel()
    {
        LoadCurrentLevel();
    }

    private void LoadCurrentLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void PauseGame()
    {
        IsGameOnPause = true;
        OnPauseStateChanged?.Invoke(IsGameOnPause);
    }

    public void UpdateBestScore()
    {
        BestScore = CurrentScore;
        CurrentScore = 0;
    }

    public void UpdateCurrentScore()
    {
        CurrentScore += GetTime();
    }

    public void ContinueGame()
    {
        IsGameOnPause = false;
        OnPauseStateChanged?.Invoke(IsGameOnPause);
    }

    public int GetCostOfPssLevel()
    {
        return coinsManager.GetCostOfPssLevel();
    }

    public void AddCoins()
    {
        coinsManager.AddCoinsForPassLevel();
    }

    public void MinusCoins()
    {
        coinsManager.TryToSubtractCoinsForHint();
    }

    public bool HasCoinsForHint()
    {
        return coinsManager.HasCoinsForHint();
    }

    private int GetTime()
    {
        return CoroutineTimer.GetTime();
    }
}