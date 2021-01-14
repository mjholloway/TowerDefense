using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using UnityEngine.Events;

public class CardHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    public static bool isSelectable = true;
    public bool isMagnified = false;
    public bool isInHand = false;

    [SerializeField] float verticalMagnificationDisplacement = 150f;
    [SerializeField] float scaleFactor = 1.25f;

    RectTransform rectTransform;
    Quaternion startRotation;
    Vector2 startPosition;
    Vector3 startScale = new Vector3(1f, 1f, 1f);
    int startIndex;
    bool isSelected = false;
    Coroutine thisCardCoroutine;
    GameObject deck;
    GameObject hand;
    GameObject discard;


    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        if (isSelected)     // Card will stay at mouse position after it is selected.
        {
            transform.position = Input.mousePosition;
        }
    }

    // This is called in HandManager when the card is dealt to store its position and rotation and index, as well as when cards are played/discarded.
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
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (isSelectable && isInHand)
        {
            MagnifyCard();
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (isSelectable && isInHand)
        {
            Demagnify();
        }
    }
    
    public void OnPointerDown(PointerEventData eventData)
    {
        // Will return card to hand if clicked in hand area, otherwise will move card to discard (later will play card)
        if (isSelected)
        {
            RectTransform handRect = hand.GetComponent<RectTransform>();
            Vector2 localPos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(handRect, Input.mousePosition, null, out localPos);
            if (handRect.rect.Contains(localPos))
            {
                isSelected = false;
                isSelectable = true;
                Demagnify();
            }
            else
            {
                isMagnified = false;
                isSelected = false;
                isSelectable = true;
                isInHand = false;
                hand.GetComponent<HandManager>().RemoveCard(rectTransform);
                discard.GetComponent<DiscardManager>().DiscardCard(rectTransform);
            }
        }
        else if (isSelectable)
        {
            hand.GetComponent<CardMover>().StopCard(thisCardCoroutine); // This seems unnecessary but it prevents the "coroutine continue failure" from appearing
            isSelected = true;
            isSelectable = false;
        }
    }

    // Calls CardMover to magnify the hovered card to target position and move others away.
    private void MagnifyCard()
    {
        isMagnified = true;

        rectTransform.SetAsLastSibling();

        Vector3 targetRot = new Vector3(0, 0, 0);
        Vector2 targetPos = new Vector2(startPosition.x, verticalMagnificationDisplacement);
        Vector3 targetScale = new Vector3(scaleFactor, scaleFactor, 1f);

        thisCardCoroutine = hand.GetComponent<CardMover>().MoveCard(rectTransform, targetRot, targetPos, targetScale, .1f, .5f);
    }

    // Calls CardMover to reset the hand and sets the card to proper index in heirarchy.
    private void Demagnify()
    {
        isMagnified = false;

        hand.GetComponent<CardMover>().MoveCard(rectTransform, startRotation.eulerAngles, startPosition, startScale, .1f, .5f);
        rectTransform.SetSiblingIndex(startIndex);
    }
    
}
