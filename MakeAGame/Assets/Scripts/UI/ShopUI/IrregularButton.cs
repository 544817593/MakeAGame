using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class IrregularButton : Image
{
    
    private PolygonCollider2D _polygon;

    private PolygonCollider2D Polygon
    {
        get
        {
            if (_polygon == null)
                _polygon = GetComponent<PolygonCollider2D>();

            return _polygon;
        }
    }

    /// <summary>
    /// 重写IsRaycastLocationValid来判断自定义按钮的可点击区域。
    /// </summary>
    public override bool IsRaycastLocationValid(Vector2 screenPoint, Camera eventCamera)
    {
        Vector3 point;
        RectTransformUtility.ScreenPointToWorldPointInRectangle(rectTransform, screenPoint, eventCamera, out point);

        return Polygon.OverlapPoint(point);
    }
}
