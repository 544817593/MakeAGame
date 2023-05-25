using UnityEngine;
using UnityEngine.UI;
using QFramework;
using System.Collections.Generic;
using Game;
using System;
using TMPro;
using UnityEngine.Assertions;
using Unity.VisualScripting;

namespace ShopEnhanceUI
{
	public class ShopEnhanceUIData : UIPanelData
	{
        public IShopSystem shopSystem = GameEntry.Interface.GetSystem<IShopSystem>();
        public ICardGeneratorSystem cardGeneratorSystem = GameEntry.Interface.GetSystem<ICardGeneratorSystem>();
    }
	public partial class ShopEnhanceUI : UIPanel
	{
        // TODO 读取玩家当前金币数量，暂时使用hardcode
        public int playerGold = 50;
        // 背包每页的格子数量上限
        private const int gridNum = 10;
        private int curPage = 1;
        private int totalPage = 1;
        // 每页显示的元素索引区间[lowerIndex, upperIndex]
        private int upperIndex = gridNum - 1;
        private int lowerIndex = 0;
        private enum BagTabs //背包分为卡牌页，道具页
        {
            card,
            item
        }
        private int curBagTab = (int) BagTabs.card; // 默认当前页是卡牌
        private int cardCount = 12; // 测试用，想要创建多少张牌
        private ViewBagCard curCardBeforeEnhance = null; // 作为展示使用
        private ViewBagCard curCardBeforeEnhanceShallowCopy = null; // 作为实际强化的牌使用
        private ViewBagCard curCardAfterEnhance = null; // 作为强化后的预览使用
        //private Button curEnhanceItemBtn = null;
        private Item curEnhanceItem = null;
        //private int sellCount = 1;
        //private Item selectedItem = null;
        //private Button selectedButton = null;
        private ItemController itemController = new ItemController(); //不确定是不是这样用
        //private Dictionary<Button, ViewBagCard> cardBtn = new Dictionary<Button, ViewBagCard>();

        private List<ViewBagCard> bagCardList = new List<ViewBagCard>();	// 手牌列表

        protected override void OnInit(IUIData uiData = null)
		{
			mData = uiData as ShopEnhanceUIData ?? new ShopEnhanceUIData();
            // please add init code here
            
            ShopPanelChange.changeShopPanel(this, Close);

            initCards();
            refreshLayout();
            pageChange();
            tabChange();
            btnClearListen();
            enhanceListen();
            
        }
		
		protected override void OnOpen(IUIData uiData = null)
		{
		}
		
		protected override void OnShow()
		{
		}
		
		protected override void OnHide()
		{
		}
		
