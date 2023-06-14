using System;
using System.Collections.Generic;
using QFramework;
using UnityEngine;
using UnityEngine.UIElements;

namespace Game
{
    public enum MapSelectStage
    {
        None,   // 没在选
        IsPutPiece, // 正在找地方放棋子
        IsPutDeathFunc, // 正在找地方放死面效果
        IsCheckPiece    // 正在选中某个棋子查看情况
    }
    
    public interface IMapSelectSystem : ISystem
    {
        MapSelectStage stage { get; }
        public BindableProperty<BoxGrid> crtGrid { get; set; }
        public BindableProperty<int> mouseDirection { get; set; }

        void SelectMapStart(SelectArea area);
        void SelectMapEnd(ViewCard viewCard);
    }
    
    public class MapSelectSystem: AbstractSystem, IMapSelectSystem
    {
        private IMapSystem mapSystem;
        
        // 当前地图选择状态
        public MapSelectStage stage { get; set; } = MapSelectStage.None;
        public BindableProperty<BoxGrid> crtGrid { get; set; } = new BindableProperty<BoxGrid>();
        public BindableProperty<int> mouseDirection { get; set; } = new BindableProperty<int>();
        public List<BoxGrid> selectedGrids = new List<BoxGrid>();
        public List<BoxGrid> validSelectedGrids = new List<BoxGrid>();

        private SelectArea areaInfo;
        
        protected override void OnInit()
        {
            mapSystem = this.GetSystem<IMapSystem>();
            
            // BindableProperty扩展测试
            // crtGrid.RegisterBeforeValueChanged((grid) =>
            // {
            //     string output = grid == null ? "null" : grid.ToString();
            //     Debug.Log($"crtGrid: BeforeValueChanged {output}");
            // });
            // crtGrid.RegisterWithInitValue((grid) =>
            // {
            //     string output = grid == null ? "null" : grid.ToString();
            //     Debug.Log($"crtGrid: OnValueChanged {output}");
            // });
            mouseDirection.RegisterWithInitValue(dir => OnGridUpdate());
        }

        void OnGridUpdate()
        {
            // 隐藏上一次高亮
            foreach (var grid in validSelectedGrids)
            {
                grid.HideHint();
            }
            
            selectedGrids.Clear();
            validSelectedGrids.Clear();
            
            // 拦住数据没准备好的情况
            if (crtGrid.Value == null || mouseDirection == -1)
                return;
            
            Debug.Log($"mouseDirection: {mouseDirection} grid: {crtGrid.Value}");

            // 以悬浮格子为中心的选取方法
             // bound均为可取值，即 0 <= bound1 <= 范围 <= bound2 <= mapR-1
             int boundLeft = crtGrid.Value.col;
             int boundRight = crtGrid.Value.col;
             int boundUp = crtGrid.Value.row;
             int boundDown = crtGrid.Value.row;

             int mapR = mapSystem.mapRow;
             int mapC = mapSystem.mapCol;

             // 范围宽为奇数，向两边扩展同等宽度
             if (areaInfo.width % 2 == 1)
             {
                 int distW = (areaInfo.width - 1) / 2;

                 int originX = crtGrid.Value.col - distW;
                 boundLeft = originX < 0 ? 0 : originX;
                 boundRight = (crtGrid.Value.col + distW) > (mapC - 1) ? (mapC - 1) : crtGrid.Value.col + distW;
             }
             // 范围宽为偶数
             else
             {
                 int distW = areaInfo.width / 2;
                 
                 // 以格子左边界为中轴线，当前悬浮格子为范围中的第(width/2)+1个
                 if (mouseDirection % 2 == 0)
                 {
                     int originX = crtGrid.Value.col - distW;
                     boundLeft = originX < 0 ? 0 : originX;
                     boundRight = (crtGrid.Value.col + distW - 1) > (mapC - 1) ? (mapC - 1) : crtGrid.Value.col + distW - 1;
                 }
                 // 以格子右边界为中轴线，当前悬浮格子为范围中的第width/2个
                 else
                 {
                     int originX = crtGrid.Value.col - distW + 1;
                     boundLeft = originX < 0 ? 0 : originX;
                     boundRight = (crtGrid.Value.col + distW) > (mapC - 1) ? (mapC - 1) : crtGrid.Value.col + distW;
                 }
             }

             // 范围高为奇数，向两边扩展同等高度
             if (areaInfo.height % 2 == 1)
             {
                 int distH = (areaInfo.height - 1) / 2;

                 int originY = crtGrid.Value.row - distH;
                 boundUp = originY < 0 ? 0 : originY;
                 boundDown = (crtGrid.Value.row + distH) > (mapR - 1) ? (mapR - 1) : crtGrid.Value.row + distH;
             }
             // 范围高为偶数
             else
             {
                 int distH = areaInfo.height / 2;
                 
                 // 以格子上边界为中轴线，当前悬浮格子为范围中的第(height/2)+1个
                 if (mouseDirection < 2)
                 {
                     int originY = crtGrid.Value.row - distH;
                     boundUp = originY < 0 ? 0 : originY;
                     boundDown = (crtGrid.Value.row + distH - 1) > (mapR - 1) ? (mapR - 1) : crtGrid.Value.row + distH - 1;
                 }
                 // 以格子下边界为中轴线，当前悬浮格子为范围中的第height/2个
                 else
                 {
                     int originY = crtGrid.Value.row - distH + 1;
                     boundUp = originY < 0 ? 0 : originY;
                     boundDown = (crtGrid.Value.row + distH) > (mapR - 1) ? (mapR - 1) : crtGrid.Value.row + distH;
                 }
             }

             // 从左到右、从上到下存储格子
             for (int r = boundUp; r <= boundDown; r++)
             {
                 for (int c = boundLeft; c <= boundRight; c++)
                 {
                     selectedGrids.Add(mapSystem.Grids()[r,c]);
                 }
             }
             
             PrintSelectedGrids();
             
             // 二次筛选
             validSelectedGrids.Clear();
             foreach (var grid in selectedGrids)
             {
                 // 上方已经有其他棋子
                 if (!grid.IsEmpty()) {}
                 else
                 {
                     validSelectedGrids.Add(grid);
                 }
             }

             foreach (var grid in validSelectedGrids)
             {
                 grid.ShowHint("selected");
             }
        }

