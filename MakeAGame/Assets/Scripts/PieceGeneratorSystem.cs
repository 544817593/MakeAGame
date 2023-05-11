using QFramework;
using UnityEngine;

namespace Game
{
    public interface IPieceGeneratorSystem : ISystem
    {
        public Transform pieceRoot { get; }
        
        GameObject CreatePieceFriend();
    }
    
    public class PieceGeneratorSystem: AbstractSystem, IPieceGeneratorSystem
    {
        public Transform pieceRoot { get; private set; }
        private GameObject mPieceFriendPrefab;  // 友方棋子prefab
        
        protected override void OnInit()
        {
            mPieceFriendPrefab = (GameObject) Resources.Load("Prefabs/Piece");
        }

        public GameObject CreatePieceFriend()
        {
            if(pieceRoot == null)
                pieceRoot = GameObject.Find("PieceRoot").transform;
            var pieceGO = GameObject.Instantiate(mPieceFriendPrefab, pieceRoot);
            // var viewPiece = pieceGO.GetComponent<ViewPiece>();
            return pieceGO;
        }
    }
}