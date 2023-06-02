using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


public class Ready : MonoBehaviour
{
    private PhotonView pv;
    private bool playerReady = false;
    public int readyCount = 0;
    private ExitGames.Client.Photon.Hashtable PlayerCustomProperties = new ExitGames.Client.Photon.Hashtable();

    private string playerReadyKey = "PlayerReady";

    // Update is called once per frame
    void Update()
    {

    }

    public void SetReady()
    {
        playerReady = true;
        if (PlayerCustomProperties.ContainsKey(playerReadyKey))
        {
            PlayerCustomProperties[playerReadyKey] = playerReady;
        }
        else
        {
            PlayerCustomProperties.Add(playerReadyKey, playerReady);
        }

        PhotonNetwork.SetPlayerCustomProperties(PlayerCustomProperties);
    }

    public void OnClickReady()
    {
        SetReady();
    }
}