        void PrintSelectedGrids()
        {
            string ret = "SelectedGrids:\n";
            foreach (var grid in selectedGrids)
            {
                ret += grid.ToString() + "\n";
            }
            
            Debug.Log(ret);
        }

        public void SelectMapStart(SelectArea area)
        {
            Debug.Log($"MapSelectSystem: SelectMapStart, area w: {area.width} h: {area.height}");
            stage = MapSelectStage.IsPutPiece;
            areaInfo = area;
        }

        public void SelectMapEnd(ViewCard viewCard)
        {
            Debug.Log("MapSelectSystem: SelectMapEnd");
            stage = MapSelectStage.None;
            
            // 判断是否成功放置棋子
            // 1.格子数量
            bool isGridCountCorrect = validSelectedGrids.Count == areaInfo.width * areaInfo.height;
            // 2.蓝是否足够
            int crtSan = UIKit.GetPanel<UIHandCard>().crtSan;
            bool isSanEnough = viewCard.card.sanCost <= crtSan;
            if (isGridCountCorrect && isSanEnough)
            {
                Debug.Log("it's ok to put piece");
                PutPieceByHandCardEvent e = new PutPieceByHandCardEvent()
                    {viewCard = viewCard, pieceGrids = validSelectedGrids};
                this.SendEvent<PutPieceByHandCardEvent>(e);
            }
            else
            {
                Debug.Log(
                    $"put piece failed, ret1: {isGridCountCorrect} ret2: {isSanEnough}");
            }

            crtGrid.Value = null;
            mouseDirection.Value = -1;
            selectedGrids.Clear();
            validSelectedGrids.Clear();
            
            this.SendEvent<SelectMapEndEvent>();
        }
        

        public IArchitecture GetArchitecture()
        {
            return GameEntry.Interface;
        }
    }

    public struct SelectArea
    {
        public int width;
        public int height;
        public List<int> pattern;

        public string ToString => $"w: {width} h: {height}";
    }

