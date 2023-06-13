using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Unity.VisualScripting;

public class PlayerScript : MonoBehaviourPunCallbacks
{
    NetWorkManager_JAH NM;
    PhotonView PV;


    void Start()
    {
        PV = photonView;
        NM = GameObject.FindGameObjectWithTag("NetWorkManager").GetComponent<NetWorkManager_JAH>();
    }

  

    void Update()
    {
        if (!PV.IsMine) return;

      
    }
}
