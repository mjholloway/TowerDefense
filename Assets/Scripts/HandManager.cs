using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class HandManager : MonoBehaviour
{
    [SerializeField] int handSize = 5;
    [SerializeField] RawImage card;
    [SerializeField] GameObject deckObject;
    [SerializeField] float cardSpacing = 90f;
    [SerializeField] RectTransform lookAt;
    
    List<RawImage> deck = new List<RawImage>();
    List<RawImage> cards = new List<RawImage>();
    List<RectTransform> hand = new List<RectTransform>();
    int cardsInHand = 0;
    float cardWidth = 260f;

    private void Start()
    {
        deck = deckObject.GetComponentsInChildren<RawImage>().ToList();
        StartCoroutine(DealHand());
    }

    private IEnumerator DealHand()
    {
        int currentHandSize = cardsInHand;

        // Add cards to hand list. TODO: Make sure only 5 (or however many cards) are dealt from deck.
        foreach (RawImage card in deck)
        {
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
                SetPosition(currentHandSize, handWidth, cardNum);
                //float displacement;
                //if (currentHandSize % 2 == 0)
                //{
                //    if ( cardNum < currentHandSize / 2)
                //    {

                //    }
                //}
                SetRotation(currentHandSize, cardNum);
            }
            yield return new WaitForSeconds(.25f);
        }

    }

    // Calculates the width of the hand based on how many cards are in the hand and the width of the cards, but subtracts the overlap of the cards.
    private float CalcHandWidth(int currentHandSizeArg)
    {
        return (cardWidth * currentHandSizeArg) - (cardSpacing * (currentHandSizeArg - 1));
    }

    private void SetPosition(int currentHandSize, float handWidth, int cardNum)
    {
        float cardPos = CalcPosition(cardNum, handWidth, currentHandSize);
        hand[cardNum].anchoredPosition = new Vector2(cardPos, 0);
    }

    private void SetRotation(int currentHandSize, int cardNum)
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
        hand[cardNum].rotation = Quaternion.Euler(0, 0, rotation);
    }

    // Calculates where each card should be placed based on the width of the cards, the given spacing, and how many cards are in the hand.
    private float CalcPosition(int currentCard, float handWidthArg, int currentHandSizeArg)
    {
        // The first card will always border the left edge.
        if (currentCard == 0)
        {
            return -((handWidthArg / 2) - 130);
        }
        // The last card will always border the right edge.
        else if (currentCard == currentHandSizeArg)
        {
            return (handWidthArg / 2) + 130;
        }
        // Otherwise the card will be placed a set distance from the previous card. This will equal the distance between the center of the first card
        // and the edge of the next card plus the distance to the center of the next card.
        else
        {
            float distance = ((cardWidth / 2) - cardSpacing) + (cardWidth / 2); 
            return hand[currentCard - 1].anchoredPosition.x + distance;
        }
    }

    // Form a right triangle between the center of the card and the "lookAt" point that is placed below the screen. Uses inverse sine with the opposite side
    // over the hypotenuse to find the angle that would point the card at the point.
    private float CalcCardAngle(RectTransform cardArg)
    {
        float hypotenuse = Vector2.Distance(cardArg.anchoredPosition, lookAt.anchoredPosition);
        float opposite = Mathf.Abs(cardArg.anchoredPosition.x - lookAt.anchoredPosition.x);
        return Mathf.Asin(opposite / hypotenuse) * 180 / Mathf.PI;
    }
}
