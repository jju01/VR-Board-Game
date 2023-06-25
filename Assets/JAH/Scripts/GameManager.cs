using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviourPunCallbacks
{
    public string nickname;
    public string Minigame1Scene;
    public string Minigame2Scene;
    public string EndingScene;

    public GameObject mainOVRCamera;
    public GameObject islandOVRCamera;

    public Camera uiCameraForMain;
    public Camera uiCameraForIsland;
    
    public GameObject islandPref;
    private GameObject islandObj;
    
    // VR Controller 사용 여부
    public bool useVRController = false;
    
    // 싱글톤 준비
    public static GameManager Instance;



    public GameObject guideUI;
    // 아이템 인벤토리 UI
    public GameObject iteminventoryUI;

    public ItemManager _itemManager;
    public MiniGameItemUI _miniGameItemUI;
    public LerpList _lerpList;
    public TriggerNoticeManager _triggerNoticeManager;
    public CanvasList _canvasList;

    // 다른사람들에게  알려주는 UI
    public GameObject minigameresultUI;
    public GameObject cameraObj;
    public GameObject MyPlayer;
    public Dice MyDice;

    // 미니게임 인트로
    public GameObject minigameFruit_Itr;
    public GameObject minigameIce_Itr;

    // 무인도 가이드 UI
    public GameObject IslandUI;

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
        Time.timeScale = 1;
        
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
        {
            BoardGameReady();
        }
        else
        {
            PhotonNetwork.AutomaticallySyncScene = true;

            PhotonNetwork.ConnectUsingSettings();
        }
    }
    

    #region Pun Callback

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        PhotonNetwork.NickName = nickname;
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
        
        if (changedProps.ContainsKey("BoardGameReady"))
        {
            GameStart();
        }

        if (changedProps.ContainsKey("triggerNotice"))
        {
            // UI 띄워주기
            int triggerIdx = (int)changedProps["triggerNotice"];
            _triggerNoticeManager.TriggerNotice(targetPlayer.NickName, (Trigger.Type)triggerIdx);
        }        
    }

    public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
    {
        base.OnRoomPropertiesUpdate(propertiesThatChanged);

        if (propertiesThatChanged.ContainsKey("winItemOwner"))
        {
            if (propertiesThatChanged["winItemOwner"] == null)
            {
                if (propertiesThatChanged.ContainsKey("winPlayer"))
                {
                    var winPlayer = (Photon.Realtime.Player)propertiesThatChanged["winPlayer"];
                    var targetPlayer = (Photon.Realtime.Player)propertiesThatChanged["targetPlayer"];
                    var item = propertiesThatChanged["item"].ToString();

                    if (winPlayer.IsLocal)
                    {
                        int itemCount = 0;
                        if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey(item))
                            itemCount = (int)PhotonNetwork.LocalPlayer.CustomProperties[item];

                        itemCount++;
            
                        Hashtable hs = new Hashtable();
                        hs.Add(item, itemCount);
                        PhotonNetwork.SetPlayerCustomProperties(hs);
                    }
                
                    if (targetPlayer.IsLocal)
                    {
                        int itemCount = 0;
                        if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey(item))
                            itemCount = (int)PhotonNetwork.LocalPlayer.CustomProperties[item];

                        itemCount--;
            
                        Hashtable hs = new Hashtable();
                        hs.Add(item, itemCount);
                        PhotonNetwork.SetPlayerCustomProperties(hs);
                    }

                    _miniGameItemUI.NoticeItemState(winPlayer.NickName, targetPlayer.NickName, item);
                }

                
                Invoke("GameStart", 4.5f);
                
            }
        }

        if (propertiesThatChanged.ContainsKey("TriggerNotice_1_Player"))
        {
            string playerName = propertiesThatChanged["TriggerNotice_1_Player"].ToString();
            _triggerNoticeManager.SetTriggerNotice1(playerName);
        }
        
        if (propertiesThatChanged.ContainsKey("TriggerNotice_2_Player"))
        {
            string playerName = propertiesThatChanged["TriggerNotice_2_Player"].ToString();
            string targetName = propertiesThatChanged["TriggerNotice_2_Target"].ToString();
            _triggerNoticeManager.SetTriggerNotice2(playerName, targetName);
        }
        
        if (propertiesThatChanged.ContainsKey("TriggerNotice_3_Player"))
        {
            string playerName = propertiesThatChanged["TriggerNotice_3_Player"].ToString();
            _triggerNoticeManager.SetTriggerNotice3(playerName);
        }
        
        if (propertiesThatChanged.ContainsKey("PlayerTurn"))
        {
            _itemManager.RefreshItemState();
            
            currentTurn = (int)propertiesThatChanged["PlayerTurn"];
            IsMyTurn = currentTurn == myOrder;

            int bTurn = 0;
            if (propertiesThatChanged.ContainsKey("BoardGameTurn"))
                bTurn = (int)propertiesThatChanged["BoardGameTurn"];

            //if (bTurn == 2)
            //    StartCoroutine(MinigameIce());
            //else if (bTurn == 4)
            //    StartCoroutine(MinigameFruit());


            else
                TurnStart();
        }

        if (propertiesThatChanged.ContainsKey("itemSetting"))
        {
            ItemPosition.Instance.SetItemFromNetwork();
        }
        
        if (propertiesThatChanged.ContainsKey("triggerSetting"))
        {
            ItemPosition.Instance.SetTriggerFromNetwork();
        }
        
        if (propertiesThatChanged.ContainsKey("GameWinner"))
        {
            SceneLoad(EndingScene);
        }
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        if (newPlayer.CustomProperties.ContainsKey("PlayerTurn"))
            GameStart();
    }

    #endregion


    #region BoardGame Logic

    private void BoardGameReady()
    {
        int iceIdx = 0;

        if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("IceCubeIdx"))
            iceIdx = (int)PhotonNetwork.LocalPlayer.CustomProperties["IceCubeIdx"];

        Quaternion playerRot = Quaternion.identity;
        if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("PlayerRot"))
            playerRot = (Quaternion)PhotonNetwork.LocalPlayer.CustomProperties["PlayerRot"];

        IceCube targetCube = StageManager.Instance.iceCubes[iceIdx];
        Vector3 cubePosition = targetCube.transform.position + (targetCube.transform.up*2.2f);
        //cubePosition.y = 0.0001f;

        if (MyPlayer == null)
        {
            MyPlayer = PhotonNetwork.Instantiate("BoardGamePlayer", cubePosition, playerRot);
            MyPlayer.GetComponent<BoardGamePlayer>().SetCamera(cameraObj.transform);
            
            PhotonView pv = MyPlayer.GetComponent<PhotonView>();
            Hashtable hs = new Hashtable();
            hs.Add("PlayerView", pv.ViewID);
            PhotonNetwork.SetPlayerCustomProperties(hs);
        }

        if (MyDice == null)
        {
            var diceObj = PhotonNetwork.Instantiate("Dice", Vector3.zero, Quaternion.identity);
            MyDice = diceObj.GetComponent<Dice>();
            MyDice.Icecube = targetCube;
            
            PhotonView pv = MyDice.GetComponent<PhotonView>();
            Hashtable hs = new Hashtable();
            hs.Add("DiceView", pv.ViewID);
            PhotonNetwork.SetPlayerCustomProperties(hs);
        }

        if (CheckIsAllReady() == false)
        {
            guideUI.gameObject.SetActive(true);
        }
        else
        {
            RoomDataLoad();
        }
        
        
        ItemPosition.Instance.SetItemFromNetwork();
        ItemPosition.Instance.SetTriggerFromNetwork();
                
        List<int> randPool = InitRandomPool();
        if(randPool == null)
            return;
        
        InitItemInfo(randPool);
        InitTriggerInfo(randPool);
    }

    private void RoomDataLoad()
    {
        iteminventoryUI.SetActive(true);

        var roomProps = PhotonNetwork.CurrentRoom.CustomProperties;

        if (roomProps.ContainsKey("PlayerTurn"))
        {
            currentTurn = (int)roomProps["PlayerTurn"];
            myOrder = (int)PhotonNetwork.LocalPlayer.CustomProperties["PlayerOrder"];
            IsMyTurn = currentTurn == myOrder;

            bool inIsland = PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("island"); 
            if (inIsland)
            {
                GoToIsland();
            }

            if (roomProps.ContainsKey("winItemOwner"))
            {
                Photon.Realtime.Player player = (Photon.Realtime.Player)roomProps["winItemOwner"];
                if (player.IsLocal)
                {
                    _miniGameItemUI.OpenUI();
                }

                return;
            }
            
            if (inIsland && IsMyTurn)
            {
                Hashtable hs = new Hashtable();
                hs.Add("island", null);
                PhotonNetwork.LocalPlayer.SetCustomProperties(hs);
                    
                NextTurn();
                return;
            }
            
            TurnStart();
        }
    }

    public void PickOrder(CardSelector card)
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
        card.SetCard(myOrder);

        Hashtable hs = new Hashtable();
        hs.Add("PlayerOrder", myOrder);
        PhotonNetwork.SetPlayerCustomProperties(hs);
    }

    public void GameReady()
    {
        Hashtable hs = new Hashtable();
        hs.Add("BoardGameReady", true);
        
        PhotonNetwork.SetPlayerCustomProperties(hs);
        GameStart();
    }

    private void GameStart()
    {
        if (PhotonNetwork.IsMasterClient == false)
            return;
        
        if(CheckIsAllReady() == false)
            return;

        Room room = PhotonNetwork.CurrentRoom;

        int turn = 0;
        if(room.CustomProperties.ContainsKey("PlayerTurn"))
            turn = (int)room.CustomProperties["PlayerTurn"];
        
        Debug.Log($"GameStart - Turn : {turn}");
        Hashtable hs = new Hashtable();
        hs.Add("PlayerTurn", turn);

        PhotonNetwork.CurrentRoom.SetCustomProperties(hs);
    }

    public void SceneLoad(string sceneName)
    {
        if(PhotonNetwork.IsMasterClient == false)
            return;
        
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.LoadLevel(sceneName);
    }

    private bool CheckIsAllReady()
    {
        foreach (var p in PhotonNetwork.PlayerList)
        {
            if(p.CustomProperties.ContainsKey("BoardGameReady") == false)
                return false;
        }
        
        return true;
    }

    private void TurnStart()
    {
        if(myOrder != currentTurn)
            return;
        
        if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("island"))
        {
            BackToMainBoardGame();
        }
        else
        {
            MyDice.gameObject.SetActive(true);
            MyDice.DiceSetActive();   
        }
    }
    
    public void NextTurn()
    {
        Room room = PhotonNetwork.CurrentRoom;
        int nextTurn = (currentTurn + 1) % room.PlayerCount;
        
        Hashtable hs = new Hashtable();
        hs.Add("PlayerTurn", nextTurn);

        int bTurn = 0;

        if (room.CustomProperties.ContainsKey("BoardGameTurn"))
            bTurn = (int)room.CustomProperties["BoardGameTurn"];

        bTurn++;
        
        if(nextTurn == 0)
            hs.Add("BoardGameTurn", bTurn);
        
        PhotonNetwork.CurrentRoom.SetCustomProperties(hs);
    }

    public void SavePosition(int cubeIdx)
    {
        Hashtable hs = new Hashtable();
        hs.Add("IceCubeIdx", cubeIdx);
        hs.Add("PlayerRot", transform.rotation);
        
        PhotonNetwork.SetPlayerCustomProperties(hs);
    }

    public List<int> InitRandomPool()
    {
        if(PhotonNetwork.IsMasterClient == false)
            return null;

        if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("itemSetting") || PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("triggerSetting"))
            return null;
        
        StageManager stage = StageManager.Instance; 
        List<int> itemPosRandPool = new List<int>();
        for (int i = 0; i < stage.iceCubes.Count; i++)
        {
            if(i != stage.startCubeIdx && i != stage.islandCubeIdx)
                itemPosRandPool.Add(i);
        }

        return itemPosRandPool;
    }

    public void InitItemInfo(List<int> randPool)
    {
        if(PhotonNetwork.IsMasterClient == false)
            return;
        
        if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("itemSetting") == false)
        {
            int itemCount = ItemPosition.Instance.items.Count;
            int[] items = new int[itemCount];
            for (int i = 0; i < items.Length; i++)
            {
                int randPos = Random.Range(0, randPool.Count);
                items[i] = randPool[randPos];
                randPool.RemoveAt(randPos);
            }

            Hashtable hs = new Hashtable();
            hs.Add("itemSetting", items);
            PhotonNetwork.CurrentRoom.SetCustomProperties(hs);
        }
    }
    
    public void InitTriggerInfo(List<int> randPool)
    {
        if(PhotonNetwork.IsMasterClient == false)
            return;
        
        if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("triggerSetting") == false)
        {
            int triggerCount = ItemPosition.Instance.triggers.Count;
            int[] triggers = new int[triggerCount];
            for (int i = 0; i < triggers.Length; i++)
            {
                int randPos = Random.Range(0, randPool.Count);
                triggers[i] = randPool[randPos];
                randPool.RemoveAt(randPos);
            }

            Hashtable hs = new Hashtable();
            hs.Add("triggerSetting", triggers);
            PhotonNetwork.CurrentRoom.SetCustomProperties(hs);
        }
    }

    public void GetItem(int itemIdx, int itemType)
    {
        if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("itemSetting"))
        {
            int[] itemSettings = (int[])PhotonNetwork.CurrentRoom.CustomProperties["itemSetting"];
            itemSettings[itemIdx] = -1;
            
            Hashtable hs = new Hashtable();
            hs.Add("itemSetting", itemSettings);
            PhotonNetwork.CurrentRoom.SetCustomProperties(hs);
        }

        {
            int itemCount = 0;
            if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey($"itemType_{itemType}"))
                itemCount = (int)PhotonNetwork.LocalPlayer.CustomProperties[$"itemType_{itemType}"];

            itemCount++;
            
            Hashtable hs = new Hashtable();
            hs.Add($"itemType_{itemType}", itemCount);
            PhotonNetwork.SetPlayerCustomProperties(hs);
        }
    }

    public void SetWinner()
    {
        Hashtable hs = new Hashtable();
        hs.Add("GameWinner", PhotonNetwork.LocalPlayer);
        PhotonNetwork.CurrentRoom.SetCustomProperties(hs);
    }

    public void GoToIsland()
    {
        Hashtable hs = new Hashtable();
        hs.Add("island", PhotonNetwork.LocalPlayer);
        PhotonNetwork.SetPlayerCustomProperties(hs);

        if (islandObj == null)
            islandObj = Instantiate(islandPref);
        
        islandObj.SetActive(true);
        
        mainOVRCamera.SetActive(false);
        islandOVRCamera.SetActive(true);
        
        foreach (var lerpUI in _lerpList.lerUis)
            lerpUI.target = islandOVRCamera.transform;
        
        foreach (var canvas in _canvasList.canvasList)
            canvas.worldCamera = uiCameraForIsland;

        StartCoroutine(IslandUIActive());
    }

    public void BackToMainBoardGame()
    {
        Hashtable hs = new Hashtable();
        hs.Add("island", null);
        PhotonNetwork.LocalPlayer.SetCustomProperties(hs);

        if (islandObj != null)
            islandObj.SetActive(false);
        
        mainOVRCamera.SetActive(true);
        islandOVRCamera.SetActive(false);
        
        foreach (var lerpUI in _lerpList.lerUis)
            lerpUI.target = mainOVRCamera.transform;
        
        foreach (var canvas in _canvasList.canvasList)
            canvas.worldCamera = uiCameraForMain;
        
        NextTurn();
    }

    public void SetWinnerItem()
    {
        var roomProps = PhotonNetwork.CurrentRoom.CustomProperties;
        if (roomProps.ContainsKey("winItemOwner"))
            return;
        
        Hashtable hs = new Hashtable();
        hs.Add("winItemOwner", PhotonNetwork.LocalPlayer);
        PhotonNetwork.CurrentRoom.SetCustomProperties(hs);
    }

    public void NoticeTrigger(int triggerIdx)
    {
        Hashtable hs = new Hashtable();
        hs.Add("triggerNotice", triggerIdx);
        PhotonNetwork.SetPlayerCustomProperties(hs);
    }

    #endregion



    // 미니게임 인트로 나오고나서 신전환!
    IEnumerator MinigameFruit()
    {
        minigameFruit_Itr.gameObject.SetActive(true);
        print("실행1");
        yield return new WaitForSeconds(5);
        minigameFruit_Itr.gameObject.SetActive(false);
        print("실행2");
        SceneLoad(Minigame1Scene);
    }

    IEnumerator MinigameIce()
    {
        minigameIce_Itr.gameObject.SetActive(true);
        print("실행1");
        yield return new WaitForSeconds(5);
        minigameIce_Itr.gameObject.SetActive(false);
        print("실행2");
        SceneLoad(Minigame2Scene);
    }


    IEnumerator IslandUIActive()
    {
        IslandUI.gameObject.SetActive(true);
        print("실행1");
        yield return new WaitForSeconds(3);
        IslandUI.gameObject.SetActive(false);
    }
}
