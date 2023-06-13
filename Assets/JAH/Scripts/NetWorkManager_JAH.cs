using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class NetWorkManager_JAH : MonoBehaviourPunCallbacks
{
    public static NetWorkManager_JAH Instance;
    private Hashtable cp;
    public GameObject player;

    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        Screen.SetResolution(1920, 1080, false);
        PhotonNetwork.ConnectUsingSettings();
        cp = PhotonNetwork.LocalPlayer.CustomProperties;
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinOrCreateRoom("Room", new RoomOptions { MaxPlayers = 4 }, null);
    }


    public override void OnJoinedRoom()
    {
        player = PhotonNetwork.Instantiate("Player", new Vector3(14.6f, 5.95f, -9.88f), Quaternion.identity);

        CameraControllor cc = Camera.main.gameObject.AddComponent<CameraControllor>();
        cc.target = player;

        if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
        {
            PhotonNetwork.LoadLevel("GameScene");
        }

      //  PhotonNetwork.LocalPlayer.SetCustomProperties(new Hashtable { { } })
        //PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable { { } })
    }


    PlayerScript FindPlayer()
    {
        foreach (GameObject Player in GameObject.FindGameObjectsWithTag("Player"))
        {
            if (Player.GetPhotonView().IsMine) return Player.GetComponent<PlayerScript>();
        }
        return null;
    }

    private void Update()
    {
        if (!PhotonNetwork.InRoom) return;
    }

    //public override void OnPlayerEnteredRoom(Player newPlayer)
    //{
    //  if( PhotonNetwork.CurrentRoom.PlayerCount ==1)
    //    {
    //        PhotonNetwork.LoadLevel("GameScene");
    //    }
    //}

    public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
    {
        base.OnRoomPropertiesUpdate(propertiesThatChanged);


    }


}
