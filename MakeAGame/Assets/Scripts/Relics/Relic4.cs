using UnityEngine;

namespace Game
{
    // 过滤罐
    public class Relic4: RelicBase
    {
        public Relic4(SORelic so): base(so)
        {
        }

        private float goldNum => crtParams[0];
        
        public override void Activate(IRelicSystem sys)
        {
            sys.RegisterRelicEvent<CombatVictoryEvent>(this, TakeEffect);
        }

        protected override void TakeEffect(object obj)
        {
            var pieceSystem = GameEntry.Interface.GetSystem<IPieceSystem>();
            var friendsOnMap = pieceSystem.pieceFriendList;

            if (friendsOnMap.Count != 0)
            {
                CardPackEnum pack = IdToSO.FindCardSOByID(friendsOnMap[0].card.charaID).cardPack;
                foreach (var friend in friendsOnMap)
                {
                    var crtPack = IdToSO.FindCardSOByID(friend.card.charaID).cardPack;
                    if (pack != crtPack)
                    {
                        Debug.Log("Relic4 check failed!");
                        return;
                    }
                }   
            }

            int crtGold = PlayerManager.Instance.player.GetGold();
            Debug.Log($"Relic4 take effect: gold {crtGold} + {goldNum}");
            PlayerManager.Instance.player.AddGold((int)goldNum);
        }
    }
}