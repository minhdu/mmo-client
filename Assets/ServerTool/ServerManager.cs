using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ServerManager : NetSingleton<ServerManager>
{
    public GameObject channelPref;
    List<string> channels = new List<string>();
    bool isBusy = false;

    public void Start()
    {
        ConnectToPhoton();
    }

    public void ConnectToPhoton ()
    {
        if (!PhotonNetwork.connected)
        {
            PhotonNetwork.ConnectUsingSettings(APIDefine.HEADER_VAL_APP_VERSION);
        }
    }

    public void OnConnectedToPhoton()
    {
        PhotonNetwork.JoinLobby();
    }

    public void OnFailedToConnectToPhoton(DisconnectCause cause)
    {
        Logger.LogError("Cause: " + cause);
    }

    public void CreateNewChannel ()
    {
        if (isBusy || !PhotonNetwork.connected)
            return;
        isBusy = true;
        RoomOptions ro = new RoomOptions();
        ro.customRoomPropertiesForLobby = new string[] {"town1"};
        ro.maxPlayers = 100;
        PhotonNetwork.CreateRoom(GenerateNewChannel(), ro, TypedLobby.Default);
    }

    public string GenerateNewChannel()
    {
        return string.Format("Channel {0}", channels.Count);
    }

    public void OnCreatedRoom()
    {
        isBusy = false;
        channels.Add(GenerateNewChannel());
    }

    //Channels
    public void OnJoinedLobby()
    {
        Logger.Log("Joined lobby!");
    }

}
