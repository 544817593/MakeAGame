using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
namespace Game
{
    public interface IRoomSystem : ISystem
    {
        void SetRoom(List<RoomEnum> m_Room);
        List<RoomEnum> GetRooms();
        List<RoomEnum> GetRoomsCount();




    }

    public class RoomSystem : AbstractSystem, IRoomSystem
    {
        
        public BindableProperty<List<RoomEnum>> new_rooms = new BindableProperty<List<RoomEnum>>();
        public BindableProperty<List<RoomEnum>> ran_rooms = new BindableProperty<List<RoomEnum>>();
        public BindableProperty<List<RoomEnum>> m_count = new BindableProperty<List<RoomEnum>>();

        int combat_Count, npc_Count, explore_Count, merchant_Count, Rest_Count, Boss_Count;

   
   
        // Start is called before the first frame update
        protected override void OnInit()
        {
          
            new_rooms.SetValueWithoutEvent(new List<RoomEnum>());
            ran_rooms.SetValueWithoutEvent(new List<RoomEnum>());
            m_count.SetValueWithoutEvent(new List<RoomEnum>());
        }

        public void SetRoom(List<RoomEnum> m_Room)
        {
            new_rooms.Value = m_Room;
            new_rooms.Value.Add(RoomEnum.Combat);
            new_rooms.Value.Add(RoomEnum.NPC);
            new_rooms.Value.Add(RoomEnum.Combat);
            new_rooms.Value.Add(RoomEnum.Combat);
            
            
            ran_rooms.Value = RoomSelector.GenerateRooms();
            for (int i = 0; i < ran_rooms.Value.Count; i++)
            {
                new_rooms.Value.Add(ran_rooms.Value[i]);
            }
            // 游戏流程第一章随机房生成后面
            new_rooms.Value.Add(RoomEnum.Merchant);
            new_rooms.Value.Add(RoomEnum.Rest);
            // 这个是BOSS房
            new_rooms.Value.Add(RoomEnum.Combat); 

        }

        public List<RoomEnum> GetRooms()
        {
            return new_rooms;
        }


        public List<RoomEnum> GetRoomsCount()
        {
            return m_count;
        }

        public void AssignR()
        {

            //foreach(var m_RE in GetRoomsCount())
            //{
            //    switch(m_RE.)
            //    {
            //        case RoomEnum.Combat:
            //            combat_Count++;
            //            break;
            //    }
            //}
        }
      
    }
}
