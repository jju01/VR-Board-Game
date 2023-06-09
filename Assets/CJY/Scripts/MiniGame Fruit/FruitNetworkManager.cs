using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class FruitNetworkManager : MonoBehaviourPunCallbacks
{
    int time = 0;
    public TextMeshProUGUI timerText;
    public PhotonView pv;

    // ���� ���� ����
    public void Connect()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    // ���� ���� ���� �� ȣ��Ǵ� �ݹ� �Լ�
    public override void OnConnectedToMaster()
    {
        Debug.Log("���� ���� �Ϸ� !");
        Debug.Log($"�κ� ���� = {PhotonNetwork.InLobby}");

        // �κ� ����
        PhotonNetwork.JoinLobby();
    }

    // �κ� ���� �� ȣ��Ǵ� �ݹ� �Լ�
    public override void OnJoinedLobby()
    {
        Debug.Log($"�κ� ���� = {PhotonNetwork.InLobby}");

        // �� ����
        PhotonNetwork.JoinRoom("MiniGame Fruit");

    }

    // �� ���� ���� ���� ��� ȣ��Ǵ� �ݹ� �Լ�
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log($"�� ���� ���� {returnCode}:{message}");

        // �� �ɼ� ����
        RoomOptions ro = new RoomOptions();
        ro.MaxPlayers = 4;      // �ִ� ������ ��
        ro.IsOpen = true;       // �� ���� ����
        ro.IsVisible = true;    // �κ񿡼� �� ��Ͽ� ���� ��ų�� ����    

        // �� ����
        PhotonNetwork.CreateRoom("MiniGame Fruit", ro);
    }

    // �� ���� �Ϸ�� �� ȣ��Ǵ� �ݹ� �Լ�
    public override void OnCreatedRoom()
    {
        Debug.Log("�� ����");
        Debug.Log($"������ �� �̸� = {PhotonNetwork.CurrentRoom.Name}");

    }

    // �� ���� �� ȣ��Ǵ� �ݹ� �Լ�
    public override void OnJoinedRoom()
    {
        Debug.Log($"�� ���� = {PhotonNetwork.InRoom}");
    }

    //// Timer ����
    //void StartTimer()
    //{
    //    if (PhotonNetwork.IsMasterClient)
    //    {
    //        time = 30;

    //        StartCoroutine(TimerCoroution());
    //    }
    //}

    //IEnumerator TimerCoroution()
    //{
    //    if (time > 0)
    //    {
    //        time -= 1;
    //    }
    //    else
    //    {
    //        Debug.Log("Ÿ�̸� ��");
    //        yield break;
    //    }
    //    // 1�� ���� �� ��ο��� ����
    //    pv.RPC("ShowTimer", RpcTarget.All, time);
    //    yield return new WaitForSeconds(1);
    //    StartCoroutine(TimerCoroution());
    //}

    //[PunRPC]
    //void ShowTimer(int number)
    //{
    //    timerText.text = number.ToString();
   // }

    // ���� ���� ���� ��������

}