    #region 旧的rangeselector

//     using System.Collections.Generic;
// using GridSelectTest;
// using Unity.Mathematics;
// using UnityEngine;
//
//     /// <summary>
//     /// 描述选择范围的各项参数，需要在开始选择前传入RangeSelector
//     /// <param name="width">范围的宽（若不指定，将根据patterni计算）</param>
//     /// <param name="height">范围的高（若不指定，将根据patterni计算）</param>
//     /// <param name="pattern">若有图案，在此处传入以左上角格子为(0,0)的int2列表</param>
//     /// <param name="gridStatusTypeFilter">若要根据格子的StatusType进行筛选，在此处传入要保留的StatusType</param>
//     /// </summary>
//     public class SelectAreaArgs
//     {
//         public int width; // 范围的宽
//         public int height; // 范围的长
//         public List<int2> pattern; // 若有指定图案，在此处传入以左上角格子为(0,0)的坐标集
//         public List<int> gridStatusTypeFilter; // 若要根据格子的StatusType进行筛选，在此处传入需要的StatusType
//         public bool isLifeCard; // 是否在选择放置生面牌的位置
//
//         public string content => $"width: {width}, height: {height}, pattern: {pattern}," +
//                                  $"gridStatusTypeFilter: {gridStatusTypeFilter}, isLifeCard: {isLifeCard}";
//     }
//
//     /// <summary>
//     /// 地图格子范围选择器
//     /// </summary>
//     public class RangeSelector
//     {
//         public enum SelectMethod
//         {
//             ByRange = 0, // 地图边界筛选，根据宽和高，选取一个矩形范围内的格子（一定会经过这道筛选）
//             ByPattern = 1 << 0, // 根据图案，选取宽高矩形内符合图案的格子
//             ByStatusType = 1 << 1, // 选取指定StatusType的格子
//             ByPatternAndStatusType = ByPattern | ByStatusType
//         }
//
//         public static int width = 1;
//         public static int height = 1;
//         public static List<int2> pattern = new List<int2>();
//         public static List<int> gridStatusTypeFilter = new List<int>();
//         public static RangeSelector.SelectMethod selectMethod = RangeSelector.SelectMethod.ByRange;
//         public static bool isLifeCard;
//
//         public static int originX; // 范围左上角坐标（可能超出边界）
//         public static int originY;
//         public static List<GridMouseHelper> selectedGrids = new List<GridMouseHelper>();
//
//         public static void OnReceiveSelectAreaArgs(SelectAreaArgs args, RangeSelector.SelectMethod method)
//         {
//             Debug.Log($"RangeSelector receive new args: {args.content}");
//             // 如果没有赋值长宽，尝试根据pattern进行赋值
//             if (args.width == 0 || args.height == 0)
//             {
//                 // 如果没有pattern，那就是混沌牌，没有范围
//                 if (args.pattern != null)
//                 {
//                     foreach (var pos in args.pattern)
//                     {
//                         if (pos.x + 1 > args.width)
//                         {
//                             args.width = pos.x + 1;
//                         }
//
//                         if (pos.y + 1 > args.height)
//                         {
//                             args.height = pos.y + 1;
//                         }
//                     }   
//                 }
//             }
//
//             // 列表如果没有初始化，会不会空值报错啊？  // 目前还没遇到过
//             width = args.width;
//             height = args.height;
//             pattern = args.pattern;
//             gridStatusTypeFilter = args.gridStatusTypeFilter;
//             isLifeCard = args.isLifeCard;
//
//             selectMethod = method;
//         }
//
//         /// <summary>
//         /// <para>重新进行选择，更新格子列表</para>
//         /// <para>可能的触发时机：</para>
//         /// <list type="number">
//         /// <item>鼠标跨过格子边界时</item>
//         /// <item>鼠标跨过格子内部横竖中间线时</item>
//         /// </list>
//         /// </summary>
//         public static void UpdateSelectedGrids()
//         {
//             if (!GameManager.Instance.isChoosingTarget && !GameManager.Instance.isDragingCard)
//             {
//                 return;
//             }
//             
//             ClearSelectedGrids();
//
//             // 无论何种选择方法，都需要先用地图边界筛一遍
//             if (selectMethod >= 0)
//             {
//                 SelectByMapBorder();
//             }
//
//             if ((selectMethod & RangeSelector.SelectMethod.ByRange) > 0)
//             {
//             }
//
//             if ((selectMethod & RangeSelector.SelectMethod.ByPattern) > 0)
//             {
//                 SelectByPattern();
//             }
//
//             if ((selectMethod & RangeSelector.SelectMethod.ByStatusType) > 0)
//             {
//                 SelectByStatusType();
//             }
//             
//             // 新规则：生面牌只能放置在时间流速为1的格子
//             if (isLifeCard)
//             {
//                 SelectByLifeCard();
//             }
//
//             HighlightSelectedGrids();
//         }
//
//         /// <summary>
//         /// <para>通过地图边界筛选，可以部分选择</para>
//         /// <param name="width">选择范围宽度</param>
//         /// <param name="height">选择范围高度</param>
//         /// </summary>
//         /// <returns>更新selected数组，将所有可选格子放入其中</returns>
//         public static bool SelectByMapBorder()
//         {
//             if (ReferenceEquals(GameManager.Instance.mouseOverGrid, null))
//             {
//                 return false;
//             }
//
//             int startR = GameManager.Instance.mouseOverGrid.mouseHelper.row;
//             int startC = GameManager.Instance.mouseOverGrid.mouseHelper.col;
//             int mapR = GameManager.Instance.mapMan.rows;
//             int mapC = GameManager.Instance.mapMan.cols;
//
//             // 以悬浮格子为中心的选取方法
//             // bound均为可取值，即 0 <= bound1 <= 范围 <= bound2 <= mapR-1
//             int boundLeft = startC;
//             int boundRight = startC;
//             int boundUp = startR;
//             int boundDown = startR;
//
//             // 范围宽为奇数，向两边扩展同等宽度
//             if (width % 2 == 1)
//             {
//                 int distW = (width - 1) / 2;
//
//                 originX = startC - distW;
//                 boundLeft = originX < 0 ? 0 : originX;
//                 boundRight = (startC + distW) > (mapC - 1) ? (mapC - 1) : startC + distW;
//             }
//             // 范围宽为偶数
//             else
//             {
//                 int distW = width / 2;
//
//                 // 具体究竟指在哪个格子（以哪根线为中轴线）需要重新计算
//                 // Vector3 hoverGridScreenPos =
//                 //     Camera.main.WorldToScreenPoint(GameManager.inst.MouseHoverGrid.transform.position);
//                 GridMouseHelper hoverGrid =
//                     GameManager.Instance.mouseOverGrid.mouseHelper;
//                 // 以格子左边界为中轴线，当前悬浮格子为范围中的第(width/2)+1个
//                 if (hoverGrid.IsMousePosLeft())
//                 {
//                     originX = startC - distW;
//                     boundLeft = originX < 0 ? 0 : originX;
//                     boundRight = (startC + distW - 1) > (mapC - 1) ? (mapC - 1) : startC + distW - 1;
//                 }
//                 // 以格子右边界为中轴线，当前悬浮格子为范围中的第width/2个
//                 else
//                 {
//                     originX = startC - distW + 1;
//                     boundLeft = originX < 0 ? 0 : originX;
//                     boundRight = (startC + distW) > (mapC - 1) ? (mapC - 1) : startC + distW;
//                 }
//             }
//
//             // 范围高为奇数，向两边扩展同等高度
//             if (height % 2 == 1)
//             {
//                 int distH = (height - 1) / 2;
//
//                 originY = startR - distH;
//                 boundUp = originY < 0 ? 0 : originY;
//                 boundDown = (startR + distH) > (mapR - 1) ? (mapR - 1) : startR + distH;
//             }
//             // 范围高为偶数
//             else
//             {
//                 int distH = height / 2;
//
//                 // 具体究竟指在哪个格子（以哪根线为中轴线）需要重新计算
//                 // Vector3 hoverGridScreenPos =
//                 //     Camera.main.WorldToScreenPoint(GameManager.inst.MouseHoverGrid.transform.position);
//                 GridMouseHelper hoverGrid =
//                     GameManager.Instance.mouseOverGrid.mouseHelper;
//                 // 以格子上边界为中轴线，当前悬浮格子为范围中的第(height/2)+1个
//                 if (hoverGrid.IsMousePosUp())
//                 {
//                     originY = startR - distH;
//                     boundUp = originY < 0 ? 0 : originY;
//                     boundDown = (startR + distH - 1) > (mapR - 1) ? (mapR - 1) : startR + distH - 1;
//                 }
//                 // 以格子下边界为中轴线，当前悬浮格子为范围中的第height/2个
//                 else
//                 {
//                     originY = startR - distH + 1;
//                     boundUp = originY < 0 ? 0 : originY;
//                     boundDown = (startR + distH) > (mapR - 1) ? (mapR - 1) : startR + distH;
//                 }
//             }
//
//             // 从左到右、从上到下存储格子
//             for (int r = boundUp; r <= boundDown; r++)
//             {
//                 for (int c = boundLeft; c <= boundRight; c++)
//                 {
//                     selectedGrids.Add(GameManager.Instance.mapMan.grid2DList[r][c].mouseHelper);
//                 }
//             }
//
//             return true;
//         }
//
//
//         // 给二维数组+行列范围
//         public static void SelectByPattern()
//         {
//             if (ReferenceEquals(GameManager.Instance.mouseOverGrid, null))
//             {
//                 return;
//             }
//
//             // 没有图案可筛选
//             if (pattern == null || pattern.Count == 0)
//             {
//                 return;
//             }
//
//             // 先根据范围进行筛选
//             SelectByMapBorder();
//
//             // 再根据图案进行筛选
//             // 还是以左上角格子为基点，存储偏移量（因为偶数长宽时不存在中心格子）
//             // 由于图案数组和已选择格子数组都是从上到下，因此一一比对即可，不用每找一个格子都遍历一次整个数组
//             // 但因为比对过程中会丢掉数组元素，正向遍历会出错，所以应该从后往前遍历
//             // 先找到第一个合法的（在地图内的）图案格子
//             int patternIndex = pattern.Count - 1;
//             patternIndex = GetNextValidPatternIndex(patternIndex);
//             // 若图案全不在地图内，清空selected数组
//             if (patternIndex < 0)
//             {
//                 ClearSelectedGrids();
//                 return;
//             }
//
//             for (int i = selectedGrids.Count - 1; i >= 0; i--)
//             {
//                 var grid = selectedGrids[i];
//
//                 // 图案已经比对完毕，剩下的全部丢掉
//                 if (patternIndex < 0)
//                 {
//                     selectedGrids.Remove(grid);
//                     continue;
//                 }
//
//                 // 当前格子属于图案，继续比对
//                 if (grid.col == originX + pattern[patternIndex].x && grid.row == originY + pattern[patternIndex].y)
//                 {
//                     patternIndex--;
//                     patternIndex = GetNextValidPatternIndex(patternIndex);
//                 }
//                 // 不是图案格子，丢出selected数组
//                 else
//                 {
//                     selectedGrids.Remove(grid);
//                 }
//             }
//         }
//
//         public static void SelectByStatusType()
//         {
//             // 没有StatusType可筛选
//             if (gridStatusTypeFilter == null || gridStatusTypeFilter.Count == 0)
//             {
//                 return;
//             }
//
//             for (int i = selectedGrids.Count - 1; i >= 0; i--)
//             {
//                 if (!gridStatusTypeFilter.Contains(selectedGrids[i].grid.statusType))
//                 {
//                     selectedGrids.RemoveAt(i);
//                 }
//             }
//         }
//         
//         public static void SelectByLifeCard()
//         {
//             for (int i = selectedGrids.Count - 1; i >= 0; i--)
//             {
//                 if (selectedGrids[i].grid.TimeMultiplier != 1f)
//                 {
//                     selectedGrids.RemoveAt(i);
//                 }
//             }
//         }
//
//         // 传入当前检查序号，从后往前检查，返回下一个合法图案格子的序号（包含当前序号）
//         public static int GetNextValidPatternIndex(int index)
//         {
//             if (index < 0)
//             {
//                 return -1;
//             }
//
//             while (!IsGridPosInMap(originX + pattern[index].x, originY + pattern[index].y))
//             {
//                 index--;
//
//                 // 图案格子全不在可选范围，清空selected数组返回
//                 if (index < 0)
//                 {
//                     return -1;
//                 }
//             }
//
//             return index;
//         }
//
//         public static bool IsGridPosInMap(int x, int y)
//         {
//             int mapR = GameManager.Instance.mapMan.rows;
//             int mapC = GameManager.Instance.mapMan.cols;
//             return x >= 0 && x < mapC && y >= 0 && y < mapR;
//         }
//
//         public static void HighlightSelectedGrids()
//         {
//             foreach (var grid in selectedGrids)
//             {
//                 grid.Highlight();
//             }
//         }
//
//         public static void ClearSelectedGrids()
//         {
//             foreach (var grid in selectedGrids)
//             {
//                 grid.Dehighlight();
//             }
//
//             selectedGrids.Clear();
//         }
//
//     }

    #endregion
}