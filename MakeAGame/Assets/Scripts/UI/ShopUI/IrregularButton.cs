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
    /// 监听点击事件 点击后打开对应panel
    /// </summary>
    //protected override void Start()
    //{
    //    string gameObjectName = gameObject.name;
    //    gameObject.GetComponent<Button>().onClick.AddListener(() =>
    //    {
    //        Debug.Log($"点击了{gameObjectName}");
    //        openPanel(gameObjectName);
    //    });
    //}
    /// <summary>
    /// 根据点击的gameobject名字打开对应panel
    /// </summary>
    /// <param name="gameObjectName"></param>
    /// TODO:
    private void openPanel(string gameObjectName)
    {
        if (gameObjectName == "Buy")
        {
            UIKit.ClosePanel<ShopMainUI.ShopMainUI>();
            UIKit.OpenPanel<ShopBuyUI.ShopBuyUI>();
        }
        else if (gameObjectName == "Sell")
        {

        }
        else if( gameObjectName == "LevelUp")
        {

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
