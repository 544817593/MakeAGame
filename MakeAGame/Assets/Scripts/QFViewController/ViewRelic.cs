using System;
using QFramework;
using UnityEngine;

namespace Game
{
    public class ViewRelic
    {
        private GameObject prefab = Resources.Load<GameObject>("Prefabs/RelicItem");
        
        public Transform transform;
        public GameObject touchArea;    // UI响应区域
        public Action<ViewRelic> OnFocus;
        public Action OnUnfocus;

        public ViewRelic(Transform parent)
        {
            GameObject go = GameObject.Instantiate(prefab,UIRoot.Instance.transform);
            transform = go.transform;
            
            InitBind();
            var uiHelper = touchArea.AddComponent<UIEventHelper>();
            uiHelper.OnUIPointEnter = MouseEnter;
            uiHelper.OnUIPointExit = MouseExit;
        }

        void MouseEnter()
        {
            OnFocus(this);
        }

        void MouseExit()
        {
            OnUnfocus.Invoke();
        }

        private void InitBind()
        {
            touchArea = transform.Find("Root/ImgRelic/UIEventArea").gameObject;
        }
        
    }
}