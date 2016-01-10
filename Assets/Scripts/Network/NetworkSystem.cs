using UnityEngine;
using System.Collections;
using System;

public enum NetworkContext
{
    OFFLINE,
    LOBBY,
    ROOM
}

public class NetworkSystem :  NetSingleton<NetworkSystem>
{
    private NetworkContext currentContext = NetworkContext.OFFLINE;


    public virtual void Start()
    {
        ConnectToServer();
    }

    public void ConnectToServer ()
    {
        if (!PhotonNetwork.connected)
        {
            PhotonNetwork.ConnectUsingSettings(APIDefine.HEADER_VAL_APP_VERSION);
        }
    }

    public virtual void CreateParty ()
    {
        PhotonNetwork.CreateRoom(null, new RoomOptions() { maxPlayers = 6 }, null);
    }

    public virtual void OnFailedToConnectToPhoton(DisconnectCause cause)
    {
        Debug.LogError("Cause: " + cause);
    }

    //Joined channel
    public void OnJoinedRoom()
    {
        Logger.Log("Joined!");
    }

    //Channels
    public void OnJoinedLobby()
    {
        Logger.Log("Joined lobby!");
    }

    //Spawn
    public void SpawnUser()
    {

    }
}
