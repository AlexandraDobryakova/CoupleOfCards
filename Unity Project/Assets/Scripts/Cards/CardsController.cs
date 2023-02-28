using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class CardsController : MonoBehaviour
{
    public event Action OnHintShow;

    public int CountOfCards { get; private set; }
    [field: SerializeField] public float CardStartAnimationTime { get; private set; } = 0.3f;

    [SerializeField] private float intervalForHint;
    [SerializeField] private GameObject containerForCards;

    private List<Card> _cards;

    private void Awake()
    {
        var cards = containerForCards.GetComponentsInChildren<Card>();
        CountOfCards = cards.Length;
        _cards = cards.ToList();
    }

    public void DestroyCards(Card firstCard, Card secondCard)
    {
        _cards.Remove(firstCard);
        _cards.Remove(secondCard);

        firstCard.DestroyMe();
        secondCard.DestroyMe();
    }

    public void ChangeCountOfCards()
    {
        CountOfCards -= 2;
    }

    public void TurnAllCardsToBack(bool ignoreChosenCard = false)
    {
        foreach (var card in _cards.Where(card => !ignoreChosenCard || !card.IsChosen).Where(card => !card.IsRemoving))
        {
            card.StartBack(CardStartAnimationTime);
        }
    }

    public void TurnAllCardsToFront()
    {
        foreach (var card in _cards)
        {
            if (card.IsChosen && card.IsRemoving == false)
                card.StartFront(CardStartAnimationTime);

            if (card.IsRemoving) continue;
            card.StartFront(CardStartAnimationTime);
        }
    }

    public void ShowCardsAsHint(Action onStart = null, Action onComplete = null)
    {
        OnHintShow?.Invoke();
        onStart?.Invoke();

        GameManager.Instance.LevelManager.LockControl();
        GameManager.Instance.MinusCoins();
        TurnAllCardsToFront();

        var sequence = DOTween.Sequence();
        sequence.AppendInterval(intervalForHint);
        sequence.AppendCallback(() => TurnAllCardsToBack(true));
        sequence.AppendInterval(CardStartAnimationTime);
        sequence.AppendCallback(() =>
        {
            GameManager.Instance.LevelManager.UnlockControl();
            onComplete?.Invoke();
        });
    }
}