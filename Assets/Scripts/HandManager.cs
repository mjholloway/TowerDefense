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
    
    List<RawImage> deck = new List<RawImage>();
    List<RawImage> cards = new List<RawImage>();
    List<RectTransform> hand = new List<RectTransform>();

    private void Start()
    {
        deck = deckObject.GetComponentsInChildren<RawImage>().ToList();
        StartCoroutine(DealHand());
    }

    private IEnumerator DealHand()
    {
        foreach (RawImage card in deck)
        {
            hand.Add(card.GetComponent<RectTransform>());
        }
        hand[0].anchoredPosition = gameObject.transform.GetComponent<RectTransform>().pivot;
        yield return new WaitForSeconds(.25f);
        hand[1].anchoredPosition = gameObject.transform.GetComponent<RectTransform>().pivot;
        hand[0].anchoredPosition = new Vector2(-100f, hand[0].anchoredPosition.y);
        hand[0].Rotate(0, 0, 4);
        hand[1].anchoredPosition = new Vector2(100f, hand[1].anchoredPosition.y);
        hand[1].Rotate(0, 0, -4);



    }
}
