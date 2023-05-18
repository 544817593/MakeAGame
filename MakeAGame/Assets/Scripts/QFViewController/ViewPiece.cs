using System;
using System.Collections.Generic;
using DG.Tweening;
using QFramework;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Game
{
    public partial class ViewPiece: ViewPieceBase
    {
        public Card card;
        
        public void SetDataWithCard(Card _card)
        {
            card = _card;
        }

        private void Start()
        {
            InitBind();
            InitView();
            
            Invoke(nameof(MoveTest), 3f);
        }

        void InitView()
        {
            spPiece.sprite = card.pieceSprite;
        }

        void MoveTest()
        {
            BoxGrid grid = pieceGrids[0];
            var mapSystem = this.GetSystem<IMapSystem>();
            int mapCol = mapSystem.mapCol;
            BoxGrid nextRightGrid = mapSystem.Grids()[grid.row, Mathf.Clamp(grid.col + 1, 0, mapCol - 1)];

            Vector3 moveVec = nextRightGrid.transform.position - grid.transform.position;
            transform.DOMove(transform.position + moveVec, 0.3f);
            
            Invoke(nameof(MoveTest), 3f);
        }
    }
}