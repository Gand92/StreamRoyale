using System.IO;

public class TwitchInfo {

    private string channelName;
    private string botUsername;
    private string botToken;
    private string clientID;

    public TwitchInfo()
    {
        StreamReader sr = new StreamReader("D:/Unity_Projects/InfoTwitch/InfoTwitch.txt");
        for (int i = 0; i < 4; ++i)
        {
            int c = sr.Read();
            while (c != '=')
                c = sr.Read();
            if (i == 0)
                ChannelName = sr.ReadLine();
            if (i == 1)
               BotUsername = sr.ReadLine();
            if (i == 2)
               BotToken = sr.ReadLine();
            if (i == 3)
                ClientID = sr.ReadLine();
        }
        sr.Close();
    }

    public string ChannelName
    {
        get
        {
            return channelName;
        }

        set
        {
            channelName = value;
        }
    }
    public string BotUsername
    {
        get
        {
            return botUsername;
        }

        set
        {
            botUsername = value;
        }
    }
    public string BotToken
    {
        get
        {
            return botToken;
        }

        set
        {
            botToken = value;
        }
    }
    public string ClientID
    {
        get
        {
            return clientID;
        }

        set
        {
            clientID = value;
        }
    }
}
