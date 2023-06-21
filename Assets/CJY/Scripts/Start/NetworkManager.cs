using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    // 싱글톤으로 관리
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

    // 포톤 서버 접속
    public void Connect()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.ConnectUsingSettings();
    }

    // 포톤 서버 접속 후 호출되는 콜백 함수
    public override void OnConnectedToMaster()
    {
        Debug.Log("서버 접속 완료 !");
        PhotonNetwork.LocalPlayer.NickName = NickNameInput.text;
        Debug.Log($"로비 입장 = {PhotonNetwork.InLobby}");

        // 로비 입장
        PhotonNetwork.JoinLobby();
    }

    // 로비 접속 후 호출되는 콜백 함수
    public override void OnJoinedLobby()
    {
        Debug.Log($"로비 입장 = {PhotonNetwork.InLobby}");

        // 룸 입장
        PhotonNetwork.JoinRoom("VR Board Game");

    }

    // 룸 입장 실패 했을 경우 호출되는 콜백 함수
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log($"룸 입장 실패 {returnCode}:{message}");

        // 룸 옵션 정의
        RoomOptions ro = new RoomOptions();
        ro.MaxPlayers = 4;      // 최대 접속자 수
        ro.IsOpen = true;       // 룸 오픈 여부
        ro.IsVisible = true;    // 로비에서 룸 목록에 노출 시킬지 여부    

        // 룸 생성
        PhotonNetwork.CreateRoom("VR Board Game", ro);
    }

    // 룸 생성 완료된 후 호출되는 콜백 함수
    public override void OnCreatedRoom()
    {
        Debug.Log("룸 생성");
        Debug.Log($"생성된 룸 이름 = {PhotonNetwork.CurrentRoom.Name}");

    }

    // 룸 입장 후 호출되는 콜백 함수
    public override void OnJoinedRoom()
    {
        Debug.Log($"룸 입장 = {PhotonNetwork.InRoom}");
        // start 패널 비활성화
        startPanel.SetActive(false);
        // 캐릭터 패널 활성화
        chrPanel.SetActive(true);

        // 룸에 접속한 사용자 정보 확인
        foreach (var player in PhotonNetwork.CurrentRoom.Players)
        {
            Debug.Log($"{player.Value.NickName},{player.Value.ActorNumber}");
        }

        // 접속자 스폰 정보를 배열에 저장
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

        // 접근
        // PhotonNetwork.CurrentRoom.CustomProperties

        //  세팅
        CustomProperties.Add($"pos_{idx}", "Selected");

        PhotonNetwork.CurrentRoom.SetCustomProperties(CustomProperties);

        // 접속자 생성
        myPlayer = PhotonNetwork.Instantiate(playerPrefab.name, points[idx].position, points[idx].rotation);


        //PhotonNetwork.CurrentRoom.CustomProperties
        //PhotonNetwork.Instantiate(playerPrefab.name, new Vector3(0,5,0), Quaternion.identity);       
    }

    // Player들 정보 가져와서 업데이트
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
                print("올라감");
            }

            if (readyCount == 4)
            {
                Debug.Log("게임 씬 가자");
                PhotonNetwork.LoadLevel("Main");
            }
        }

    }


    [ContextMenu("정보")]
    void Info()
    {
        if (PhotonNetwork.InRoom)
        {
            Debug.Log("접속 인원 수 :" + PhotonNetwork.CountOfPlayers);
            Debug.Log("접속 최대 인원 수 :" + PhotonNetwork.CurrentRoom.MaxPlayers);

            string playerStr = "룸에 있는 플레이어 목록 : ";
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

    // 이선좌 UI 뜨는 함수
    IEnumerator NoColorSelect()
    {
        noColorPanel.SetActive(true);
        yield return new WaitForSeconds(1f);
        noColorPanel.SetActive(false);
    }
}
