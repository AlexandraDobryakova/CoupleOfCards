using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseWindow : MonoBehaviour
{
    [SerializeField] private float showTimeAnim = 1f;
    [SerializeField] private float hideTimeAnim = 1f;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private Button continueGameButton;
    [SerializeField] private Button goToMenuButton;

    private Tween _animTween;

    private void Start()
    {
        continueGameButton.onClick.AddListener(ContinueGame);
        goToMenuButton.onClick.AddListener(GoToMenu);
    }

    private void OnDestroy()
    {
        _animTween?.Kill();
        continueGameButton.onClick.RemoveListener(ContinueGame);
        goToMenuButton.onClick.RemoveListener(GoToMenu);
    }

    public void ShowMe()
    {
        _animTween?.Kill();
        canvasGroup.alpha = 0;
        gameObject.SetActive(true);
        _animTween = canvasGroup.DOFade(1, showTimeAnim);
    }

    public void HideMe(Action onComplete = null)
    {
        _animTween?.Kill();
        _animTween = canvasGroup.DOFade(0, hideTimeAnim).OnComplete(() => gameObject.SetActive(false));
        _animTween.OnComplete(() =>
        {
            gameObject.SetActive(false);
            onComplete?.Invoke();
        });
    }

    private void ContinueGame()
    {GameManager.Instance.PlayClickOnButtonSound();
        GameManager.Instance.ContinueGame();
    }

    private void GoToMenu()
    {GameManager.Instance.PlayClickOnButtonSound();
        SceneManager.LoadScene(0);
    }
}