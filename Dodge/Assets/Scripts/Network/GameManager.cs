using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class GameManager : MonoBehaviourPunCallbacks
{
    public GameObject camaraPrefab;
    public GameObject camaraPrefab1;
    public GameObject playerPrefab;
    public GameObject playerPrefab2;
    public GameObject enemyPrefab;
    public static GameManager Instance;

    public GameObject pClone;
    public GameObject pClone1;

    public GameObject eClone;
    public GameObject eClone1;

    public int enemyDie;
    public int playerDie;

    private bool start;

    private void Start()
    {
        enemyDie = 0;
        playerDie = 0;
        Instance = this;

        if (playerPrefab == null)
        {
            Debug.LogError("<Color=Red><a>Missing</a></Color> playerPrefab Reference. Please set it up in GameObject 'Game Manager'", this);
        }
        else
        {
            Debug.LogFormat("We are Instantiating LocalPlayer from {0}", Application.loadedLevelName);
         
            if (PhotonNetwork.CurrentRoom.PlayerCount % 2 == 1)
            {
                // we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate
                Instantiate(camaraPrefab, camaraPrefab.transform.position, camaraPrefab.transform.rotation);
                pClone = PhotonNetwork.Instantiate(this.playerPrefab.name, new Vector3(0f, 0f, -270f), Quaternion.identity, 0);
                eClone = PhotonNetwork.Instantiate(this.enemyPrefab.name, new Vector3(0f, 1f, -100f), Quaternion.Euler(0, 180, 0), 0);
            }
            else if (PhotonNetwork.CurrentRoom.PlayerCount % 2 == 0)
            {
                Instantiate(camaraPrefab1, camaraPrefab1.transform.position, camaraPrefab1.transform.rotation);
                pClone1 = PhotonNetwork.Instantiate(this.playerPrefab2.name, new Vector3(0f, 0f, 250f), Quaternion.Euler(0,180,0), 0);
                eClone1 = PhotonNetwork.Instantiate(this.enemyPrefab.name, new Vector3(0f, 1f, 90f), Quaternion.identity, 0);
            }
        }
    }

    private void Update()
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount >= 2) start = true;
        if (enemyDie >= 1)
        {
            OnLeftRoom();
            LeaveRoom();
        }
        if (playerDie >= 1)
        {
            OnLeftRoom();
            LeaveRoom();
        }
        if (start == true && PhotonNetwork.CurrentRoom.PlayerCount == 1)
        {
            OnLeftRoom();
            LeaveRoom();
        }
    }

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene(0);
       
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    void LoadArena()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            Debug.LogError("PhotonNetwork : Trying to Load a level but we are not the master Client");
        }
        Debug.LogFormat("PhotonNetwork : Loading Level : {0}", PhotonNetwork.CurrentRoom.PlayerCount);
    }

    public override void OnPlayerEnteredRoom(Player other)
    {
        Debug.LogFormat("OnPlayerEnteredRoom() {0}", other.NickName); // not seen if you're the player connecting

        if (PhotonNetwork.IsMasterClient)
        {
            Debug.LogFormat("OnPlayerEnteredRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom

            LoadArena();
        }
    }


    public override void OnPlayerLeftRoom(Player other)
    {
        Debug.LogFormat("OnPlayerLeftRoom() {0}", other.NickName); // seen when other disconnects

        if (PhotonNetwork.IsMasterClient)
        {
            Debug.LogFormat("OnPlayerLeftRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom

            LoadArena();
        }
    }
}