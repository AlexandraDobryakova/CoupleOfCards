using UnityEngine;
using UnityEngine.UI;

public class SettingsWindow : MonoBehaviour
{
    [SerializeField] private Button backToMenuButton;
    [SerializeField] private Button resetGameButton;
    [SerializeField] private Button muteButton;
    [SerializeField] private Button unmuteButton;

    private void Start()
    {
        MakeButtonMute(SoundManager.Instance.IsSoundsEnabled);

        backToMenuButton.onClick.AddListener(BackToMenu);
        resetGameButton.onClick.AddListener(ResetGame);
        muteButton.onClick.AddListener(Mute);
        unmuteButton.onClick.AddListener(Unmute);

        resetGameButton.gameObject.SetActive(PlayerPrefs.HasKey("Coins"));
    }

    private void OnDestroy()
    {
        backToMenuButton.onClick.RemoveListener(BackToMenu);
        resetGameButton.onClick.RemoveListener(ResetGame);
        muteButton.onClick.RemoveListener(Mute);
        unmuteButton.onClick.RemoveListener(Unmute);
    }

    public void ShowMe()
    {
        gameObject.SetActive(true);
    }

    private void HideMe()
    {
        gameObject.SetActive(false);
    }

    private void BackToMenu()
    {
        GameManager.Instance.PlayClickOnButtonSound();
        HideMe();
        MenuWindow.Instance.ShowMe();
    }

    private void ResetGame()
    {
        MakeButtonMute(true);
        GameManager.Instance.PlayClickOnButtonSound();
        resetGameButton.gameObject.SetActive(false);
        MenuWindow.Instance.ResetGame();
    }

    private void Mute()
    {
        MakeButtonMute(false);
        SoundManager.Instance.Mute();
    }

    private void Unmute()
    {
        MakeButtonMute(true);
        SoundManager.Instance.UnMute();
        GameManager.Instance.PlayClickOnButtonSound();
    }

    private void MakeButtonMute(bool flag)
    {
        muteButton.gameObject.SetActive(flag);
        unmuteButton.gameObject.SetActive(!flag);
    }
}