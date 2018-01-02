using System.Collections.Generic;

public class Room {

    private int roomID;
    private int maxPlayers;
    private int minPlayers;
    private List<Player> playerList = new List<Player>();
    private bool isAvailable; //a room is not available if a run is already started for that room
    private bool startFlag; //used in the update function of gameController to start the coroutine
    private bool startTimeout; //used in the update function of gameController to start the timeout of the room before the start of the run


    public Room(int roomID, int maxPlayers, int minPlayers)
    {
        this.roomID = roomID;
        this.maxPlayers = maxPlayers;
        this.minPlayers = minPlayers;
        isAvailable = true;
        startFlag = false;
        startTimeout = false;
    }


    //Add a new player to the room if the maximum number of player is not reached
    public void AddPlayer(Player player)
    {
        playerList.Add(player);
    }
    public void ClearRoom()
    {
        //Actually this function will be called with only the winner in the player list, but nevermind. It could be usefull in other ways
        foreach (Player p in playerList)
        {
            p.IsPlaying = false;
        }
        playerList.Clear();
        IsAvailable = true;
    }

    public int RoomID
    {
        get
        {
            return roomID;
        }

        set
        {
            roomID = value;
        }
    }
    public List<Player> PlayerList
    {
        get
        {
            return playerList;
        }

        set
        {
            playerList = value;
        }
    }
    public bool IsAvailable
    {
        get
        {
            return isAvailable;
        }

        set
        {
            isAvailable = value;
        }
    }
    public bool StartFlag
    {
        get
        {
            return startFlag;
        }

        set
        {
            startFlag = value;
        }
    }
    public bool StartTimeout
    {
        get
        {
            return startTimeout;
        }

        set
        {
            startTimeout = value;
        }
    }
    public int MaxPlayers
    {
        get
        {
            return maxPlayers;
        }

        set
        {
            maxPlayers = value;
        }
    }
    public int MinPlayers
    {
        get
        {
            return minPlayers;
        }

        set
        {
            minPlayers = value;
        }
    }

}
