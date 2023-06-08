using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GameManager_CJY : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // 플레이어 생성 (0,0,0) => 위치는 나중에 조정
        PhotonNetwork.Instantiate("Player", Vector3.zero, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
