using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class IceNetworkManager : MonoBehaviourPunCallbacks
{
    private void Start()
    {
        Conect();
    }

    // ���� ���� ����
    public void Conect()
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
        PhotonNetwork.JoinRoom("MiniGame Ice");

    }

    // �� ���� ���� ���� ��� ȣ��Ǵ� �ݹ� �Լ�
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log($"�� ���� ���� {returnCode}:{message}");

        // �� �ɼ� ����
        RoomOptions ro = new RoomOptions();
        ro.MaxPlayers = 2;      // �ִ� ������ ��
        ro.IsOpen = true;       // �� ���� ����
        ro.IsVisible = true;    // �κ񿡼� �� ��Ͽ� ���� ��ų�� ����    

        // �� ����
        PhotonNetwork.CreateRoom("MiniGame Ice", ro);
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

        if (PhotonNetwork.CurrentRoom.MaxPlayers == PhotonNetwork.CurrentRoom.PlayerCount)
        {
            Debug.Log("��� ���� �Ϸ�");

            Invoke("StartGame", 3f);
        }
    }

    // �濡 ���� ��� üũ
    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        if (PhotonNetwork.CurrentRoom.MaxPlayers == PhotonNetwork.CurrentRoom.PlayerCount)
        {
            Debug.Log("��� ���� �Ϸ�");

            Invoke("StartGame", 3f);
        }
    }

    // 3�� �� ���ӽ���
    private void StartGame()
    {
        MiniGameIce.Instance.isstart = true;
        if (MiniGameIce.Instance.isstart == true)
        {
            MiniGameIce.Instance.game.SetActive(true);
        }
    }

    // Ice MiniGame ����Ǵ� ���� ��������
    public override void OnPlayerPropertiesUpdate(Photon.Realtime.Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        base.OnPlayerPropertiesUpdate(targetPlayer, changedProps);

        if (changedProps.ContainsKey($"MiniGameIce"))
        {
            MiniGameIce.Instance.OnGameEnd();
        }
    }
}