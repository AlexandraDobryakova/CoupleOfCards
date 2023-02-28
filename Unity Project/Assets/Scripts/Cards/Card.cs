using DG.Tweening;
using UnityEngine;

public class Card : MonoBehaviour
{
    public bool IsSelectionLocked { get; private set; }
    public bool IsChosen { get; private set; }
    public bool IsRemoving { get; private set; }

    [SerializeField] private GameObject frontSide;
    [SerializeField] private GameObject backSide;
    [SerializeField] private TypesOfCard typeOfCard;
    [SerializeField] private GameObject flipCardSound;
    [SerializeField] private float flipTime = 0.15f;

    public enum TypesOfCard
    {
        Card1,
        Card2,
        Card3
    }

    private enum CardState
    {
        Front,
        Back
    }

    private CardState _mCardState = CardState.Front;
    private Sequence _rotateSequence;

    private void Start()
    {
        Init();
    }

    private void OnDestroy()
    {
        _rotateSequence?.Kill();
    }

    private void Init()
    {
        if (_mCardState == CardState.Front)
        {
            frontSide.transform.eulerAngles = Vector3.zero;
            backSide.transform.eulerAngles = new Vector3(0, 90, 0);
        }
        else
        {
            frontSide.transform.eulerAngles = new Vector3(0, 90, 0);
            backSide.transform.eulerAngles = Vector3.zero;
        }
    }

    public void StartFront(float time)
    {
        ToFront(time);
    }

    public void StartBack(float time)
    {
        ToBack(time);
    }


    public void ToBack(float time)
    {
        var frontRotation = new Vector3(0, 90, 0);
        var backRotation = new Vector3(0, 0, 0);

        RotateTo(time, frontRotation, backRotation);
    }

    public void ToFront(float time)
    {
        var frontRotation = new Vector3(0, 0, 0);
        var backRotation = new Vector3(0, 90, 0);

        RotateTo(time, frontRotation, backRotation);
    }

    private void RotateTo(float time, Vector3 rotationForFront, Vector3 rotationForBack)
    {
        SoundManager.Instance.PlaySound(flipCardSound);
        IsSelectionLocked = true;

        _rotateSequence?.Kill(true);
        _rotateSequence = DOTween.Sequence();
        _rotateSequence.Insert(0, frontSide.transform.DORotate(rotationForFront, time));
        _rotateSequence.Insert(0, backSide.transform.DORotate(rotationForBack, time));
        _rotateSequence.OnComplete(() => { IsSelectionLocked = false; });
    }

    public void ChooseMe()
    {
        StartFront(flipTime);
        IsChosen = true;
    }

    public void UnChooseMe()
    {
        StartBack(flipTime);
        IsChosen = false;
    }

    public TypesOfCard GetTypeOfCard()
    {
        return typeOfCard;
    }

    public void DestroyMe()
    {
        Destroy(gameObject);
    }

    public void SetIsRemovingFlag(bool state)
    {
        IsRemoving = state;
    }
}