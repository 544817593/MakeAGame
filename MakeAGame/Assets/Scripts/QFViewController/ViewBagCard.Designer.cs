using UnityEngine;
using System.Collections.Generic;
using System.ComponentModel;
using QFramework;
using TMPro;
using UnityEngine.UI;

namespace Game
{
    // 保持了和QFramework代码生成同样的格式，但是是手写的，因为自动生成会调用两次start，给我整不会了
    // 可能因为我的ViewBagCard脚本是动态挂上去的，而不是prefab自带的
    // 所以也需要动态赋值，无法用拖拽的形式提前赋值
    public partial class ViewBagCard
    {
        public Transform nodeFeature;   // feature根结点
        public List<GameObject> featureTouchArea = new List<GameObject>();   // feature UI响应区域
        public GameObject touchArea;    // UI响应区域
        public Canvas canvas;   // 用于调整层级
        public CanvasGroup canvasGroup; // 用于调整透明度
        public Transform tooltipTrans;  // 二级浮窗浮现位置节点

        public Image imgRarity;
        public Image imgCharacter;
        public TextMeshProUGUI tmpSanCost;
        public TextMeshProUGUI tmpHP;
        public TextMeshProUGUI tmpMoveSpd;
        public TextMeshProUGUI tmpDamage;
        public TextMeshProUGUI tmpDefend;
        public TextMeshProUGUI tmpName;
        public TextMeshProUGUI tmpDesc;
        
        // 绑定各组件
        private void InitBind()
        {
            nodeFeature = transform.Find("Root/NodeFeature");
            for (int i = 0; i < 3; i++)
            {
                var tmpTrans = nodeFeature.GetChild(i).gameObject;
                featureTouchArea.Add(tmpTrans);
            }
            touchArea = transform.Find("Root/UIEventArea").gameObject;
            canvas = gameObject.GetComponent<Canvas>();
            canvasGroup = gameObject.GetComponent<CanvasGroup>();
            tooltipTrans = transform.Find("Root/TooltipPos");

            imgRarity = transform.Find("Root/ImgRarity").GetComponent<Image>();
            imgCharacter = transform.Find("Root/ImgCharacter").GetComponent<Image>();

            tmpSanCost = transform.Find("Root/ImgRarity/TextSanCost").GetComponent<TextMeshProUGUI>();
            tmpHP = transform.Find("Root/NodePoint/TextHP").GetComponent<TextMeshProUGUI>();
            tmpMoveSpd = transform.Find("Root/NodePoint/TextAGI").GetComponent<TextMeshProUGUI>();
            tmpDamage = transform.Find("Root/NodePoint/TextSTR").GetComponent<TextMeshProUGUI>();
            tmpDefend = transform.Find("Root/NodePoint/TextDEF").GetComponent<TextMeshProUGUI>();
            tmpName = transform.Find("Root/TextName").GetComponent<TextMeshProUGUI>();
            tmpDesc = transform.Find("Root/TextDesc").GetComponent<TextMeshProUGUI>();
        }
    }
}