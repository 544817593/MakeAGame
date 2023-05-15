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
        public BindableProperty<TimeMultiplierEnum> timeMultiplier = new BindableProperty<TimeMultiplierEnum>();  // 时间流逝倍数
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
            mapSelectSystem = this.GetSystem<IMapSelectSystem>();
            
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
            timeMultiplier.RegisterWithInitValue(tm => OnTimeMultiplierChanged(tm));
            
            // 开始选择格子时
            this.RegisterEvent<SelectMapStartEvent>(e => OnSelectStart(e));
            // 结束选择格子
            this.RegisterEvent<SelectMapEndEvent>(e => OnSelectEnd(e));
        }


        private void OnTerrainChanged(int terr)
        {
            // 改变地形图片
            if (terr == (int) TerrainEnum.Invalid)
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

        private void OnTimeMultiplierChanged(TimeMultiplierEnum tm)
        {
            // todo 速度变化触发的效果
            
        }

        IMapSelectSystem mapSelectSystem;
        private bool isMouseEntered;
        void OnEnter()
        {
            isMouseEntered = true;
            
            // Debug.Log($">>boxgrid enter, {this.ToString()}");
            // Debug.Log($"enter direction: {mouseDirection}");
            // ShowHint("selected");

            mapSelectSystem.crtGrid.Value = this;
        }

        private void Update()
        {
            if (isMouseEntered)
            {
                UpdateMouseDirection();
            }
        }

        private int mouseDirection = -1;    // 鼠标在格子内的相对方位 0:左上 1:右上 2:左下 3:右下
        void UpdateMouseDirection()
        {
            int oldDirection = mouseDirection;
            var mousePos = Input.mousePosition;
            var gridScreenPos = Camera.main.WorldToScreenPoint(transform.position);
            // Debug.Log($"mousePos: {mousePos} gridScreenPos: {gridScreenPos}");
            bool isUp = mousePos.y > gridScreenPos.y;
            bool isLeft = mousePos.x < gridScreenPos.x;
            if (isUp && isLeft) mouseDirection = 0;
            else if (isUp && !isLeft) mouseDirection = 1;
            else if (!isUp && isLeft) mouseDirection = 2;
            else mouseDirection = 3;

            if (oldDirection != mouseDirection)
            {
                // Debug.Log($"check direction diff: old {oldDirection} new {mouseDirection}");
                mapSelectSystem.mouseDirection.Value = mouseDirection;
            }
        }

        void OnExit()
        {
            // HideHint();

            isMouseEntered = false;
            mouseDirection = -1;
            mapSelectSystem.crtGrid.Value = null;
            mapSelectSystem.mouseDirection.Value = mouseDirection;
        }

        public void ShowHint(string hintType)
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

        public void HideHint()
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