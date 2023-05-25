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

      


    }

    public class RoomSystem : AbstractSystem, IRoomSystem
    {
        
        public BindableProperty<List<RoomEnum>> new_rooms = new BindableProperty<List<RoomEnum>>();
        public BindableProperty<List<RoomEnum>> ran_rooms = new BindableProperty<List<RoomEnum>>();
        // Start is called before the first frame update
        protected override void OnInit()
        {
          
            new_rooms.SetValueWithoutEvent(new List<RoomEnum>());
            ran_rooms.SetValueWithoutEvent(new List<RoomEnum>());
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
        }

        public List<RoomEnum> GetRooms()
        {
            return new_rooms;
        }

      
    }
}
