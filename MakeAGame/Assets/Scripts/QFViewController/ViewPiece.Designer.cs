using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public partial class ViewPiece
    {
        public SpriteRenderer spPiece;

        void InitBind()
        {
            spPiece = transform.Find("Root/SpritePiece").GetComponent<SpriteRenderer>();
        }
    }
}