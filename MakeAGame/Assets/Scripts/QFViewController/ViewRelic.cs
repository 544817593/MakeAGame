using System;
using QFramework;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class ViewRelic
    {
        private GameObject prefab = Resources.Load<GameObject>("Prefabs/RelicItem");
        
        public Transform transform;
        private GameObject touchArea;    // UI响应区域
        private Image imgRelic;

        public RelicBase relicData { get; private set; }
        
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

        public void InitWithData(RelicBase relic)
        {
            relicData = relic;
            imgRelic.sprite = Extensions.GetRelicSpriteByID(relicData.so.relicID);
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
            imgRelic = transform.Find("Root/ImgRelic").GetComponent<Image>();
            touchArea = transform.Find("Root/ImgRelic/UIEventArea").gameObject;
        }
        
    }
}