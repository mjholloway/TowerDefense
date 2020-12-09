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
    [SerializeField] float cardSpacing = 90f;
    [SerializeField] RectTransform lookAt;
    [SerializeField] float verticalDisplacementMultiplier = 15f;
    
    RectTransform handObject;
    Queue<RawImage> deck;
    List<RectTransform> hand = new List<RectTransform>();
    int cardsInHand = 0;
    float cardWidth = 260f;
    Coroutine coroutine;

    private void Start()
    {
        handObject = GetComponent<RectTransform>();
        deck = new Queue<RawImage>(deckObject.GetComponentsInChildren<RawImage>());
        StartCoroutine(DealHand(5));
    }

    private IEnumerator DealHand(int cardsToDeal)
    {
        
        CardHandler.isSelectable = false;

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
        foreach (RectTransform card in hand)
        {
            currentHandSize++;
            float handWidth = CalcHandWidth(currentHandSize);
            for (int cardNum = 0; cardNum < currentHandSize; cardNum++)
            {
                float xPos = SetPosition(currentHandSize, handWidth, cardNum);
                float yPos = SetDisplacement(currentHandSize, cardNum);
                float rotation = SetRotation(currentHandSize, cardNum);
                coroutine = StartCoroutine(MoveCard(cardNum, xPos, yPos, rotation));
                //StartCoroutine(RotateCard) <-- do this
            }
            yield return coroutine;
        }

        cardsInHand = currentHandSize;
        CardHandler.isSelectable = true;
    }

    // Calculates the width of the hand based on how many cards are in the hand and the width of the cards, but subtracts the overlap of the cards.
    private float CalcHandWidth(int currentHandSizeArg)
    {
        return (cardWidth * currentHandSizeArg) - (cardSpacing * (currentHandSizeArg - 1));
    }

    private float SetPosition(int currentHandSize, float handWidth, int cardNum)
    {
        float cardPos = CalcPosition(cardNum, handWidth, currentHandSize);
        return cardPos;
    }

    // Calculates where each card should be placed based on the distance from the left bound (half the calculated width of the hand).
    private float CalcPosition(int currentCard, float handWidthArg, int currentHandSizeArg)
    {
        float leftBound = -((handWidthArg / 2) - 130);
        float distance = ((cardWidth / 2) - cardSpacing) + (cardWidth / 2);
        return leftBound + (distance * currentCard);
    }

    // Okay, so here I'm trying to set the vertical displacement of the cards so that they will be farther down the farther they are from the center of the hand.
    private float SetDisplacement(int currentHandSize, int cardNum)
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

    private float SetRotation(int currentHandSize, int cardNum)
    {
        float rotation;
        // Rotation should be negative if the card is to the right of center.
        if ((cardNum + 1) > currentHandSize / 2)
        {
            rotation = -CalcCardAngle(hand[cardNum]);
        }
        else
        {
            rotation = CalcCardAngle(hand[cardNum]);
        }
        return rotation;
    }


    // Form a right triangle between the center of the card and the "lookAt" point that is placed below the screen. Uses inverse sine with the opposite side
    // over the hypotenuse to find the angle that would point the card at the point.
    private float CalcCardAngle(RectTransform cardArg)
    {
        float hypotenuse = Vector2.Distance(cardArg.anchoredPosition, lookAt.anchoredPosition);
        float opposite = Mathf.Abs(cardArg.anchoredPosition.x - lookAt.anchoredPosition.x);
        return Mathf.Asin(opposite / hypotenuse) * 180 / Mathf.PI;
    }

    private IEnumerator MoveCard(int cardNum, float xPos, float yPos, float rotation)
    {
        Vector2 velocity = new Vector2(0f, 0f);
        float passedTime = 0f;
        float targetTime = .25f;
        Vector2 target = new Vector2(xPos, yPos);
        while (passedTime < targetTime)
        {
            Vector2 intermediate = Vector2.SmoothDamp(hand[cardNum].anchoredPosition, target, ref velocity, targetTime);
            hand[cardNum].anchoredPosition = intermediate;
            passedTime += Time.deltaTime;
            yield return null;
        }
        hand[cardNum].anchoredPosition = target;
    }
}