		protected override void OnClose()
		{
            foreach (var viewBagCard in bagCardList)
            {
                // 面板关闭时，在这里撤销点击卡牌会触发的事件，避免卡牌在其他面板还会触发
                viewBagCard.OnTouchAction = null;
            }
            
		}
        /// <summary>
        /// 初始化卡牌
        /// </summary>
        public void initCards()
        {
            Assert.IsNotNull(GameObject.Find("BagPanel"));
            
            for (int i = 1; i <= cardCount; ++i)
            {
                Card testData = new Card(1);
                ViewBagCard viewBagCard = mData.cardGeneratorSystem.CreateBagCard(testData);
                bagCardList.Add(viewBagCard);
                // 接收数据，初始化牌面显示
                viewBagCard.card.enhanceID = i - 1;
                viewBagCard.gameObject.transform.SetParent(BagPanel.transform);
                viewBagCard.OnTouchAction = () =>
                {
                    Debug.Log($"Click card enhanceID: {viewBagCard.card.enhanceID}");
                    // 如果已经有卡牌在强化位置，先删除
                    foreach(Transform transform in CardBeforeEnhance.GetComponentInChildren<Transform>(includeInactive: true))
                    {
                        Destroy(transform.gameObject);
                    }
                    ViewBagCard copy = mData.cardGeneratorSystem.CreateBagCard(testData); 
                    copy.gameObject.transform.SetParent(CardBeforeEnhance);
                    copy.gameObject.transform.position = CardBeforeEnhance.position;
                    curCardBeforeEnhance = copy;
                    curCardBeforeEnhanceShallowCopy = viewBagCard;
                    // 如果已经有强化道具在位置上，生成强化后的卡牌预览图
                    if(curEnhanceItem != null)
                    {
                        generateCardAfterEnhance();
                    }
                };
            }
        }
        /// <summary>
        /// 更新页面布局
        /// </summary>
        private void refreshLayout()
        {
            if (curBagTab == (int)BagTabs.card)
            {
                totalPage = bagCardList.Count != 0 ? (int)Math.Ceiling((double)bagCardList.Count / gridNum) : 1;
                // 页数显示
                TextPageNum.text = $" {curPage} / {totalPage}";
                int idx = 0;
                foreach (ViewBagCard viewBagCard in bagCardList)
                {
                    if(idx >= lowerIndex && idx <= upperIndex)
                    {
                        viewBagCard.gameObject.SetActive(true);
                    }
                    else
                    {
                        //Debug.Log($"idx : {idx}, low: {lowerIndex}, high: {upperIndex}");
                        viewBagCard.gameObject.SetActive(false);
                    }
                    idx++;
                }
            }
            else if (curBagTab == (int)BagTabs.item)
            {
                List<Item> bagItemList = mData.shopSystem.getBagItemList();
                totalPage = bagItemList.Count != 0 ? (int)Math.Ceiling((double)bagItemList.Count / gridNum) : 1;
                // 页数显示
                TextPageNum.text = $" {curPage} / {totalPage}";
                int idx = lowerIndex;
                foreach (Transform transform in BagPanel.GetComponentInChildren<Transform>(includeInactive: true))
                {
                    // 跳过ViewBagCard，只找item button
                    if (transform.GetComponent<ViewBagCard>() != null) continue;
                    GameObject curItem = transform.gameObject;
                    // bagItemList已遍历完，未填充物品的格子不显示，并且重置text和sprite
                    if (idx < bagItemList.Count)
                    {
                        Item itemInList = bagItemList[idx];
                        curItem.SetActive(true);
                        curItem.GetComponent<Image>().sprite = itemInList.data.sprite;
                        foreach (Transform texts in curItem.GetComponentInChildren<Transform>())
                        {
                            //Debug.Log(texts.gameObject.name);
                            if (texts.gameObject.name == "ItemNum")
                            {
                                texts.gameObject.GetComponent<TextMeshProUGUI>().text = itemInList.amount.ToString();
                            }
                        }
                        // 监听物品按钮
                        curItem.GetComponent<Button>().onClick.AddListener(() =>
                        {
                            Debug.Log($"点击了{itemInList.data.name}");
                            EnhanceItem.gameObject.SetActive(true);
                            EnhanceItem.GetComponent<Image>().sprite = itemInList.data.sprite;
                            //curEnhanceItemBtn = curItem.GetComponent<Button>();
                            curEnhanceItem = itemInList;
                            // 如果已经有卡牌在强化位置，生成强化后的卡牌预览图
                            if(curCardBeforeEnhance != null)
                            {
                                generateCardAfterEnhance();
                            }
                        });
                    }
                    else
                    {
                        curItem.GetComponent<Image>().sprite = null;
                        foreach (Transform texts in curItem.GetComponentInChildren<Transform>())
                        {
                            //Debug.Log(texts.gameObject.name);
                            if (texts.gameObject.name == "ItemNum")
                            {
                                texts.gameObject.GetComponent<TextMeshProUGUI>().text = null;
                            }
                        }
                        curItem.SetActive(false);
                    }
                    idx++;
                }
            }
        }
        /// <summary>
        /// 更新当前背包页的index区间
        /// </summary>
        private void updateIndex()
        {
            if(curBagTab == (int)BagTabs.card)
            {
                int BagCardCount = bagCardList.Count;
                lowerIndex = (curPage - 1) * gridNum;
                // 索引上限为当前页*格子数量-1，如果超过list大小，则为list的元素数量-1
                upperIndex = curPage * gridNum - 1 >= BagCardCount ? BagCardCount - 1 : curPage * gridNum - 1;
            }
            else if(curBagTab == (int)BagTabs.item)
            {
                int bagItemCount = mData.shopSystem.getBagItemList().Count;
                lowerIndex = (curPage - 1) * gridNum;
                // 索引上限为当前页*格子数量-1，如果超过list大小，则为list的元素数量-1
                upperIndex = curPage * gridNum - 1 >= bagItemCount ? bagItemCount - 1 : curPage * gridNum - 1;
            }
        }
        /// <summary>
        /// 背包页面切换监听
        /// </summary>
        private void pageChange()
        {
            BtnNextPage.onClick.AddListener(() =>
            {
                if (curPage >= totalPage)
                {
                    Debug.Log("无法往后翻页");
                }
                else
                {
                    curPage++;
                    updateIndex();
                    refreshLayout();
                }

            });
            BtnPrePage.onClick.AddListener(() =>
            {
                if (curPage == 1)
                {
                    Debug.Log("无法往前翻页");
                }
                else
                {
                    curPage--;
                    updateIndex();
                    refreshLayout();
                }
            });
        }
        /// <summary>
        /// 背包栏按钮监听
        /// </summary>
        private void tabChange()
        {
            BtnCard.onClick.AddListener(() =>
            {
                inactiveObjects();
                curPage = 1;
                curBagTab = (int)BagTabs.card;
                
                updateIndex();
                refreshLayout();
            });
            BtnItem.onClick.AddListener(() => 
            {
                inactiveObjects();
                curPage = 1;
                curBagTab = (int)BagTabs.item;
                updateIndex();
                refreshLayout();
            });
        }
        /// <summary>
        /// 每次切换背包卡牌和道具调用，将所有BagPanel内的gameobject隐藏
        /// </summary>
        private void inactiveObjects()
        {
            foreach (Transform transform in BagPanel.GetComponentInChildren<Transform>(includeInactive: true))
            {
                transform.gameObject.SetActive(false);
            }
        }
        /// <summary>
        /// 强化按钮监听
        /// </summary>
        private void enhanceListen()
        {
            ButtonEnhance.onClick.AddListener(() =>
            {
                if(curCardBeforeEnhance == null)
                {
                    Debug.Log("请放置强化卡牌");
                }
                else if(curEnhanceItem == null)
                {
                    Debug.Log("请放置强化道具");
                }
                else
                {
                    Debug.Log($"{curCardBeforeEnhance.card.enhancement}, {curEnhanceItem.data.enhanceLevel}");
                    if (curCardBeforeEnhance.card.enhancement != curEnhanceItem.data.enhanceLevel)
                    {
                        Debug.Log("卡牌等级与强化道具不匹配");
                        
                    }
                    // TODO: 根据卡牌特性也需要匹配道具的要求
                    else
                    {
                        Debug.Log($"强化了 card: {curCardBeforeEnhance.name}, item: {curEnhanceItem.data.name}");
                        UseItemEvent e = new UseItemEvent();
                        e.item = curEnhanceItem;
                        e.viewBagCard = curCardBeforeEnhanceShallowCopy;
                        itemController.OnUseMerchantItem(e);

                        // 物品数量更新
                        curEnhanceItem.amount--;
                        if (curEnhanceItem.amount == 0)
                        {
                            mData.shopSystem.getBagItemList().Remove(curEnhanceItem);
                            if (mData.shopSystem.getBagItemList().Count % gridNum == 0 && curPage != 1 && curPage == totalPage)
                            {
                                curPage--;
                            }
                        }

                        // 清空强化面板
                        clearEnhancePanel();

                        //更新布局
                        updateIndex();
                        refreshLayout();
                        
                    }

                }
            });
        }
        /// <summary>
        /// 清空按钮监听
        /// </summary>
        private void btnClearListen()
        {
            ButtonClear.onClick.AddListener(() =>
            {
                clearEnhancePanel();
            });
        }
        private void clearEnhancePanel()
        {
            foreach (Transform t in CardBeforeEnhance.GetComponentInChildren<Transform>(includeInactive: true))
            {
                Destroy(t.gameObject);
            }
            foreach (Transform transform in CardAfterEnhance.GetComponentInChildren<Transform>(includeInactive: true))
            {
                Destroy(transform.gameObject);
            }
            curCardBeforeEnhance = null;
            curCardBeforeEnhanceShallowCopy = null;
            EnhanceItem.GetComponent<Image>().sprite = null;
            EnhanceItem.gameObject.SetActive(false);
            curEnhanceItem = null;
            curCardAfterEnhance = null;
        }

        private void generateCardAfterEnhance()
        {
            if (curEnhanceItem != null && curCardBeforeEnhance != null && curCardBeforeEnhanceShallowCopy != null)
            {
                // 如果已经有卡牌在强化后的位置，先删除
                foreach (Transform transform in CardAfterEnhance.GetComponentInChildren<Transform>(includeInactive: true))
                {
                    Destroy(transform.gameObject);
                }
                curCardAfterEnhance = mData.cardGeneratorSystem.CreateBagCard(Extensions.GetCopy(curCardBeforeEnhanceShallowCopy.card));
                curCardAfterEnhance.gameObject.transform.SetParent(CardAfterEnhance);
                curCardAfterEnhance.gameObject.transform.position = CardAfterEnhance.position;
                UseItemEvent e = new UseItemEvent { item = curEnhanceItem , viewBagCard = curCardBeforeEnhance };
                //e.item = curEnhanceItem;;
                //e.viewBagCard = curCardBeforeEnhance;
                Debug.Log($"{e.viewBagCard.name}");
                itemController.OnUseMerchantItem(e);
            }
        }
        
    }
}
