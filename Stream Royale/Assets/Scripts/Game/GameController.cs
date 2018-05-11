using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

    [Header("Initial Room Setup")]
    public int roomsNumber = 6;
    public int maxPlayers = 20;
    public int minPlayers = 2;
    public float timeoutTimer = 61;
    [Header("UI Manager")]
    public Text[] roomNameText;
    public Text[] maxPlayersText;
    public Text[] actualPlayersText;
    public Text[] chosenPlayer1;
    public Text[] chosenPlayer2;
    public Text[] timeoutText;

    private RoomManager roomManager;
    private List<Player> playerList = new List<Player>();
    private string[] player1Name;       //It represents the name of player one for every room in the game
    private string[] player2Name;       //Same for player number two
    private float initialTimeoutTimer;

    void Start() {
        player1Name = new string[roomsNumber];
        player2Name = new string[roomsNumber];
        initialTimeoutTimer = timeoutTimer;
        InitialRoomsSetup();
	}
	
	void Update () {
        //For loop: used to check if a run can be started, based on the number of players who joined a specific room.
        for (int i = 0; i < roomsNumber; ++i)
        {
            if (roomManager.RoomList[i].StartTimeout)
            {
                timeoutTimer -= Time.deltaTime;
                if (timeoutText[i] != null)
                    timeoutText[i].text = (Math.Floor(timeoutTimer)).ToString();
                if (timeoutTimer <= 0f)
                {
                    roomManager.RoomList[i].IsAvailable = false;
                    roomManager.RoomList[i].StartFlag = true;
                }
                //TODO Check if is possible to move this if statament inside the one below
                if (roomManager.RoomList[i].StartFlag)
                {
                    Debug.Log("sono dentro");
                    roomManager.RoomList[i].StartTimeout = false;
                    timeoutTimer = initialTimeoutTimer;
                    if (timeoutText[i] != null)
                        timeoutText[i].text = "";
                }
            }
            if (roomManager.RoomList[i].StartFlag)
            {
                roomManager.RoomList[i].StartFlag = false;
                StartCoroutine(StartRun(roomManager.RoomList[i]));
            }
        }
        UpdateRoomUI();
	}

    private void UpdateRoomUI()
    {
        for (int i = 0; i < roomsNumber; ++i)
        {
            if (actualPlayersText[i] != null)
                actualPlayersText[i].text = roomManager.RoomList[i].PlayerList.Count.ToString();
            if (chosenPlayer1[i] != null)
                chosenPlayer1[i].text = player1Name[i];
            if (chosenPlayer2[i] != null)
                chosenPlayer2[i].text = player2Name[i];
        }
    }

    public void JoiningPlayer(string username)
    {
        Player player = GetPlayerByName(username);
        if (player == null)
        {
            player = new Player(username);
            playerList.Add(player); //TODO We need to decide when we have to delete player fromt this list
        }

        if (!player.IsPlaying)
        {
            /*TODO in this way we choose the first room available, but it's possible that another room already contains some players 
                (it's better to fill one room at time)*/
            foreach (Room r in roomManager.RoomList)
            {
                //It will create a new player in the first available room
                if (r.IsAvailable)
                {
                    r.AddPlayer(player);
                    Debug.Log(player.Name + " joined Room " + r.RoomID);
                    player.IsPlaying = true;
                    if (r.PlayerList.Count == r.MaxPlayers)
                    {
                        r.IsAvailable = false;
                        r.StartFlag = true;
                    }
                    else if (r.PlayerList.Count >= r.MinPlayers && r.PlayerList.Count < r.MaxPlayers)
                    {
                        //Start a timeout, after 1 min starts the run
                        r.StartTimeout = true;
                    }
                    break;
                }
            }
        } else
        {
            Debug.Log(player.Name + " is already playing in a room");
        }
    }

    private void InitialRoomsSetup()
    {
        //Create rooms
        roomManager = RoomManager.RoomManagerInstance;
        roomManager.CreateRooms(roomsNumber, maxPlayers, minPlayers);
        //Set UI
        for (int i = 0; i < roomsNumber; ++i)
        {
            if (roomNameText[i] != null)
                roomNameText[i].text = "Room " + i;
            if (maxPlayersText[i] != null)
                maxPlayersText[i].text = "/" + maxPlayers.ToString();
        }
    }

    private IEnumerator StartRun(Room r)
    {
        /*  0° phase: retrieve players info (optional for now)
         *  1° phase: initial loot
         *  2° phase: battle + loot     |
         *  3° phase: declare winner    | ------>upgrade of the database with the points of the 1st, 2nd ,3rd players
         *  4° phase: free the room */

        //0° phase

        //1° phase
        

        //2° Phase
        while (r.PlayerList.Count > 1)
        {
            System.Random random = new System.Random();
            Player p1 = r.PlayerList[random.Next(0, r.PlayerList.Count)];
            Player p2;
            do
            {
                p2 = r.PlayerList[random.Next(0, r.PlayerList.Count)];
            } while (p1 == p2);
            //Show name UI
            player1Name[r.RoomID] = p1.Name;
            player2Name[r.RoomID] = p2.Name;
            yield return new WaitForSeconds(3);
            //Fictional Battle (CHANGING AS SOON AS POSSIBLE) HARDCODED
            player2Name[r.RoomID] = "DEAD";
            p2.IsPlaying = false;
            r.PlayerList.Remove(p2);
            yield return new WaitForSeconds(2);
        }

        //3°phase
        //TODO Add a winnerText and fill it with the name of the winner!
        player1Name[r.RoomID] = r.PlayerList[0].Name + " is the winner";
        player2Name[r.RoomID] = "";
        yield return new WaitForSeconds(5);

        //4° phase
        r.ClearRoom();
        player1Name[r.RoomID] = "";
        player2Name[r.RoomID] = "";
    }

    private Player GetPlayerByName(string username)
    {
        for (int i = 0; i < playerList.Count; ++i)
        {
            if (playerList[i].Name == username)
                return playerList[i];
        }
        return null;
    }
}
