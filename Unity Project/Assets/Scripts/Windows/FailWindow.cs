using UnityEngine.UI;
using UnityEngine;


public class FailWindow : MonoBehaviour
{
    [SerializeField] private Button resetButton;
    [SerializeField] private GameObject gameOverSound;

    private void Awake()
    {
        resetButton.onClick.AddListener(ResetLevel);
    }

    private void OnDestroy()
    {
        resetButton.onClick.RemoveListener(ResetLevel);
    }

    public void ShowMe()
    {
        SoundManager.Instance.PlaySound(gameOverSound);
        gameObject.SetActive(true);
    }

    private void ResetLevel()
    {
        GameManager.Instance.PlayClickOnButtonSound();
        GameManager.Instance.RestartLevel();
    }
}