using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GameManager_CJY : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // �÷��̾� ���� (0,0,0) => ��ġ�� ���߿� ����
        PhotonNetwork.Instantiate("Player", Vector3.zero, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
