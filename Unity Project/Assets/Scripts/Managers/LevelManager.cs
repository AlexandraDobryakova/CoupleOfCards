using DG.Tweening;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public CardsController cardsController;
    public bool IsLevelComplete { get; private set; }

    [Header("Windows")] [SerializeField] private PauseWindow pauseWindow;
    [SerializeField] private GameWindow gameWindow;
    [SerializeField] private WinWindow winWindow;
    [SerializeField] private FailWindow failWindow;

    [Header("Other settings")] [SerializeField]
    private float intervalForWaitingToResetCards = 0.5f;

    [SerializeField] private float startIntervalForCards = 1f;
    [SerializeField] private GameObject wrongCoupleCardsSound;
    [SerializeField] private GameObject rightCoupleCardsSound;

    private bool IsControlLocked => _controlLocksCount > 0;
    private Card _firstChosenCard;
    private Card _secondChosenCard;
    private Vector2 _mousePosition;
    private Sequence _wrongCoupleSequence;
    private Sequence _rightCoupleSequence;
    private Camera _camera;
    private int _controlLocksCount;
    private int _animationsCount;

    private void Awake()
    {
        _camera = Camera.main;
    }

    private void Start()
    {
        GameManager.Instance.OnLevelStart += OnLevelStart;
        GameManager.Instance.OnLevelComplete += OnLevelComplete;
        GameManager.Instance.OnPauseStateChanged += OnPauseStateChanged;
        GameManager.Instance.OnLevelFail += OnLevelFail;
        cardsController.OnHintShow += OnHintShow;

        GameManager.Instance.LevelStart();
    }

    private void Update()
    {
        if (IsControlLocked == false)
        {
            if (Input.GetMouseButtonDown(0))
            {
                _mousePosition = _camera.ScreenToWorldPoint(Input.mousePosition);
                ChoseCard();
            }

            if (_firstChosenCard != null && _secondChosenCard != null)
            {
                CheckCouple(_firstChosenCard, _secondChosenCard);

                _firstChosenCard = null;
                _secondChosenCard = null;
            }
        }
    }

    private void OnDestroy()
    {
        _wrongCoupleSequence?.Kill();
        _rightCoupleSequence?.Kill();

        GameManager.Instance.OnLevelStart -= OnLevelStart;
        GameManager.Instance.OnLevelComplete -= OnLevelComplete;
        GameManager.Instance.OnPauseStateChanged -= OnPauseStateChanged;
        GameManager.Instance.OnLevelFail -= OnLevelFail;
        cardsController.OnHintShow -= OnHintShow;
    }

    private void OnHintShow()
    {
        _wrongCoupleSequence?.Kill(true);
    }

    private void OnPauseStateChanged(bool state)
    {
        if (state)
        {
            LockControl();
            GameManager.Instance.CoroutineTimer.StopMe();
            pauseWindow.ShowMe();
        }
        else
        {
            pauseWindow.HideMe(() =>
            {
                UnlockControl();
                GameManager.Instance.CoroutineTimer.RunMe();
            });
        }
    }

    private void OnLevelFail()
    {
        failWindow.ShowMe();
        gameWindow.HideMe();
        GameManager.Instance.CoroutineTimer.HideText();
        cardsController.gameObject.SetActive(false);
    }

    private void OnLevelStart()
    {
        var mySequence = DOTween.Sequence();
        mySequence.OnStart(LockControl);
        mySequence.AppendInterval(startIntervalForCards);
        mySequence.AppendCallback(() =>
        {
            cardsController.TurnAllCardsToBack();
            GameManager.Instance.CoroutineTimer.RunMe();
            GameManager.Instance.StartGame();
        });
        mySequence.AppendInterval(cardsController.CardStartAnimationTime);
        mySequence.AppendCallback(UnlockControl);
    }

    private void OnLevelComplete()
    {
        GameManager.Instance.CoroutineTimer.StopMe();
        GameManager.Instance.CoroutineTimer.HideText();
        gameWindow.HideMe();
        winWindow.ShowMe();
    }

    private void ChoseCard()
    {
        if (_firstChosenCard == null)
        {
            _firstChosenCard = MainRaycastForCards();
        }
        else if (_secondChosenCard == null)
        {
            _secondChosenCard = MainRaycastForCards();
        }
    }

    private Card MainRaycastForCards()
    {
        var hit = Physics2D.Raycast(_mousePosition, Vector2.zero);
        if (hit.collider != null)
        {
            if (hit.collider.gameObject.TryGetComponent<Card>(out var chosenCard))
            {
                if (chosenCard.IsChosen == false)
                {
                    chosenCard.ChooseMe();
                    return chosenCard;
                }
            }
        }

        return null;
    }

    private void CheckCouple(Card firstCard, Card secondCard)
    {
        if (firstCard.GetTypeOfCard() == secondCard.GetTypeOfCard())
            FindCouple(firstCard, secondCard);
        else
            FindWrongCouple(firstCard, secondCard);
    }

    private void FindCouple(Card firstCard, Card secondCard)
    {
        _animationsCount++;
        SoundManager.Instance.PlaySound(rightCoupleCardsSound);
        cardsController.ChangeCountOfCards();
        CheckForWin();

        firstCard.SetIsRemovingFlag(true);
        secondCard.SetIsRemovingFlag(true);

        _rightCoupleSequence = DOTween.Sequence();
        _rightCoupleSequence.AppendInterval(intervalForWaitingToResetCards);
        _rightCoupleSequence.OnComplete(() =>
        {
            _animationsCount--;
            cardsController.DestroyCards(firstCard, secondCard);

            if (IsLevelComplete && _animationsCount == 0)
            {
                GameManager.Instance.LevelComplete();
            }
        });
    }

    private void FindWrongCouple(Card firstCard, Card secondCard)
    {
        SoundManager.Instance.PlaySound(wrongCoupleCardsSound);
        _wrongCoupleSequence = DOTween.Sequence();
        _wrongCoupleSequence.AppendInterval(intervalForWaitingToResetCards);
        _wrongCoupleSequence.OnComplete(() =>
        {
            firstCard.UnChooseMe();
            secondCard.UnChooseMe();
        });
    }

    public void LockControl()
    {
        _controlLocksCount++;
    }

    public void UnlockControl()
    {
        if (_controlLocksCount == 0) return;

        _controlLocksCount--;
    }

    private void CheckForWin()
    {
        if (cardsController.CountOfCards == 0)
        {
            IsLevelComplete = true;
        }
    }
}