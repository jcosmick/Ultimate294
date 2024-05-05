using Exiled.API.Enums;
using Exiled.API.Features;
using System.Collections.Generic;
using System.Linq;

namespace SCP294.Utils
{
    public class RoomHandler
    {
        public static Room GetRandomRoom(RoomType _roomType)
        {
            IEnumerable<Room> _roomList = Room.Get((Room room) => room.Type == _roomType);
            return _roomList.ElementAtOrDefault(UnityEngine.Random.Range(0, _roomList.Count()));
        }
        public static Room GetRandomRoom(List<RoomType> _roomType)
        {
            IEnumerable<Room> _roomList = Room.Get((Room room) => _roomType.Contains(room.Type));
            return _roomList.ElementAtOrDefault(UnityEngine.Random.Range(0, _roomList.Count()));
        }
    }
}
