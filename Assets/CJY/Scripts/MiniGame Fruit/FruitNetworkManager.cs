using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class FruitNetworkManager : MonoBehaviourPunCallbacks
{
    public PhotonView pv;

    public static FruitNetworkManager Instance;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        Connect();
    }

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
        ro.MaxPlayers = 2;      // �ִ� ������ ��
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
        MiniGameManager.Instance.menuPanel.SetActive(true);
        if (PhotonNetwork.CurrentRoom.MaxPlayers == PhotonNetwork.CurrentRoom.PlayerCount)
        {
            Debug.Log("��� ���� �Ϸ�");

            Invoke("StartTimer", 3f);
        }
    }

    // �濡 ���� ��� üũ 
    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        MiniGameManager.Instance.menuPanel.SetActive(true);
        if (PhotonNetwork.CurrentRoom.MaxPlayers == PhotonNetwork.CurrentRoom.PlayerCount)
        {
            Debug.Log("��� ���� �Ϸ�");

            Invoke("StartTimer", 3f);
        }
    }

    private void StartTimer()
    {
        MiniGameManager.Instance.menuPanel.SetActive(false);
        MiniGameManager.Instance.isready = true;
        FruiteSpawner.Instance.StartFruit();
        MiniGameManager.Instance.StartTimer();
    }

    ////Timer ����
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
    //        time -= Time.deltaTime;
    //    }
    //    else if (time <= 0)
    //    {
    //        Debug.Log("���� ��");
    //        yield break;
    //    }

    ////    1�� ���� �� ��ο��� ����
    //    pv.RPC("ShowTimer", RpcTarget.All, time);
    //    yield return new WaitForSeconds(1);
    //    StartCoroutine(TimerCoroution());
    //}

    //[PunRPC]
    //void ShowTimer(int number)
    //{
    //    timerText.text = number.ToString();
    //}


    // ���� ���� ���� ��������
    public override void OnPlayerPropertiesUpdate(Photon.Realtime.Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        base.OnPlayerPropertiesUpdate(targetPlayer, changedProps);


        // ���ӳ����� ���� Ȯ�� �� ���� ���� 
        if (changedProps.ContainsKey($"MiniGameFruit"))
        {
            MiniGameManager.Instance.OnGameEnd();
        }

        // �������� ������ �ִ��� �� Ȯ�� �� ���� �����ϸ� UI â ������
        if (changedProps.ContainsKey($"MiniGameFruitScore"))
        {
            int scoreCount = 0;
            foreach (Photon.Realtime.Player p in PhotonNetwork.CurrentRoom.Players.Values)
            {
                if (p.CustomProperties.ContainsKey("MiniGameFruitScore"))
                    scoreCount++;
                print("�ö�");
            }

            if (scoreCount == 2)
            {
                Debug.Log("UI ");
                // ���� ����

            }
        }
    }
}
