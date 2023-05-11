using QFramework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Game
{
    public interface IShopSystem : ISystem
    {
        
    }
    public class ShopSystem : AbstractSystem, IShopSystem
    {
        public BindableProperty<List<Item>> shopItemList = new BindableProperty<List<Item>>(); // 物品列表
        protected override void OnInit()
        {
            //shopItemList
        }
    }
}

