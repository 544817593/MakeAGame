using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
using System.Linq;

namespace Game
{
    public class SceneFlow : MonoBehaviour
    {
      public static  SceneFlow instance = null;
        public List<RoomEnum> ttt;
        public string Pre_Room = "Intro";
        public static int combatSceneCount =0 ;
        public static int NpcSceneCount = 0;
        // Start is called before the first frame update
        void Start()
        {
            SetRoomList();

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void SetRoomList()
        {
             IRoomSystem roomSystem = GameEntry.Interface.GetSystem<IRoomSystem>();
            roomSystem.SetRoom(ttt);
            ttt = roomSystem.GetRooms();
        }

        public void LoadRoom()
        {
            RoomEnum m_room = ttt[0];
            ttt.RemoveAt(0);
            UIKit.CloseAllPanel();
            if(Pre_Room == RoomEnum.Combat.ToString())
            {
                IMapSystem mapSystem = GameEntry.Interface.GetSystem<IMapSystem>();
                mapSystem.SetNUllMap();
                
            }
            if(m_room == RoomEnum.Combat)
            {
              
                combatSceneCount++;
            }else if (m_room == RoomEnum.NPC)
            {
                NpcSceneCount++;
            }
           
           
            StartCoroutine(GameManager.Instance.gameSceneMan.LoadScene(m_room.ToString(), false));
            StartCoroutine(GameManager.Instance.gameSceneMan.UnloadScene(Pre_Room));
            
            Pre_Room = m_room.ToString();
            
        }    
    }
}
    
