using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

namespace Game
{
    public partial class ViewPiece
    {
        public SpriteRenderer spPiece;
        public GameObject touchArea;    // 鼠标响应区域
        
        void InitBind()
        {
            spPiece = transform.Find("Root/SpritePiece").GetComponent<SpriteRenderer>();
            touchArea = transform.Find("Root/SpritePiece").gameObject;
            mouseHelper = touchArea.AddComponent<TriggerHelper>();
            mouseHelper.OnMouseDownEvent = MouseDown;
            mouseHelper.OnMouseUpEvent = MouseUp;
        }
    }
}