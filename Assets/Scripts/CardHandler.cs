using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class CardHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    public static bool isSelectable = true;

    RectTransform rectTransform;
    Quaternion rotation;
    Vector2 position;
    Vector3 scale;
    int index;
    bool isSelected = false;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        if (isSelected)
        {
            transform.position = Input.mousePosition;
            isSelectable = false;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (isSelectable)
        {
            MagnifyCard();
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Demagnify();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (isSelectable)
        {
            isSelected = true;
        }
    }

    private void MagnifyCard()
    {
        rotation = rectTransform.rotation;
        rectTransform.rotation = Quaternion.Euler(0, 0, 0);

        position = rectTransform.anchoredPosition;
        rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, 150f);

        scale = rectTransform.localScale;
        rectTransform.localScale = new Vector3(1.25f, 1.25f, 1f);

        index = rectTransform.GetSiblingIndex();
        rectTransform.SetAsLastSibling();
    }

    private void Demagnify()
    {
        rectTransform.rotation = rotation;
        rectTransform.anchoredPosition = position;
        rectTransform.localScale = scale;
        rectTransform.SetSiblingIndex(index);
    }
}
