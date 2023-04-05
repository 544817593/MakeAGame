using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 负责地图的管理和创建
/// </summary>
public class MapManager : MonoBehaviour
{
    private static MapManager instance; // 地图管理器实例

    /// <summary>
    /// 地图管理器Getter
    /// </summary>
    public static MapManager Instance { get { return instance; } }

    public SOMapBase activeMap; // 当前地图
    [SerializeField]  private GameObject boxGridPrefab; // 格子的预设体
    [SerializeField]  private float spacing; // 格子间距

    private void Awake()
    {
        // 确保单例
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }

        // 把自身赋予GM
        GameManager.Instance.mapMan = this;
    }

    private void Start()
    {
        // 暂时测试用，场景管理器发送切换到战斗场景并加载完毕后调动InitMap
        InitMap();
    }

    /// <summary>
    /// 初始化地图
    /// </summary>
    public void InitMap()
    {
        activeMap.grid2DList = new List<List<EntityBoxGrid>>();
        for (int i = 0; i < activeMap.rows; i++)
        {
            var tmpGridList = new List<EntityBoxGrid>();
            activeMap.grid2DList.Add(tmpGridList);
        }
        GenerateMapGrid();
    }

    /// <summary>
    /// 生成地图里的格子
    /// </summary>
    private void GenerateMapGrid()
    {
        // 找到场景中放置格子的空物体
        Transform gridRoot = GameObject.Find("GridBoxes").GetComponent<Transform>();
        
        // 创建格子，并添加到地图物体里
        for (int row = 0; row < activeMap.rows; row++)
        {
            for (int col = 0; col < activeMap.cols; col++)
            {
                GameObject boxGrid = Instantiate(boxGridPrefab, gridRoot);
                boxGrid.GetComponent<EntityBoxGrid>().coordinate = (row, col);

                float posX = col * spacing;
                float posZ = row * -spacing;

                boxGrid.transform.position = new Vector3(posX, 0.1f, posZ);

                GameManager.Instance.mapMan.AddGridToList(boxGrid.GetComponent<EntityBoxGrid>(), row);
            }
        }

        // 调整偏差
        float gridW = (activeMap.cols - 2) * spacing;
        float gridH = activeMap.rows * spacing;
        gridRoot.position = new Vector3(-(gridW / 2 + spacing / 2), 0.1f, gridH / 2 - spacing / 2);

    }

    /// <summary>
    /// 把生成的格子放入grid2DList里
    /// </summary>
    /// <param name="grid"></param>
    /// <param name="row"></param>
    public void AddGridToList(EntityBoxGrid grid, int row)
    {
        activeMap.grid2DList[row].Add(grid);
    }
}
