using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WinWindow : MonoBehaviour
{
    [SerializeField] private float showTimeAnim = 1f;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private TextMeshProUGUI coinsText;
    [SerializeField] private Button goToMenuButton;
    [SerializeField] private Button continueButton;
    [SerializeField] private TextMeshProUGUI currentScoreText;
    [SerializeField] private TextMeshProUGUI bestScoreText;
    [SerializeField] private GameObject victorySound;

    private void Awake()
    {
        coinsText.text = "+" + GameManager.Instance.GetCostOfPssLevel() + "$";
        goToMenuButton.onClick.AddListener(GoToMenu);
        continueButton.onClick.AddListener(ContinueGame);

        var currentLevelIndex = SceneManager.GetActiveScene().buildIndex;
        var countOfLevels = SceneManager.sceneCountInBuildSettings - 1;

        ChangeSetActiveButtons(currentLevelIndex == countOfLevels);

        currentScoreText.text = "Current score: " + GameManager.Instance.CurrentScore + "s";
        bestScoreText.text = "Last best score: " + GameManager.Instance.BestScore + "s";
    }

    private void OnDestroy()
    {
        goToMenuButton.onClick.RemoveListener(GoToMenu);
        continueButton.onClick.RemoveListener(ContinueGame);
    }

    private void ChangeSetActiveButtons(bool flag)
    {
        goToMenuButton.gameObject.SetActive(flag);
        continueButton.gameObject.SetActive(!flag);
    }

    public void ShowMe()
    {
        SoundManager.Instance.PlaySound(victorySound);
        canvasGroup.alpha = 0;
        canvasGroup.DOFade(1, showTimeAnim);
        gameObject.SetActive(true);
    }

    private void ContinueGame()
    {
        GameManager.Instance.PlayClickOnButtonSound();
        GameManager.Instance.UpdateCurrentScore();
        GameManager.Instance.AddCoins();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    private void GoToMenu()
    {
        GameManager.Instance.PlayClickOnButtonSound();
        if (GameManager.Instance.BestScore == 0 ||
            GameManager.Instance.BestScore > GameManager.Instance.CurrentScore)
            GameManager.Instance.UpdateBestScore();

        GameManager.Instance.AddCoins();
        SceneManager.LoadScene(0);
    }
}