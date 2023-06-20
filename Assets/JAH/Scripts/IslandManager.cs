using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class IslandManager : MonoBehaviourPunCallbacks
{
    public string Minigame1Scene;
    public string Minigame2Scene;
    public string EndingScene;
    
    bool IsMyTurn;
    int myOrder;
    int currentTurn;
    
    private void Start()
    {
        myOrder = (int)PhotonNetwork.LocalPlayer.CustomProperties["PlayerOrder"];
        currentTurn = (int)PhotonNetwork.CurrentRoom.CustomProperties["PlayerTurn"];
    }

    public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
    {
        base.OnRoomPropertiesUpdate(propertiesThatChanged);
        
        if (propertiesThatChanged.ContainsKey("PlayerTurn"))
        {
            currentTurn = (int)propertiesThatChanged["PlayerTurn"];
            IsMyTurn = currentTurn == myOrder;

            int bTurn = 0;
            if (propertiesThatChanged.ContainsKey("BoardGameTurn"))
                bTurn = (int)propertiesThatChanged["BoardGameTurn"];
            
            if(bTurn == 2)
                SceneLoad(Minigame1Scene);
            else if(bTurn == 4)
                SceneLoad(Minigame2Scene);
            else
            {
                if(IsMyTurn == false)
                    return;
            
                if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("island"))
                {
                    Hashtable hs = new Hashtable();
                    hs.Add("island", null);
                    PhotonNetwork.LocalPlayer.SetCustomProperties(hs);
                
                    NextTurn();
                }
                else
                {
                    PhotonNetwork.LeaveRoom();
                }
            }
        }
        
        if (propertiesThatChanged.ContainsKey("GameWinner"))
        {
            SceneLoad(EndingScene);
        }
    }
    
    public void NextTurn()
    {
        
        Room room = PhotonNetwork.CurrentRoom;
        int nextTurn = (currentTurn + 1) % room.PlayerCount;
        
        Hashtable hs = new Hashtable();
        hs.Add("PlayerTurn", nextTurn);

        int bTurn = 0;

        if (room.CustomProperties.ContainsKey("BoardGameTurn"))
            bTurn = (int)room.CustomProperties["BoardGameTurn"];

        bTurn++;
        
        if(nextTurn == 0)
            hs.Add("BoardGameTurn", bTurn);
        
        PhotonNetwork.CurrentRoom.SetCustomProperties(hs);
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        
        PhotonNetwork.JoinRoom("VR Board Game");
    }
    
    private void SceneLoad(string sceneName)
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        
        if(PhotonNetwork.IsMasterClient == false)
            return;
        
        PhotonNetwork.LoadLevel(sceneName);
    }
}
