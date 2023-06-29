using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

namespace Game
{
    public partial class ViewPiece
    {
        public SpriteRenderer spPiece;
        public GameObject touchArea;    // 鼠标响应区域
        public BoxCollider2D collider2d;
        
        void InitBind()
        {
            spPiece = transform.Find("Root/SpritePiece").GetComponent<SpriteRenderer>();
            touchArea = transform.Find("Root/SpritePiece").gameObject;
            collider2d = transform.Find("Root/SpritePiece").GetComponent<BoxCollider2D>();
            mouseHelper = touchArea.AddComponent<TriggerHelper>();
            mouseHelper.OnMouseDownEvent = MouseDown;
            mouseHelper.OnMouseUpEvent = MouseUp;
            mouseHelper.OnMouseEnterEvent = MouseEnter;
            mouseHelper.OnMouseExitEvent = MouseExit;
        }
    }
}