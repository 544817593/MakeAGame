using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
using System.Linq;

namespace Game
{
    public class SceneFlow : MonoBehaviour, ICanGetSystem
    {
      public static  SceneFlow instance = null;
        public List<RoomEnum> roomList;
        public static string Pre_Room = "Intro";
        public static int combatSceneCount =0 ;
        public static int NpcSceneCount = 0;
        public static int totalRoomCount = 0;
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
            roomSystem.SetRoom(roomList);
            roomList = roomSystem.GetRooms();
        }

        public void LoadRoom()
        {
            RoomEnum m_room = roomList[0];
            roomList.RemoveAt(0);
            UIKit.HideAllPanel();
            UIKit.ClosePanel<DialogueUI.DialoguePanel?>();
           
           


            if (Pre_Room == RoomEnum.Combat.ToString())
            {
                IMapSystem mapSystem = GameEntry.Interface.GetSystem<IMapSystem>();
                mapSystem.SetNUllMap();
                
            }
            if (m_room == RoomEnum.Combat)
            {
               
                combatSceneCount++;
            }
            else if (m_room == RoomEnum.NPC)
            {
                NpcSceneCount++;
            }
            else if(m_room == RoomEnum.Merchant && ShopSystem.enterRoomTime != 0)
            {
                Debug.Log($"enterRoomTime {ShopSystem.enterRoomTime}");
                this.GetSystem<IShopSystem>().resetSystem();
                ShopSystem.enterRoomTime++;
            }
            else if (m_room == RoomEnum.Merchant && ShopSystem.enterRoomTime == 0)
            {
                ShopSystem.enterRoomTime++;
            }

            GameManager.Instance.ResumeGame();
            
            
            EnterRoom(m_room);
            ExitRoom(Pre_Room);

            Pre_Room = m_room.ToString();
        }

        private void EnterRoom(RoomEnum room)
        {
            StartCoroutine(GameManager.Instance.gameSceneMan.LoadScene(room.ToString(), false));
           
        }

        private void ExitRoom(string roomName)
        {
            StartCoroutine(GameManager.Instance.gameSceneMan.UnloadScene(roomName));
        }

        public IArchitecture GetArchitecture()
        {
            return GameEntry.Interface;
        }
    }
}
    
