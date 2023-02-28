using System.Collections;
using TMPro;
using UnityEngine;

public class CoroutineTimer : MonoBehaviour
{
    [SerializeField] private float time;
    [SerializeField] private TextMeshProUGUI timerText;

    private float _timeLeft = 0f;
    private bool _isActive;

    private void Awake()
    {
        _timeLeft = time;
        UpdateTimeText();
    }

    public void RunMe()
    {
        if (_isActive) return;
        _isActive = true;

        StartCoroutine(StartTimer());
    }

    public void StopMe()
    {
        if (_isActive == false) return;
        _isActive = false;

        StopCoroutine(StartTimer());
    }

    private IEnumerator StartTimer()
    {
        while (_timeLeft >= 0 && _isActive)
        {
            UpdateTimeText();
            _timeLeft -= Time.deltaTime;

            yield return null;
        }
    }

    private void UpdateTimeText()
    {
        if (_timeLeft < 1 && GameManager.Instance.LevelManager.IsLevelComplete == false)
        {
            _timeLeft = 0;
            TimeOver();
        }

        float minutes = Mathf.FloorToInt(_timeLeft / 60);
        float seconds = Mathf.FloorToInt(_timeLeft % 60);
        timerText.text = $"{minutes:00}:{seconds:00}";
    }

    public void HideText()
    {
        timerText.gameObject.SetActive(false);
    }

    private void TimeOver()
    {
        GameManager.Instance.LevelFail();
    }

    public int GetTime()
    {
        return (int) (time - _timeLeft);
    }
}