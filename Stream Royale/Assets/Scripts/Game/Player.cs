
using System;

public class Player
{
    private string name;
    private bool isPlaying;


    public Player(string name)
    {
        this.name = name;
        this.isPlaying = false;
    }

    public bool IsPlaying
    {
        get
        {
            return isPlaying;
        }

        set
        {
            isPlaying = value;
        }
    }
    public string Name
    {
        get
        {
            return name;
        }

        set
        {
            name = value;
        }
    }

}