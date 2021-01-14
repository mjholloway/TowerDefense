using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscardManager : MonoBehaviour
{
    [SerializeField] CardMover mover;

    public void DiscardCard(RectTransform card)
    {
        card.SetParent(GetComponent<RectTransform>(), true);
        mover.MoveCard(card, new Vector3(0, 0, 0), GetComponent<RectTransform>().anchoredPosition, new Vector3(1, 1, 1), .1f, .5f);
    }
}
