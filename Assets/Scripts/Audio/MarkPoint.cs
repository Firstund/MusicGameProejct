using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkPoint : MonoBehaviour
{
    private RectTransform rectTransform;

    private float markTime = 0f;
    public float MarkTime
    {
        get { return markTime; }
        set { markTime = value; }
    }

    private void Start()
    {
        if (rectTransform == null)
        {
            rectTransform = GetComponent<RectTransform>();
        }
    }

    public void SetMarkTime(Transform parent, float time, float xLengthPerSec)
    {
        if (rectTransform == null)
        {
            rectTransform = GetComponent<RectTransform>();
        }
        
        Vector3 sizeDelta = rectTransform.sizeDelta;
        transform.SetParent(parent);
        transform.localScale = Vector3.one;

        sizeDelta.x = xLengthPerSec;
        rectTransform.sizeDelta = sizeDelta;

        Vector3 pos = Vector3.zero;
        pos.x = time * xLengthPerSec;

        transform.localPosition = pos;
    }
}
