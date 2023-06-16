using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviourPunCallbacks
{
    // VR Controller 사용 여부
    public bool useVRController = false;
    
    // 싱글톤 준비
    public static GameManager Instance;



    public GameObject guideUI;
    
    public GameObject cameraObj;
    public GameObject MyPlayer;
    public Dice MyDice;

    public int currentTurn;
    public int myOrder;

    public bool IsMyTurn = false;


    private void Awake()
    {
        // 만일, 나 자신(=this)이 비어있는 상태라면
        if (Instance == null)
        {
            // Instance에 나 자신을 할당한다.
            Instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        AutoControllerSetting();
        CheckConnect();
    }

    // 자동 VR Controller 사용여부 체크
    private void AutoControllerSetting()
    {
        if (GameObject.Find("Main Camera"))
        {
            cameraObj = GameObject.Find("Main Camera");
            useVRController = false;
        }
        if (GameObject.Find("OVRCameraRig"))
        {
            cameraObj = GameObject.Find("OVRCameraRig");
            useVRController = true;
        }
    }

    // 3인칭 카메라 시점으로 바뀐다
    public void SetThirdPersonView(Transform thirdcam)
    {
        cameraObj.transform.SetParent(thirdcam);
        cameraObj.transform.localPosition = Vector3.zero;
        cameraObj.transform.localEulerAngles = Vector3.zero;
    }
    // 1인칭 Ovrcamera 시점으로 바뀐다
    public void SetOVRCameraView()
    {
        MyPlayer.GetComponent<BoardGamePlayer>().SetCamera(cameraObj.transform);
    }

    private void CheckConnect()
    {
        if (PhotonNetwork.IsConnected)
            BoardGameReady();
        else
            PhotonNetwork.ConnectUsingSettings();
    }
    

    #region Pun Callback

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        PhotonNetwork.JoinOrCreateRoom("VR Board Game", new RoomOptions(), TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        BoardGameReady();
    }

    public override void OnPlayerPropertiesUpdate(Photon.Realtime.Player targetPlayer, Hashtable changedProps)
    {
        base.OnPlayerPropertiesUpdate(targetPlayer, changedProps);
        
        if (changedProps.ContainsKey("PlayerReady"))
        {
            GameStart();
        }
    }

    public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
    {
        base.OnRoomPropertiesUpdate(propertiesThatChanged);
        
        if (propertiesThatChanged.ContainsKey("PlayerTurn"))
        {
            currentTurn = (int)propertiesThatChanged["PlayerTurn"];
            IsMyTurn = currentTurn == myOrder;
            
            TurnStart();
        }
    }

    #endregion


    #region BoardGame Logic

    private void BoardGameReady()
    {
        int iceIdx = 0;

        if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("IceCubeIdx"))
            iceIdx = (int)PhotonNetwork.LocalPlayer.CustomProperties["IceCubeIdx"];

        IceCube targetCube = StageManager.Instance.iceCubes[iceIdx];
        Vector3 cubePosition = targetCube.transform.position;
        cubePosition.y = 2.6f;
        
        MyPlayer = PhotonNetwork.Instantiate("BoardGamePlayer", cubePosition, Quaternion.identity);
        MyPlayer.GetComponent<BoardGamePlayer>().SetCamera(cameraObj.transform);
        
        var diceObj = PhotonNetwork.Instantiate("Dice", Vector3.zero, Quaternion.identity);
        MyDice = diceObj.GetComponent<Dice>();
        MyDice.Icecube = targetCube;

        
        if (CheckIsAllReady() == false)
        {
            guideUI.gameObject.SetActive(true);
        }
        else
        {
            RoomDataLoad();
        }
    }

    private void RoomDataLoad()
    {
        var roomProps = PhotonNetwork.CurrentRoom.CustomProperties;
        if (roomProps.ContainsKey("PlayerTurn"))
        {
            currentTurn = (int)roomProps["PlayerTurn"];
            IsMyTurn = currentTurn == myOrder;
            
            NextTurn();
        }
    }

    public void PickOrder(TMP_Text textComp)
    {
        int random = 0;
        bool isUnique = false;
        
        while (isUnique == false)
        {
            isUnique = true;
            random = Random.Range(0, PhotonNetwork.CurrentRoom.PlayerCount);
            
            foreach (var p in PhotonNetwork.PlayerList)
            {
                var pProps = p.CustomProperties;
                if (pProps.ContainsKey("PlayerOrder") == false)
                    continue;

                int order = (int)pProps["PlayerOrder"];
                if (order == random)
                    isUnique = false;
            }
            
        }
        
        myOrder = random;
        textComp.text = $"{myOrder+1}";
        
        Hashtable hs = new Hashtable();
        hs.Add("PlayerOrder", myOrder);
        PhotonNetwork.SetPlayerCustomProperties(hs);
    }

    public void GameReady()
    {
        Hashtable hs = new Hashtable();
        hs.Add("PlayerReady", true);
        
        PhotonNetwork.SetPlayerCustomProperties(hs);
        GameStart();
    }

    private void GameStart()
    {
        if (PhotonNetwork.IsMasterClient == false)
            return;

        if(CheckIsAllReady() == false)
            return;
        
        Hashtable hs = new Hashtable();
        hs.Add("PlayerTurn", 0);

        PhotonNetwork.CurrentRoom.SetCustomProperties(hs);
    }

    private bool CheckIsAllReady()
    {
        foreach (var p in PhotonNetwork.PlayerList)
        {
            if(p.CustomProperties.ContainsKey("PlayerReady") == false)
                return false;
        }
        
        return true;
    }

    private void TurnStart()
    {
        if(myOrder != currentTurn)
            return;
        
        MyDice.gameObject.SetActive(true);
        MyDice.DiceSetActive();
    }

    public void NextTurn()
    {
        int nextTurn = (currentTurn + 1) % PhotonNetwork.CurrentRoom.PlayerCount;
        
        Hashtable hs = new Hashtable();
        hs.Add("PlayerTurn", nextTurn);

        PhotonNetwork.CurrentRoom.SetCustomProperties(hs);
    }

    

    #endregion
    
    

}
