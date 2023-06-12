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

    // 포톤 서버 접속
    public void Conect()
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
        PhotonNetwork.JoinRoom("MiniGame Ice");

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
        PhotonNetwork.CreateRoom("MiniGame Ice", ro);
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

    }

    // 게임 종료되는 정보 가져오기
    public override void OnPlayerPropertiesUpdate(Photon.Realtime.Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        base.OnPlayerPropertiesUpdate(targetPlayer, changedProps);

        if (changedProps.ContainsKey($"MiniGameIce"))
        {
            MiniGameIce.Instance.OnGameEnd();
        }
    }
}
