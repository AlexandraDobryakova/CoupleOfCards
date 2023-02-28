using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class GameWindow : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private TextMeshProUGUI currentLevelText;
    [SerializeField] private Button pauseButton;
    [SerializeField] private TextMeshProUGUI coinsText;
    [SerializeField] private Button showHintButton;

    private void Awake()
    {
        ChangeLevelText();
        pauseButton.onClick.AddListener(StopGame);
        showHintButton.onClick.AddListener(TryToShowHint);
    }

    private void Start()
    {
        ChangeCoinsText();
    }

    private void OnDestroy()
    {
        pauseButton.onClick.RemoveListener(StopGame);
        showHintButton.onClick.RemoveListener(TryToShowHint);
    }

    private void TryToShowHint()
    {
        if (GameManager.Instance.IsGameStarted == false) return;
        if (GameManager.Instance.LevelManager.IsLevelComplete) return;

        if (GameManager.Instance.HasCoinsForHint())
            ShowHint();
    }

    private void ShowHint()
    {
        GameManager.Instance.PlayClickOnButtonSound();

        GameManager.Instance.LevelManager.cardsController.ShowCardsAsHint(
            () => { showHintButton.interactable = false; },
            () => showHintButton.interactable = true);

        ChangeCoinsText();
    }

    public void HideMe()
    {
        panel.SetActive(false);
    }

    private void ChangeLevelText()
    {
        var currentLevelIndex = SceneManager.GetActiveScene().buildIndex;
        var countOfLevels = SceneManager.sceneCountInBuildSettings - 1;
        currentLevelText.SetText("Level " + currentLevelIndex + "/" + countOfLevels);
    }

    private void ChangeCoinsText()
    {
        var countOfCoins = GameManager.Instance.CoinsManager.CurrentCoins;
        coinsText.text = countOfCoins + "$";
    }

    private void StopGame()
    {
        GameManager.Instance.PlayClickOnButtonSound();
        if (GameManager.Instance.LevelManager.IsLevelComplete) return;
        GameManager.Instance.PauseGame();
    }
}