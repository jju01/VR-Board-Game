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

    // 포톤 서버 접속
    public void Connect()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    // 포톤 서버 접속 후 호출되는 콜백 함수
    public override void OnConnectedToMaster()
    {
        Debug.Log("서버 접속 완료 !");
        Debug.Log($"로비 입장 = {PhotonNetwork.InLobby}");

        // 로비 입장
        PhotonNetwork.JoinLobby();
    }

    // 로비 접속 후 호출되는 콜백 함수
    public override void OnJoinedLobby()
    {
        Debug.Log($"로비 입장 = {PhotonNetwork.InLobby}");

        // 룸 입장
        PhotonNetwork.JoinRoom("MiniGame Fruit");

    }

    // 룸 입장 실패 했을 경우 호출되는 콜백 함수
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log($"룸 입장 실패 {returnCode}:{message}");

        // 룸 옵션 정의
        RoomOptions ro = new RoomOptions();
        ro.MaxPlayers = 2;      // 최대 접속자 수
        ro.IsOpen = true;       // 룸 오픈 여부
        ro.IsVisible = true;    // 로비에서 룸 목록에 노출 시킬지 여부    

        // 룸 생성
        PhotonNetwork.CreateRoom("MiniGame Fruit", ro);
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
        MiniGameManager.Instance.menuPanel.SetActive(true);
        if (PhotonNetwork.CurrentRoom.MaxPlayers == PhotonNetwork.CurrentRoom.PlayerCount)
        {
            Debug.Log("모두 참가 완료");

            Invoke("StartTimer", 3f);
        }
    }

    // 방에 들어온 사람 체크 
    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        MiniGameManager.Instance.menuPanel.SetActive(true);
        if (PhotonNetwork.CurrentRoom.MaxPlayers == PhotonNetwork.CurrentRoom.PlayerCount)
        {
            Debug.Log("모두 참가 완료");

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

    ////Timer 시작
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
    //        Debug.Log("게임 끝");
    //        yield break;
    //    }

    ////    1초 마다 방 모두에게 전달
    //    pv.RPC("ShowTimer", RpcTarget.All, time);
    //    yield return new WaitForSeconds(1);
    //    StartCoroutine(TimerCoroution());
    //}

    //[PunRPC]
    //void ShowTimer(int number)
    //{
    //    timerText.text = number.ToString();
    //}


    // 과일 게임 정보 가져오기
    public override void OnPlayerPropertiesUpdate(Photon.Realtime.Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        base.OnPlayerPropertiesUpdate(targetPlayer, changedProps);


        // 게임끝나는 정보 확인 후 게임 종료 
        if (changedProps.ContainsKey($"MiniGameFruit"))
        {
            MiniGameManager.Instance.OnGameEnd();
        }

        // 점수정보 가지고 있는지 다 확인 후 조건 충족하면 UI 창 나오기
        if (changedProps.ContainsKey($"MiniGameFruitScore"))
        {
            int scoreCount = 0;
            foreach (Photon.Realtime.Player p in PhotonNetwork.CurrentRoom.Players.Values)
            {
                if (p.CustomProperties.ContainsKey("MiniGameFruitScore"))
                    scoreCount++;
                print("올라감");
            }

            if (scoreCount == 2)
            {
                Debug.Log("UI ");
                // 점수 띄우기

            }
        }
    }
}
