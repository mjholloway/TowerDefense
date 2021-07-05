using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TowerDefense.Deck
{
    public class DiscardManager : MonoBehaviour
    {
        [SerializeField] CardMover mover;

        public void DiscardCard(CardProperties card)
        {
            RectTransform cardRectTransform = card.GetRectTransform();
            cardRectTransform.SetParent(GetComponent<RectTransform>(), true);
            mover.MoveCard(cardRectTransform, new Vector3(0, 0, 0), GetComponent<RectTransform>().anchoredPosition, 
                new Vector3(1, 1, 1), .1f, .5f, false);
        }
    }
}
