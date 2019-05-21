using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Launcher : MonoBehaviourPunCallbacks
{
    string gameVersion = "1";
    public string userId = "test01";

    [SerializeField]
    private byte maxPlayersPerRoom = 4;
    [SerializeField]
    private GameObject controlPanel;
    [SerializeField]
    private GameObject progressLabel;

    bool isConnecting;

    void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }


    void Start()
    {
        progressLabel.SetActive(false);
        controlPanel.SetActive(true);
    }


    public void Connect()
    {
        isConnecting = true;

        progressLabel.SetActive(true);
        controlPanel.SetActive(false);

        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            PhotonNetwork.GameVersion = this.gameVersion;
            PhotonNetwork.NickName = userId;

            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public override void OnConnectedToMaster()
    {
        if (isConnecting)
        {
            Debug.Log("Connect To Master");
            PhotonNetwork.JoinRandomRoom();
        }
    }


    public override void OnDisconnected(DisconnectCause cause)
    {
        progressLabel.SetActive(false);
        controlPanel.SetActive(true);

        Debug.LogWarningFormat("OnDisconnected() was called by PUN with reason {0}", cause);
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("Failed Join Room!!!");

        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = maxPlayersPerRoom });
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined Room!!!");
        PhotonNetwork.LoadLevel("PlayScene");
    }
}