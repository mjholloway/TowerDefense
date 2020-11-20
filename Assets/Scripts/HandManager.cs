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
    
    List<RawImage> deck = new List<RawImage>();
    List<RawImage> cards = new List<RawImage>();
    List<RectTransform> hand = new List<RectTransform>();
    int cardsInHand = 0;

    private void Start()
    {
        deck = deckObject.GetComponentsInChildren<RawImage>().ToList();
        StartCoroutine(DealHand());
    }

    private IEnumerator DealHand()
    {
        int currentHandSize = cardsInHand;

        foreach (RawImage card in deck)
        {
            hand.Add(card.GetComponent<RectTransform>());
        }

        foreach (RectTransform card in hand)
        {
            currentHandSize++;
            float handWidth = CalcHandWidth(currentHandSize);
            for (int cardNum = 0; cardNum < currentHandSize; cardNum++)
            {
                float cardPos = calcPosition(cardNum, handWidth, currentHandSize);
                hand[cardNum].anchoredPosition = new Vector2(cardPos, 0);
            }
            yield return new WaitForSeconds(.25f);
        }


        //hand[0].anchoredPosition = gameObject.transform.GetComponent<RectTransform>().pivot;
        //yield return new WaitForSeconds(.25f);
        //hand[1].anchoredPosition = gameObject.transform.GetComponent<RectTransform>().pivot;
        //hand[0].anchoredPosition = new Vector2(-100f, hand[0].anchoredPosition.y);
        //hand[0].Rotate(0, 0, 4);
        //hand[1].anchoredPosition = new Vector2(100f, hand[1].anchoredPosition.y);
        //hand[1].Rotate(0, 0, -4);
        //yield return new WaitForSeconds(.25f);

        //hand[2].anchoredPosition = gameObject.transform.GetComponent<RectTransform>().pivot;
        //hand[0].anchoredPosition = new Vector2(-215f, -14f);
        //hand[0].Rotate(0, 0, 4);
        //hand[1].anchoredPosition = gameObject.transform.GetComponent<RectTransform>().pivot;
        //hand[1].Rotate(0, 0, 4);
        //hand[2].anchoredPosition = new Vector2(215f, -14f);
        //hand[2].Rotate(0, 0, -8);
        //yield return new WaitForSeconds(.25f);

        //hand[3].anchoredPosition = hand[2].anchoredPosition;
        //hand[0].anchoredPosition = new Vector2(-315f, -25f);
        //hand[0].Rotate(0, 0, 2);
        //hand[1].anchoredPosition = new Vector2(-100f, hand[1].anchoredPosition.y);
        //hand[1].Rotate(0, 0, 4);
        //hand[2].anchoredPosition = new Vector2(100f, hand[1].anchoredPosition.y);
        //hand[2].Rotate(0, 0, 4);
        //hand[3].anchoredPosition = new Vector2(315f, -25f);
        //hand[3].Rotate(0, 0, -10);
        //yield return new WaitForSeconds(.25f);

        


    }

    // 260 is width of cards, I should code that in as a var later. 90 is the spacing between the cards. Also add that in later probs.
    private float CalcHandWidth(int currentHandSizeArg)
    {
        return (260 * currentHandSizeArg) - (90 * (currentHandSizeArg - 1));
    }

    private float calcPosition(int currentCard, float handWidthArg, int currentHandSizeArg)
    {
        if (currentCard == 0)
        {
            return -((handWidthArg / 2) - 130);
        }
        else if (currentCard == currentHandSizeArg)
        {
            return (handWidthArg / 2) + 130;
        }
        else
        {
            return hand[currentCard - 1].anchoredPosition.x + 170;
        }
    }

}
