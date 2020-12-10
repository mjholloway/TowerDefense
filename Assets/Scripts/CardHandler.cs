using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class CardHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    public static bool isSelectable = true;

    [SerializeField] float verticalMagnificationDisplacement = 150f;
    [SerializeField] float scaleFactor = 1.25f;

    RectTransform rectTransform;
    Quaternion rotation;
    Vector2 position;
    Vector3 scale;
    int index;
    bool isSelected = false;
    Coroutine coroutine;
    Rect rect;

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
        if (!rect.Contains(Input.mousePosition))
        {
            Demagnify();
        }
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
        rect = rectTransform.rect; //????????????????
        print(rect);

        index = rectTransform.GetSiblingIndex();
        rectTransform.SetAsLastSibling();
        rotation = rectTransform.rotation;
        position = rectTransform.anchoredPosition;
        scale = rectTransform.localScale;

        Vector3 targetRot = new Vector3(0, 0, 0);
        Vector2 targetPos = new Vector2(position.x, verticalMagnificationDisplacement);
        Vector3 targetScale = new Vector3(scaleFactor, scaleFactor, 1f);

        coroutine = StartCoroutine(AdjustCard(targetRot, targetPos, targetScale));
    }

    private void Demagnify()
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
        coroutine = StartCoroutine(AdjustCard(rotation.eulerAngles, position, scale));
        rectTransform.SetSiblingIndex(index);
    }

    private IEnumerator AdjustCard(Vector3 targetRot, Vector2 targetPos, Vector3 targetScale)
    {
        float rotVelocity = 0f;
        Vector2 posVelocity = new Vector2(0f, 0f);
        Vector3 scaleVelocity = new Vector3(0, 0, 0);
        float passedTime = 0f;
        float targetTime = .1f;

        while (passedTime < .5f)
        {
            float intermediateRot = Mathf.SmoothDampAngle(rectTransform.eulerAngles.z, targetRot.z, ref rotVelocity, targetTime);
            rectTransform.rotation = Quaternion.Euler(0, 0, intermediateRot);

            Vector2 intermediatePos = Vector2.SmoothDamp(rectTransform.anchoredPosition, targetPos, ref posVelocity, targetTime);
            rectTransform.anchoredPosition = intermediatePos;

            Vector3 intermediateScale = Vector3.SmoothDamp(rectTransform.localScale, targetScale, ref scaleVelocity, targetTime);
            rectTransform.localScale = intermediateScale;

            passedTime += Time.deltaTime;
            yield return null;
        }
        rectTransform.rotation = Quaternion.Euler(targetRot);
        rectTransform.anchoredPosition = targetPos;
        rectTransform.localScale = targetScale;
    }
}
