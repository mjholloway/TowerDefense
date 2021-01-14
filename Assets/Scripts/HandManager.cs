using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class HandManager : MonoBehaviour
{
    [SerializeField] int handSize = 5;
    [SerializeField] GameObject deckObject;
    [SerializeField] GameObject discardObject;
    [SerializeField] float cardSpacing = 90f;
    [SerializeField] RectTransform lookAt;
    [SerializeField] float verticalDisplacementMultiplier = 15f;
    
    RectTransform handObject;
    Queue<RawImage> deck;
    List<RectTransform> hand = new List<RectTransform>();
    int cardsInHand = 0;
    float cardWidth = 260f;
    Coroutine coroutine;
    CardMover cardMover;

    private void Start()
    {
        handObject = GetComponent<RectTransform>();
        deck = new Queue<RawImage>(deckObject.GetComponentsInChildren<RawImage>());
        cardMover = GetComponent<CardMover>();
        StartCoroutine(DealHand(handSize));
    }

    private IEnumerator DealHand(int cardsToDeal)
    {
        CardHandler.isSelectable = false;

        int targetHandSize = cardsInHand + cardsToDeal;
        int currentHandSize = cardsInHand;
        int dealValue = Mathf.Min(cardsToDeal, deck.Count); // This should eventually be removed and replaced (discard should be shuffled for new deck)!!!!

        // Add cards to hand.
        for (int i = 0; i < dealValue; i++)
        {
            var card = deck.Dequeue();
            card.rectTransform.SetParent(handObject.transform, true);
            hand.Add(card.GetComponent<RectTransform>());
        }

        // The goal here is to iterate through each card item in the hand, calculate the width of the hand, and then iterate again using a for loop to set the
        // positions and rotations of the cards step by step to create the appearance of cards being dealt as opposed to all the cards appearing in their 
        // designated positions.
        for (; currentHandSize <= targetHandSize; currentHandSize++)
        {
            float handWidth = CalcHandWidth(currentHandSize);
            GetComponent<RectTransform>().sizeDelta = new Vector2(handWidth, 420);
            for (int cardNum = 0; cardNum < currentHandSize; cardNum++)
            {
                RectTransform card = hand[cardNum];
                CardHandler cardHandler = card.GetComponent<CardHandler>();
                Vector2 position = new Vector2(CalcPosition(cardNum, handWidth), CalcDisplacement(currentHandSize, cardNum));
                Vector3 rotation = GetRotation(currentHandSize, cardNum, position);
                cardHandler.SetProperties(Quaternion.Euler(rotation), position, cardNum);
                cardHandler.SetParents(deckObject, gameObject, discardObject);
                coroutine = cardMover.DealCard(card, rotation, position, new Vector3(1, 1, 1), .1f, .3f);
            }
            yield return coroutine;
        }

        cardsInHand = currentHandSize - 1;
        CardHandler.isSelectable = true;
    }

    // Calculates the width of the hand based on how many cards are in the hand and the width of the cards, but subtracts the overlap of the cards.
    private float CalcHandWidth(int currentHandSizeArg)
    {
        return (cardWidth * currentHandSizeArg) - (cardSpacing * (currentHandSizeArg - 1));
    }

    // Calculates where each card should be placed based on the distance from the left bound (half the calculated width of the hand).
    private float CalcPosition(int currentCard, float handWidthArg)
    {
        float leftBound = -((handWidthArg / 2) - 130);
        float distance = ((cardWidth / 2) - cardSpacing) + (cardWidth / 2);
        return leftBound + (distance * currentCard);
    }

    // Okay, so here I'm trying to set the vertical displacement of the cards so that they will be farther down the farther they are from the center of the hand.
    private float CalcDisplacement(int currentHandSize, int cardNum)
    {
        float displacementFactor;
        int split = currentHandSize / 2;
        int switchFactor;
        // if the hand size is even...
        if (currentHandSize % 2 == 0)
        {
            if (cardNum < currentHandSize / 2)
            {
                switchFactor = split - 1 - cardNum; // There are 2 splits for an even hand since there is no true center card. Defining different switchfactors
            }                                       // also allows me to use one switch function for everything regardless of the contents of the hand.
                                                    // For example: 10 cards: half is 5, split at 4 & 5 (since card count starts at 0). 0,1,2,3,4 -- 5,6,7,8,9
            else
            {
                switchFactor = cardNum - split;
            }
            
        }
        // if the hand size is odd...
        else
        {
            switchFactor = Mathf.Abs(split - cardNum);
        }
        // It is worth pointing out that these displacement factors were determined purely by experimenting with moving the cards around on the screen and 
        // seeing what looks right. They might need to be changed if I change literally anything else about how the cards appear on screen.
        switch (switchFactor)
        {
            case 0:
                displacementFactor = 0;
                break;
            case 1:
                displacementFactor = 1;
                break;
            case 4:
                displacementFactor = 10;
                break;
            default:
                displacementFactor = (switchFactor - 1) * 3;
                break;
        }
        // Just for reminder, the multiplier is what I can change from the inspector to get instant changes. The factor is essentially the ratio of card 
        // displacement calculated in the previous step.
        float displacement = verticalDisplacementMultiplier * displacementFactor;
        return -displacement;
    }

    //The naming convention is different for this function since the actual angle calculation is done in another function, but this one returns the proper rotation.
    private Vector3 GetRotation(int currentHandSize, int cardNum, Vector2 cardPos)
    {
        float rotation;
        // Rotation should be negative if the card is to the right of center.
        if ((cardNum + 1) > currentHandSize / 2)
        {
            rotation = -CalcCardAngle(cardPos);
        }
        else
        {
            rotation = CalcCardAngle(cardPos);
        }
        return new Vector3(0, 0, rotation);
    }


    // Form a right triangle between the center of the card and the "lookAt" point that is placed below the screen. Uses inverse sine with the opposite side
    // over the hypotenuse to find the angle that would point the card at the point.
    private float CalcCardAngle(Vector2 cardPos)
    {
        float hypotenuse = Vector2.Distance(cardPos, lookAt.anchoredPosition);
        float opposite = Mathf.Abs(cardPos.x - lookAt.anchoredPosition.x);
        return Mathf.Asin(opposite / hypotenuse) * 180 / Mathf.PI;
    }

    public int GetHandSize()
    {
        return cardsInHand;
    }

    public int GetCardIndex(RectTransform card)
    {
        return hand.IndexOf(card.GetComponent<RectTransform>());
    }

    // Function is called from CardMover when a card is magnified and other cards need to be moved out of the way. It calculates the position and rotation of the
    // card based on its placement in the hand list by calling the previous calculation functions in this class.
    public RectTransform GetShiftValues(int currentIndex, out Vector3 rotation, out Vector2 position)
    {
        float handWidth = handObject.rect.width;
        
        position = new Vector2(CalcPosition(currentIndex, handWidth), CalcDisplacement(cardsInHand, currentIndex));
        rotation = GetRotation(cardsInHand, currentIndex, position);

        return hand[currentIndex];
    }

    // Used when a card is removed from hand, will remove it from the hand list, augment cardsinhand variable, and alter the size of the handobject.
    public void RemoveCard(RectTransform card)
    {
        hand.Remove(card);
        cardsInHand--;
        GetComponent<RectTransform>().sizeDelta = new Vector2(CalcHandWidth(cardsInHand), 420);
    }
}
