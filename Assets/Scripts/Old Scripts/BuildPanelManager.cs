using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;

public class BuildPanelManager : MonoBehaviour
{
    Button button;
    Text buttonText;
    RectTransform rect;
    Vector3 downPos;
    Vector3 upPos;
    bool isActivated = false;
    
    void Start()
    {
        button = GetComponentInChildren<Button>();
        button.onClick.AddListener(ActivatePanel);
        buttonText = button.GetComponentInChildren<Text>();
        buttonText.text = "Build Panel " + "\u25B2".ToString();
        rect = GetComponent<RectTransform>();
        downPos = rect.localPosition;
        upPos = new Vector3(rect.localPosition.x, -120, rect.localPosition.z);
    }

    void Update()
    {
        
    }

    void ActivatePanel()
    {
        if (!isActivated)
        {
            StartCoroutine(MovePanel(downPos, upPos));
            isActivated = true;
            buttonText.text = "Build Panel " + "\u25BC".ToString();
        }
        else
        {
            StartCoroutine(MovePanel(upPos, downPos));
            isActivated = false;
            buttonText.text = "Build Panel " + "\u25B2".ToString();
        }
    }

    IEnumerator MovePanel(Vector3 start, Vector3 end)
    {
        float passedTime = 0f;
        float moveDuration = .5f;

        while (passedTime < moveDuration)
        {
            float lerpFactor = passedTime / moveDuration;
            float smoothLerpFactor = Mathf.SmoothStep(0, 1, lerpFactor);

            rect.localPosition = Vector3.Lerp(start, end, smoothLerpFactor);

            passedTime += Time.deltaTime;
            yield return null;
        }

        rect.localPosition = end;
    }

    private void OnDisable()
    {
        button.onClick.RemoveListener(ActivatePanel);
    }
}
