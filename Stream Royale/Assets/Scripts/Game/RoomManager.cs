using System.Collections.Generic;

public class RoomManager {

    private static RoomManager roomManagerInstance;
    private List<Room> roomList = new List<Room>();

    private RoomManager() { }

    public void CreateRooms(int roomsNumber, int maxPlayers, int minPlayers)
    {
        for (int i = 0; i < roomsNumber; ++i)
        {
            Room r = new Room(i, maxPlayers, minPlayers);
            roomList.Add(r);
        }
    }

    public int GetRoomCount()
    {
        return roomList.Count;
    }

    public static RoomManager RoomManagerInstance
    {
        get
        {
            if (roomManagerInstance == null)
            {
                roomManagerInstance = new RoomManager();
            }
            return roomManagerInstance;
        }
    }

    public List<Room> RoomList
    {
        get
        {
            return roomList;
        }

        set
        {
            roomList = value;
        }
    }
}
