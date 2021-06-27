using System.Collections;
using System.Collections.Generic;
using TowerDefense.Control;
using UnityEngine;

namespace TowerDefense.Deck
{
    public class CardMover : MonoBehaviour
    {
        Coroutine coroutine;
        HandManager hand;

        private void Start()
        {
            hand = GetComponent<HandManager>();
        }

        // This function is only used when called from HandManager when cards are being dealt. It starts the AdjustCard coroutine and returns it.
        public Coroutine DealCard(RectTransform card, Vector3 targetRot, Vector2 targetPos, Vector3 targetScale, float speed, float duration)
        {
            return StartCoroutine(AdjustCard(card, targetRot, targetPos, targetScale, speed, duration));
        }

        /* This function also calls the AdjustCard coroutine but is called any time the cards are moving (except for when cards are being dealt).
        It will stop any previous coroutines and call for other cards to be shifted. */
        public Coroutine MoveCard(RectTransform card, Vector3 targetRot, Vector2 targetPos, Vector3 targetScale, float speed, float duration, bool shift = true)
        {
            if (coroutine != null)
            {
                StopAllCoroutines();
            }
            if (shift)
            {
                ShiftOtherCards(card, card.GetComponent<CardHandler>().isMagnified);
            }
            return coroutine = StartCoroutine(AdjustCard(card, targetRot, targetPos, targetScale, speed, duration));
        }

        // This is the dedicated coroutine for moving cards from one place to another.
        private IEnumerator AdjustCard(RectTransform rect, Vector3 targetRot, Vector2 targetPos, Vector3 targetScale, float speed, float duration)
        {
            float rotVelocity = 0f;
            Vector2 posVelocity = new Vector2(0f, 0f);
            Vector3 scaleVelocity = new Vector3(0, 0, 0);
            float passedTime = 0f;

            while (passedTime < duration)
            {
                // Will smoothly move cards from one place to another by calculating intermediate values one frame at a time.
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

        // Will iterate through cards in hand and move them away from the magnified card (or back in towards the demagnified card).
        private void ShiftOtherCards(RectTransform selectedCard, bool shiftAway)
        {
            int handSize = hand.GetHandSize();
            Vector3 rotation;
            Vector2 position;
            Vector3 scale = new Vector3(1, 1, 1);
            int cardIndex = hand.GetCardIndex(selectedCard);
            CardHandler cardHandler = selectedCard.GetComponent<CardHandler>();

            for (int i = 0; i < handSize; i++)
            {
                if (cardIndex != i)
                {
                    RectTransform cardToMove = hand.GetShiftValues(i, out rotation, out position); // Returns the correct card based on the index and gives the
                                                                                                   // position and rotation.
                                                                                                   // If we are shifting the cards away the calculated position must be adjusted.
                    if (shiftAway)
                    {
                        if (i < cardIndex) { position.x -= 100; }
                        else { position.x += 100; }
                    }
                    StartCoroutine(AdjustCard(cardToMove, rotation, position, scale, .1f, .5f));

                    if (!cardHandler.isInHand) // If the cards are shifting due to a card being discarded, it will set the new positions of the cards.
                    {
                        cardToMove.GetComponent<CardHandler>().SetProperties(Quaternion.Euler(rotation), position, i);
                    }
                }
            }
        }

        public void StopCard(Coroutine cardCoroutine)
        {
            StopCoroutine(cardCoroutine);
        }
    }
}
