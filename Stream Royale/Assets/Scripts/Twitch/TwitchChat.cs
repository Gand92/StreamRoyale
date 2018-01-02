using System;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using TwitchLib;
using TwitchLib.Events.Client;
using TwitchLib.Models.Client;

public class TwitchChat : MonoBehaviour {

    private TwitchClient client;
    private TwitchInfo twitchInfo;
    private GameController gameController;

	private void Start () {

        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        ServicePointManager.ServerCertificateValidationCallback = CertificateValidationMonoFix;
        twitchInfo = new TwitchInfo();
        TwitchAPI twitchAPI = new TwitchAPI();
        twitchAPI.Settings.ClientId = twitchInfo.ClientID;
        ConnectionCredentials credentials = new ConnectionCredentials(twitchInfo.BotUsername, twitchInfo.BotToken);
        client = new TwitchClient(credentials, twitchInfo.ChannelName);
        EventHandlerSetup(client);
        client.Connect();
        
	}

    private void EventHandlerSetup(TwitchClient client)
    {
        client.OnJoinedChannel += Client_OnJoinedChannel;
        client.OnMessageReceived += Client_OnMessageReceived;
    }

    private void Client_OnJoinedChannel(object sender, OnJoinedChannelArgs e)
    {
        client.SendMessage(twitchInfo.BotUsername + " joined the channel!");
    }

    private void Client_OnMessageReceived(object sender, OnMessageReceivedArgs e)
    {
        //This is the right way to add a player to a new room but for testing purpose we need to use fictional player
        //if (e.ChatMessage.Message == "!join")
        //{
        //    gameController.PlayerJoin(e.ChatMessage.Username)
        //}
        //This is the fictional way
        if (e.ChatMessage.Message.StartsWith("!join ", StringComparison.InvariantCultureIgnoreCase))
        {
            string username = e.ChatMessage.Message.Substring(6); //Used to pick the username after join: !join Gand92
            Debug.Log(username + " wants to join a room");
            gameController.JoiningPlayer(username);
        }
    }

    public bool CertificateValidationMonoFix(System.Object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
    {
        bool isOk = true;

        if (sslPolicyErrors == SslPolicyErrors.None)
        {
            return true;
        }

        foreach (X509ChainStatus status in chain.ChainStatus)
        {
            if (status.Status == X509ChainStatusFlags.RevocationStatusUnknown)
            {
                continue;
            }

            chain.ChainPolicy.RevocationFlag = X509RevocationFlag.EntireChain;
            chain.ChainPolicy.RevocationMode = X509RevocationMode.Online;
            chain.ChainPolicy.UrlRetrievalTimeout = new TimeSpan(0, 1, 0);
            chain.ChainPolicy.VerificationFlags = X509VerificationFlags.AllFlags;

            bool chainIsValid = chain.Build((X509Certificate2)certificate);

            if (!chainIsValid)
            {
                isOk = false;
            }
        }

        return isOk;
    }
}
