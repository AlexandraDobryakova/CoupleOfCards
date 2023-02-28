using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Button = UnityEngine.UI.Button;

public class MenuWindow : MonoBehaviour
{
    public static MenuWindow Instance;
    [SerializeField] private SettingsWindow settingsWindow;
    [SerializeField] private GameObject buttonsContainer;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button startButton;
    [SerializeField] private Button exitButton;
    [SerializeField] private TextMeshProUGUI bestScoreText;
    [SerializeField] private GameObject titleText;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }

    private void Start()
    {
        StartAnimation();
        startButton.onClick.AddListener(StartGame);
        exitButton.onClick.AddListener(ExitGame);
        settingsButton.onClick.AddListener(Settings);
        UpdateBestScore();
    }


    private void StartAnimation()
    {
        titleText.SetActive(false);
        var titleTextScale = titleText.transform.localScale;
        titleText.transform.localScale = new Vector3(0, 0, 0);
        titleText.SetActive(true);
        titleText.transform.DOScale(titleTextScale, 1);

        var buttonsContainerPosition = buttonsContainer.transform.localPosition;
        buttonsContainer.transform.localPosition =
            new Vector3(-1000, buttonsContainerPosition.y, buttonsContainerPosition.z);
        buttonsContainer.transform.DOLocalMove(buttonsContainerPosition, 1);

        var scoreTextPosition = bestScoreText.transform.localPosition;
        bestScoreText.transform.localPosition = new Vector3(-1000, scoreTextPosition.y, scoreTextPosition.z);
        bestScoreText.transform.DOLocalMove(scoreTextPosition, 1);
    }

    private void OnDestroy()
    {
        startButton.onClick.RemoveListener(StartGame);
        exitButton.onClick.RemoveListener(ExitGame);
        settingsButton.onClick.RemoveListener(Settings);
    }

    private void StartGame()
    {
        GameManager.Instance.PlayClickOnButtonSound();
        SceneManager.LoadScene(GameManager.Instance.CurrentLevelIndex == SceneManager.sceneCountInBuildSettings - 1
            ? 1
            : GameManager.Instance.CurrentLevelIndex);
    }

    public void ShowMe()
    {
        gameObject.SetActive(true);
    }

    private void HideMe()
    {
        gameObject.SetActive(false);
    }

    private void Settings()
    {
        GameManager.Instance.PlayClickOnButtonSound();
        settingsWindow.ShowMe();
        HideMe();
    }

    public void ResetGame()
    {
        GameManager.Instance.PlayClickOnButtonSound();
        PlayerPrefs.DeleteAll();
        UpdateBestScore();
    }

    private void UpdateBestScore()
    {
        bestScoreText.text = "Best score: " + GameManager.Instance.BestScore + "s";
    }

    private void ExitGame()
    {
        GameManager.Instance.PlayClickOnButtonSound();
        Application.Quit();
        Debug.Log("Exit");
    }
}