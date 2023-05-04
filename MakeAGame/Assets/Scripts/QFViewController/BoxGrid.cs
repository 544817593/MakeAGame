using System;
using QFramework;
using UnityEngine;

namespace Game
{
    /// <summary>
    /// 地图格子相关的常量
    /// </summary>
    public struct GridConst
    {
        public static string TerrainResPathPrefix = "Sprites/Grids/terrain";    // 地块图片Resources路径前缀
    }
    
    /// <summary>
    /// 地图格子
    /// </summary>
    public class BoxGrid: MonoBehaviour, IController
    {
        public int row; // 行
        public int col; // 列
        public BindableProperty<int> terrain = new BindableProperty<int>((int)TerrainEnum.Road);    // 地形类型
        public BindableProperty<float> timeMultiplier = new BindableProperty<float>();  // 时间流逝倍数
        public int occupation; // 当前格子上的单位的ID
        public BindableProperty<GridStatusEnum> gridStatus = new BindableProperty<GridStatusEnum>(); // 格子状态
        
        // components
        private SpriteRenderer srFloor; // 地形图片
        private SpriteRenderer srHint;  // 提示颜色图片
        public GameObject touchArea;    // 鼠标响应区域
        public TriggerHelper mouseHelper;

        // A*寻路算法需要的变量
        public int gCost; // 从初始点到目前点已经耗费的点数
        public int hCost; // 预计到终点还需要的点数
        public int fCost; // 总点数
        public BoxGrid cameFrom; // 上一个格子

        private void Start()
        {
            srFloor = transform.Find("Root/SpriteFloor").GetComponent<SpriteRenderer>();
            gridStatus.Value = GridStatusEnum.Unoccupied;
            srHint = transform.Find("Root/SpriteHint").GetComponent<SpriteRenderer>();
            HideHint();

            touchArea = transform.Find("Root/TriggerArea").gameObject;
            mouseHelper = touchArea.AddComponent<TriggerHelper>();
            mouseHelper.OnMouseEnterEvent = OnEnter;
            mouseHelper.OnMouseExitEvent = OnExit;
            // mouseHelper.enabled = false; // 无效，invoke还是会执行
            touchArea.gameObject.SetActive(false);

            // 注册属性改变时会触发的方法
            terrain.RegisterWithInitValue(terr => OnTerrainChanged(terr));
            timeMultiplier.RegisterWithInitValue(time => OnTimeMultiplierChanged(time));
            
            // 开始选择格子时
            this.RegisterEvent<SelectMapStartEvent>(e => OnSelectStart(e));
            // 结束选择格子
            this.RegisterEvent<SelectMapEndEvent>(e => OnSelectEnd(e));
        }


        private void OnTerrainChanged(int terr)
        {
            // 改变地形图片
            if (terr == (int) TerrainEnum.Empty)
            {
                Color tmpColor = srFloor.color;
                tmpColor.a = 0f;
                srFloor.color = tmpColor;
            }
            else
            {
                var sprite = Resources.Load<Sprite>(GridConst.TerrainResPathPrefix + terr);
                srFloor.sprite = sprite;   
            }
        }

        private void OnTimeMultiplierChanged(float time)
        {
            // todo 速度变化触发的效果
            
        }

        void OnEnter()
        {
            Debug.Log($">>boxgrid enter, {this.ToString()}");
            ShowHint("selected");
        }

        void OnExit()
        {
            HideHint();
        }

        void ShowHint(string hintType)
        {
            if(!srHint.gameObject.activeSelf)
                srHint.gameObject.SetActive(true);
            
            switch (hintType)
            {
                case "selected":
                    srHint.color = Color.yellow;
                    break;
                case "attackRange":
                    srHint.color = Color.blue;
                    break;
            }
        }

        void HideHint()
        {
            srHint.gameObject.SetActive(false);
        }

        void OnSelectStart(SelectMapStartEvent e)
        {
            // mouseHelper.enabled = true;
            touchArea.gameObject.SetActive(true);
        }

        void OnSelectEnd(SelectMapEndEvent e)
        {
            HideHint();
            touchArea.gameObject.SetActive(false);
        }

        /// <summary>
        /// 获取Architecture 每个IController都要写
        /// </summary>
        /// <returns></returns>
        public IArchitecture GetArchitecture()
        {
            return GameEntry.Interface;
        }

        // 输出一些信息
        public override string ToString()
        {
            return $"row: {row} col: {col} timeMultiplier: {timeMultiplier.Value} " +
                   $"terrain: {terrain}";
        }

        /// <summary>
        /// 检查格子是否为空
        /// </summary>
        /// <returns></returns>
        public bool IsEmpty()
        {
            return occupation == 0;
        }
    }
}