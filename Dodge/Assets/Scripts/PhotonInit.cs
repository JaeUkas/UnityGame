using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PhotonInit : MonoBehaviourPunCallbacks
{
    public string gameVersion = "1.0";
    public string nickName = "JaeUk";

    void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    void Start()
    {
        OnLogin();   
    }
    
    void OnLogin()
    {
        PhotonNetwork.GameVersion = this.gameVersion;
        PhotonNetwork.NickName = this.nickName;
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected!!");   
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("Failed join room!!");
        this.CreateRoom();
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined room!!");
        StartCoroutine(this.CreatePlayer());
    }

    IEnumerator CreatePlayer()
    {
        PhotonNetwork.Instantiate("Player", new Vector3(0, 0, -30), Quaternion.identity, 0);

        yield return null;
    }

    void CreateRoom()
    {
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 4 });
    }
 
}
