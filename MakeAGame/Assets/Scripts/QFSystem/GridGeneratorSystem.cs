using QFramework;
using UnityEngine;

namespace Game
{
    public interface IGridGeneratorSystem : ISystem
    {
        BoxGrid CreateGrid(int r, int c, Transform root);
    }
    
    /// <summary>
    /// 实例化地图格子相关
    /// </summary>
    public class GridGeneratorSystem : AbstractSystem, IGridGeneratorSystem
    {
        private GameObject mGridPrefab; // 格子prefab
        private float mSpacing = 1.44f;  // 格子间距    // todo 自动化

        protected override void OnInit()
        {
            mGridPrefab = (GameObject) Resources.Load("Prefabs/Grid");
        }

        /// <summary>
        /// 实例化格子，赋值部分属性
        /// </summary>
        /// <param name="r">行</param>
        /// <param name="c">列</param>
        /// <param name="root">地图父节点</param>
        /// <returns></returns>
        public BoxGrid CreateGrid(int r, int c, Transform root)
        {
            var gridGO = GameObject.Instantiate(mGridPrefab, root);
            gridGO.transform.localPosition = new Vector3(c * mSpacing, (-1) * r * mSpacing);

            var grid = gridGO.GetComponent<BoxGrid>();
            grid.row = r;
            grid.col = c;

            return grid;
        }
    }
}