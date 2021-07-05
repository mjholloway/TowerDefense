using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace TowerDefense.Deck
{
    public class CardProperties : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public bool isMagnified = false;
        public bool isInHand = false;
        public bool centered = false;

        RectTransform rectTransform;
        GameObject deck;
        GameObject hand;
        GameObject discard;
        IPlayable playable;
        CardPointerEvent cardPointerEvent;

        Quaternion startRotation;
        Vector2 startPosition;
        Vector3 startScale = new Vector3(1f, 1f, 1f);
        int startIndex;       
 
        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            playable = GetComponent<IPlayable>();
        }

        // This is called in HandManager when the card is dealt to store its position and rotation and index,
        // as well as when cards are played/discarded.
        public void SetProperties(Quaternion rotationCalc, Vector2 positionCalc, int index)
        {
            startRotation = rotationCalc;
            startPosition = positionCalc;
            startIndex = index;
            isInHand = true;
        }

        public void SetParents(GameObject deckObject, GameObject handObject, GameObject discardObject)
        {
            deck = deckObject;
            hand = handObject;
            discard = discardObject;
            cardPointerEvent = GetComponentInParent<CardPointerEvent>();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (cardPointerEvent.isSelectable && isInHand)
            {
                cardPointerEvent.pointerEnterEvent.Invoke(this);
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (cardPointerEvent.isSelectable && isInHand)
            {
                cardPointerEvent.pointerExitEvent.Invoke(this);
            }
        }

        public RectTransform GetRectTransform()
        {
            return rectTransform;
        }

        public Vector2 GetStartPosition()
        {
            return startPosition;
        }

        public int GetStartIndex()
        {
            return startIndex;
        }

        public Quaternion GetStartRotation()
        {
            return startRotation;
        }

        public Vector3 GetStartScale()
        {
            return startScale;
        }
    }
}
