using QFramework;
using UnityEngine;

namespace Game
{
    public interface ICardGeneratorSystem : ISystem
    {
        GameObject mCardPrefab { get; }

        // 单纯创建卡牌实体
        GameObject CreateCard();
        // 创建已经挂上ViewBagCard的卡牌实体
        ViewBagCard CreateBagCard(Card cardData);
    }
    
    public class CardGeneratorSystem: AbstractSystem, ICardGeneratorSystem
    {
        public GameObject mCardPrefab { get; private set; }

        protected override void OnInit()
        {
            mCardPrefab = (GameObject) Resources.Load("Prefabs/CardItem");
        }

        public GameObject CreateCard()
        {
            // 这里只是生成game object，返回后再根据卡牌所处位置（手牌？背包）挂载对应组件
            var cardGO = GameObject.Instantiate(mCardPrefab);
            return cardGO;
        }

        public ViewBagCard CreateBagCard(Card cardData)
        {
            var cardGO = CreateCard();
            
            var viewBagCard = cardGO.AddComponent<ViewBagCard>();
            viewBagCard.card = cardData;
            
            return viewBagCard;
        }

        public IArchitecture GetArchitecture()
        {
            return GameEntry.Interface;
        }
    }
}