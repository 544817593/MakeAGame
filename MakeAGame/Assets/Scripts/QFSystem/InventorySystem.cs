using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public interface IInventorySystem : ISystem
    {

    }

    public class InventorySystem : AbstractSystem, IInventorySystem
    {
        public List<Item> itemList;

        protected override void OnInit()
        {
            itemList = new List<Item>();
            AddItem(new Item { itemType = ItemTypeEnum.Enhancement, amount = 1 });
            AddItem(new Item { itemType = ItemTypeEnum.Potion, amount = 1 });

        }

        public void AddItem(Item item)
        {
            itemList.Add(item);
        }

        public List<Item> GetItemList()
        {
            return itemList;
        }
    }
}

