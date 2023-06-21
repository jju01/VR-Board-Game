using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    // �̱������� ����
    public static NetworkManager Instance;

    public GameObject myPlayer;
    public TMP_InputField NickNameInput;
    public PhotonView playerPrefab;

    [Space]
    public GameObject startPanel;
    public GameObject chrPanel;
    public GameObject noColorPanel;

    private List<Transform> positionList = new List<Transform>();

    void Awake()
    {
        Instance = this;
        Screen.SetResolution(1920, 1080, false);
        chrPanel.SetActive(false);
    }

    // ���� ���� ����
    public void Connect()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.ConnectUsingSettings();
    }

    // ���� ���� ���� �� ȣ��Ǵ� �ݹ� �Լ�
    public override void OnConnectedToMaster()
    {
        Debug.Log("���� ���� �Ϸ� !");
        PhotonNetwork.LocalPlayer.NickName = NickNameInput.text;
        Debug.Log($"�κ� ���� = {PhotonNetwork.InLobby}");

        // �κ� ����
        PhotonNetwork.JoinLobby();
    }

    // �κ� ���� �� ȣ��Ǵ� �ݹ� �Լ�
    public override void OnJoinedLobby()
    {
        Debug.Log($"�κ� ���� = {PhotonNetwork.InLobby}");

        // �� ����
        PhotonNetwork.JoinRoom("VR Board Game");

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
        PhotonNetwork.CreateRoom("VR Board Game", ro);
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
        // start �г� ��Ȱ��ȭ
        startPanel.SetActive(false);
        // ĳ���� �г� Ȱ��ȭ
        chrPanel.SetActive(true);

        // �뿡 ������ ����� ���� Ȯ��
        foreach (var player in PhotonNetwork.CurrentRoom.Players)
        {
            Debug.Log($"{player.Value.NickName},{player.Value.ActorNumber}");
        }

        // ������ ���� ������ �迭�� ����
        Transform[] points = GameObject.Find("Player Spawn Point").GetComponentsInChildren<Transform>();
        foreach (Transform pos in points)
        {
            positionList.Add(pos);
        }


        Hashtable CustomProperties = PhotonNetwork.CurrentRoom.CustomProperties;
        int idx = Random.Range(1, 5);
        while (CustomProperties.ContainsKey($"pos_{idx}"))
        {
            idx = Random.Range(1, 5);
        }

        // ����
        // PhotonNetwork.CurrentRoom.CustomProperties

        //  ����
        CustomProperties.Add($"pos_{idx}", "Selected");

        PhotonNetwork.CurrentRoom.SetCustomProperties(CustomProperties);

        // ������ ����
        myPlayer = PhotonNetwork.Instantiate(playerPrefab.name, points[idx].position, points[idx].rotation);


        //PhotonNetwork.CurrentRoom.CustomProperties
        //PhotonNetwork.Instantiate(playerPrefab.name, new Vector3(0,5,0), Quaternion.identity);       
    }

    // Player�� ���� �����ͼ� ������Ʈ
    public override void OnPlayerPropertiesUpdate(Photon.Realtime.Player targetPlayer, Hashtable changedProps)
    {
        base.OnPlayerPropertiesUpdate(targetPlayer, changedProps);

        if (!PhotonNetwork.IsMasterClient)
        { return; }

        if (changedProps.ContainsKey($"PlayerReady"))
        {
            int readyCount = 0;
            foreach (Photon.Realtime.Player p in PhotonNetwork.CurrentRoom.Players.Values)
            {
                if (p.CustomProperties.ContainsKey("PlayerReady"))
                    readyCount++;
                // Player.Instance.ReadyChecktrue();
                print("�ö�");
            }

            if (readyCount == 4)
            {
                Debug.Log("���� �� ����");
                PhotonNetwork.LoadLevel("Main");
            }
        }

    }


    [ContextMenu("����")]
    void Info()
    {
        if (PhotonNetwork.InRoom)
        {
            Debug.Log("���� �ο� �� :" + PhotonNetwork.CountOfPlayers);
            Debug.Log("���� �ִ� �ο� �� :" + PhotonNetwork.CurrentRoom.MaxPlayers);

            string playerStr = "�뿡 �ִ� �÷��̾� ��� : ";
            for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
            {
                playerStr += PhotonNetwork.PlayerList[i].NickName + ",";
            }
            Debug.Log(playerStr);
        }
    }

    public void NOColorSelected()
    {
        StartCoroutine(NoColorSelect());
    }

    // �̼��� UI �ߴ� �Լ�
    IEnumerator NoColorSelect()
    {
        noColorPanel.SetActive(true);
        yield return new WaitForSeconds(1f);
        noColorPanel.SetActive(false);
    }
}
