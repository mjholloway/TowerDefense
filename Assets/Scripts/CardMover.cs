using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardMover : MonoBehaviour
{
    Coroutine coroutine;
    RectTransform lastCardMoved;
    HandManager hand;

    private void Start()
    {
        hand = GetComponent<HandManager>();
    }

    public Coroutine DealCard(RectTransform card, Vector3 targetRot, Vector2 targetPos, Vector3 targetScale, float speed, float duration)
    {
        return StartCoroutine(AdjustCard(card, targetRot, targetPos, targetScale, speed, duration));
    }

    public void MoveCard(RectTransform card, Vector3 targetRot, Vector2 targetPos, Vector3 targetScale, float speed, float duration)
    {
        if (coroutine != null)
        {
            StopAllCoroutines();
        }
        ShiftOtherCards(card, card.GetComponent<CardHandler>().isMagnified);
        coroutine = StartCoroutine(AdjustCard(card, targetRot, targetPos, targetScale, speed, duration));
        lastCardMoved = card;
    }

    private IEnumerator AdjustCard(RectTransform rect, Vector3 targetRot, Vector2 targetPos, Vector3 targetScale, float speed, float duration)
    {
        float rotVelocity = 0f;
        Vector2 posVelocity = new Vector2(0f, 0f);
        Vector3 scaleVelocity = new Vector3(0, 0, 0);
        float passedTime = 0f;

        while (passedTime < duration)
        {
            float intermediateRot = Mathf.SmoothDampAngle(rect.eulerAngles.z, targetRot.z, ref rotVelocity, speed);
            rect.rotation = Quaternion.Euler(0, 0, intermediateRot);

            Vector2 intermediatePos = Vector2.SmoothDamp(rect.anchoredPosition, targetPos, ref posVelocity, speed);
            rect.anchoredPosition = intermediatePos;

            Vector3 intermediateScale = Vector3.SmoothDamp(rect.localScale, targetScale, ref scaleVelocity, speed);
            rect.localScale = intermediateScale;

            passedTime += Time.deltaTime;
            yield return null;
        }
        rect.rotation = Quaternion.Euler(targetRot);
        rect.anchoredPosition = targetPos;
        rect.localScale = targetScale;
    }

    private void ShiftOtherCards(RectTransform selectedCard, bool shiftAway)
    {
        int handSize = hand.GetHandSize();
        Vector3 rotation;
        Vector2 position;
        Vector3 scale = new Vector3(1, 1, 1);
        int cardIndex = hand.GetCardIndex(selectedCard);

        for (int i = 0; i < handSize; i++)
        {
            if (cardIndex != i)
            {
                RectTransform cardToMove = hand.GetShiftValues(i, out rotation, out position);
                if (shiftAway)
                {
                    if (i < cardIndex) { position.x -= 100; }
                    else { position.x += 100; }
                }
                StartCoroutine(AdjustCard(cardToMove, rotation, position, scale, .1f, .5f));
            }
        }
    }
}
