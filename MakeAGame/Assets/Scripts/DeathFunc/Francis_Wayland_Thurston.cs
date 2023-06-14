using System.Collections.Generic;
using QFramework;
using UnityEngine;

namespace Game
{
    /// <summary>
    /// 在当前位置放置一个不可摧毁的障碍物，获得一张弗朗西斯。
    /// 传入选择的格子，摧毁卡牌，改变格子，生成美术障碍物，背包加入卡牌
    /// </summary>
    public class Francis_Wayland_Thurston: DeathFuncBase
    {
        public Francis_Wayland_Thurston()
        {
            area.width = 1;
            area.height = 1;
        }

        public override void OnExecute(List<BoxGrid> grids)
        {
            base.OnExecute(grids);
            Debug.Log("Francis_Wayland_Thurston");

            if (grids.Count != 1)
            {
                return;
            }

            foreach (var grid in grids)
            {
                grid.terrain.Value = (int) TerrainEnum.Wall; // 格子类型
                grid.setSrFloor(Resources.Load<Sprite>("Sprites/Grids/地砖")); // 更换障碍物资源
            }

            // 死面保护判定
            if (ItemController.Instance.deathDestroyProtection > 0)
            {
                this.GetSystem<IInventorySystem>().SpawnBagCardInBag(viewCard.card);
                ItemController.Instance.deathDestroyProtection -= 1;
            }

            this.GetSystem<IInventorySystem>().SpawnBagCardInBag(viewCard.card); // 加入背包
            this.GetSystem<IHandCardSystem>().SubCard(viewCard); // 摧毁卡牌
        }
    }
}