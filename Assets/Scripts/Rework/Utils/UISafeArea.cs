using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISafeArea : MonoBehaviour
{
    private Rect _lastSafeArea;

    private void Update()
    {
        if (_lastSafeArea != Screen.safeArea)
        {
            ApplySafeArea();
        }
    }

    private void ApplySafeArea()
    {
        Rect safeAreaRect = Screen.safeArea;

        RectTransform rectTransform = GetComponent<RectTransform>();

        float scaleRatio = rectTransform.root.GetComponentInChildren<RectTransform>().rect.width / Screen.width;

        var left = safeAreaRect.xMin * scaleRatio;
        var right = -( Screen.width - safeAreaRect.xMax ) * scaleRatio;
        var top = -( Screen.height - safeAreaRect.yMax ) * scaleRatio;
        var bottom = safeAreaRect.yMin * scaleRatio;

        rectTransform.offsetMin = new Vector2( left, bottom );
        rectTransform.offsetMax = new Vector2( right, top );

        _lastSafeArea = Screen.safeArea;
    }
}
