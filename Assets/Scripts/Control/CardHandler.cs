using UnityEngine;
using TowerDefense.Core;
using TowerDefense.Deck;
using TowerDefense.Combat;

namespace TowerDefense.Control
{
    public class CardHandler : MonoBehaviour
    {
        [SerializeField] float verticalMagnificationDisplacement = 150f;
        [SerializeField] float magnifyScaleFactor = 1.25f;
        [SerializeField] float centerScaleFactor = 1.1f;

        [SerializeField] Vector3 magnifyRotation = new Vector3(0, 0, 0);
        [SerializeField] Vector2 centerPos = new Vector2(0f, 100f);
        [SerializeField] Vector2 centerRotation = new Vector3(0, 0, 0);

        [SerializeField] CardMover mover;
        [SerializeField] DiscardManager discard;
        [SerializeField] RectTransform handRect;
        [SerializeField] CardPointerEvent cardPointerEvent;
        [SerializeField] CursorChanger cursorChanger;
        [SerializeField] CardActions cardActions;

        int frames = 0;
        Coroutine thisCardCoroutine;

        CardProperties magnifiedCard;
        CardProperties selectedCard;

        bool selectedCardTargets = false;

        private void OnEnable()
        {
            cardPointerEvent.pointerEnterEvent.AddListener(HandlePointerEnterEvent);
            cardPointerEvent.pointerExitEvent.AddListener(HandlePointerExitEvent);
        }

        private void OnDisable()
        {
            cardPointerEvent.pointerEnterEvent.RemoveListener(HandlePointerEnterEvent);
            cardPointerEvent.pointerExitEvent.RemoveListener(HandlePointerExitEvent);
        }

        void HandlePointerEnterEvent(CardProperties card)
        {
            if (!card.isMagnified) { MagnifyCard(card); }
        }

        void HandlePointerExitEvent(CardProperties card)
        {
            if (magnifiedCard != null) { DemagnifyCard(card); }
        }

        // Calls CardMover to magnify the hovered card to target position and move others away.
        private void MagnifyCard(CardProperties card)
        {
            magnifiedCard = card;
            card.isMagnified = true;
            RectTransform cardRectTransform = card.GetRectTransform();

            cardRectTransform.SetAsLastSibling();

            Vector2 magnifyPos = new Vector2(card.GetStartPosition().x, verticalMagnificationDisplacement);
            Vector3 magnifyScale = new Vector3(magnifyScaleFactor, magnifyScaleFactor, 1f);

            thisCardCoroutine = mover.MoveCard(cardRectTransform, magnifyRotation, magnifyPos, magnifyScale, .1f, .5f, card.isMagnified);
        }
        
        // Calls CardMover to reset the hand and sets the card to proper index in heirarchy.
        private void DemagnifyCard(CardProperties card)
        {
            Reset();

            card.isMagnified = false;
            RectTransform cardRectTransform = card.GetRectTransform();

            mover.MoveCard(cardRectTransform, card.GetStartRotation().eulerAngles, card.GetStartPosition(), 
                card.GetStartScale(), .1f, .5f, card.isMagnified);

            cardRectTransform.SetSiblingIndex(card.GetStartIndex());
        }

        private void Update()
        {
            if (selectedCard != null) 
            {
                if (!selectedCardTargets)
                {
                    selectedCard.transform.position = Input.mousePosition;
                }
                else { UseTargetingMode(); }
            }            

            if (magnifiedCard != null)
            {
                if (Input.GetMouseButtonDown(0) && frames != Time.frameCount)
                {
                    if (selectedCard != null)
                    {
                        PlayOrReturnCard();
                    }

                    else
                    {
                        mover.StopCard(thisCardCoroutine);

                        if (magnifiedCard.Targets())
                        {
                            cursorChanger.SetCursor();
                            frames = Time.frameCount;
                            selectedCardTargets = true;
                        }
                        else { selectedCardTargets = false; }

                        selectedCard = magnifiedCard;
                        cardPointerEvent.isSelectable = false;
                    }
                }
                else if (selectedCard != null && Input.GetMouseButtonDown(1))
                {
                    ReturnCard();
                }
            }
        }
        
        private void PlayOrReturnCard()
        {
            bool inHandArea = IsMouseInHandArea();
            if (inHandArea)
            {
                ReturnCard();
            }
            else
            {
                PlayCard();
            }
        }
        
        private bool IsMouseInHandArea()
        {
            Vector2 localPos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(handRect, Input.mousePosition, null, out localPos);
            return handRect.rect.Contains(localPos);
        }
        
        private void ReturnCard()
        {
            cursorChanger.ResetCursor();
            DemagnifyCard(magnifiedCard);
        }
        
        private void PlayCard()
        {
            selectedCard.isInHand = false;
            selectedCard.PlayCard(cardActions);
            GetComponent<HandManager>().RemoveCard(selectedCard);
            discard.DiscardCard(selectedCard);
            Reset();
        }

        private void UseTargetingMode()
        {
            if (IsMouseInHandArea())
            {
                mover.StopCard(thisCardCoroutine);
                selectedCard.transform.position = Input.mousePosition;
                selectedCard.centered = false;
            }
            else if (!selectedCard.centered)
            {
                CenterCard();
            }
        }

        private void CenterCard()
        {
            selectedCard.centered = true;
            Vector3 centerScale = new Vector3(centerScaleFactor, centerScaleFactor, centerScaleFactor);
            thisCardCoroutine = mover.MoveCard(selectedCard.GetRectTransform(), centerRotation, centerPos, centerScale, .1f, .5f, false);
        }

        private void Reset()
        {
            selectedCard = null;
            magnifiedCard = null;
            cardPointerEvent.isSelectable = true;
            cursorChanger.ResetCursor();
        }
    }
}
