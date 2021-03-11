using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class CardHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    public static bool isSelectable = true;
    public bool isMagnified = false;
    public bool isInHand = false;

    [SerializeField] float verticalMagnificationDisplacement = 150f;
    [SerializeField] float magnifyScaleFactor = 1.25f;
    [SerializeField] float centerScaleFactor = 1.1f;
    [SerializeField] Vector3 magnifyRotation = new Vector3(0, 0, 0);
    [SerializeField] Vector2 centerPos = new Vector2(0f, 100f);
    [SerializeField] Vector2 centerRotation = new Vector3(0, 0, 0);

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
    IPlayable playable;
    bool targets;
    int frames;
    bool centered = false;


    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        playable = GetComponent<IPlayable>();
    }

    private void Update()
    {
        if (isSelected)     // Card will stay at mouse position after it is selected.
        {
            if (!targets)
            {
                transform.position = Input.mousePosition;
            }
            else
            {
                if (FindMousePos())
                {
                    hand.GetComponent<CardMover>().StopCard(thisCardCoroutine);
                    transform.position = Input.mousePosition;
                    centered = false;
                }
                else if(!centered)
                {
                    CenterCard();
                }
                if (Input.GetMouseButtonDown(0) && frames != Time.frameCount)
                {
                    PlayOrReturnCard();
                }
            }
        }

    }

    // This is called in HandManager when the card is dealt to store its position and rotation and index, as well as when cards are played/discarded.
    public void SetProperties(Quaternion rotationCalc, Vector2 positionCalc, int index)
    {
        startRotation = rotationCalc;
        startPosition = positionCalc;
        startIndex = index;
        isInHand = true;
        targets = playable.targets;
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
            PlayOrReturnCard();
        }
        else if (isSelectable)
        {
            hand.GetComponent<CardMover>().StopCard(thisCardCoroutine); // This seems unnecessary but it prevents the "coroutine continue failure" from appearing
            if (targets)
            {
                transform.parent.GetComponent<CursorChanger>().SetCursor();
                frames = Time.frameCount;
            }
            isSelected = true;
            isSelectable = false;
        }
    }

    private void PlayOrReturnCard()
    {
        centered = false;
        transform.parent.GetComponent<CursorChanger>().ResetCursor();
        bool inHandArea = FindMousePos();
        if (inHandArea)
        {
            ReturnCard();
        }
        else
        {
            PlayCard();
        }
    }

    private bool FindMousePos()
    {
        RectTransform handRect;
        Vector2 localPos;
        handRect = hand.GetComponent<RectTransform>();
        RectTransformUtility.ScreenPointToLocalPointInRectangle(handRect, Input.mousePosition, null, out localPos);
        return handRect.rect.Contains(localPos);
    }

    private void ReturnCard()
    {
        isSelected = false;
        isSelectable = true;
        Demagnify();
    }

    private void PlayCard()
    {
        isMagnified = false;
        isSelected = false;
        isSelectable = true;
        isInHand = false;
        hand.GetComponent<HandManager>().RemoveCard(rectTransform);
        discard.GetComponent<DiscardManager>().DiscardCard(rectTransform);
    }

    private void CenterCard()
    {
        centered = true;
        Vector3 centerScale = new Vector3(centerScaleFactor, centerScaleFactor, centerScaleFactor);
        thisCardCoroutine = hand.GetComponent<CardMover>().MoveCard(rectTransform, centerRotation, centerPos, centerScale, .1f, .5f, false);
    }

    // Calls CardMover to magnify the hovered card to target position and move others away.
    private void MagnifyCard()
    {
        isMagnified = true;

        rectTransform.SetAsLastSibling();

        Vector2 magnifyPos = new Vector2(startPosition.x, verticalMagnificationDisplacement);
        Vector3 magnifyScale = new Vector3(magnifyScaleFactor, magnifyScaleFactor, 1f);

        thisCardCoroutine = hand.GetComponent<CardMover>().MoveCard(rectTransform, magnifyRotation, magnifyPos, magnifyScale, .1f, .5f);
    }

    // Calls CardMover to reset the hand and sets the card to proper index in heirarchy.
    private void Demagnify()
    {
        isMagnified = false;

        hand.GetComponent<CardMover>().MoveCard(rectTransform, startRotation.eulerAngles, startPosition, startScale, .1f, .5f);
        rectTransform.SetSiblingIndex(startIndex);
    }
    
}
