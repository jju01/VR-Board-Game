using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Photon.Pun;
using Random = UnityEngine.Random;

// 역할 1: 주사위를 굴린다
// 역할 2: 주사위 수만큼 Player를 이동시킨다 (IceCube 스크립트)

public class Dice : MonoBehaviour
{
    private GameObject player;

    public GameObject baseDice;

    // 주사위 리스트 (Basic Model, 1~6)
    public List<GameObject> DiceList = new List<GameObject>();

    // IceCube
    public IceCube Icecube { get; set; }

    // 주사위 번호
    private int curDice;
    public int moveValue;

    // 주사위 이동
    public bool isMoving  = false;
    public bool isRolling  = false;
    
    private PhotonView pv;

    private Tweener rotX;
    private Tweener rotY;
    private Tweener rotZ;

    // 주사위 결과 나올 때 효과음
    private AudioSource effectaudio;
    // >> 파티클
    public GameObject effectparticlePref;
    public GameObject effectparticle;

    // 주사위 차례를 알려주는 UI
    public GameObject diceturnuiPref;
    public GameObject diceturnui;

    private void Awake()
    {
        InitDice();
    }

    private void InitDice()
    {
        if(pv == null)
            pv = GetComponent<PhotonView>();
        
        if(player == null)
            player = GameManager.Instance.MyPlayer;

        if(effectaudio == null)
            effectaudio = GetComponent<AudioSource>();
        
        if (effectparticle == null)
        {
            effectparticle = Instantiate(effectparticlePref);
            effectparticle.SetActive(false);
        }
        
        if(diceturnui == null)
        {
            diceturnui = Instantiate(diceturnuiPref);
            diceturnui.SetActive(false);
        }
        rotX = transform.DOBlendableRotateBy(Vector3.right * 80, 0.7f).SetEase(Ease.InOutQuad).SetLoops(-1, LoopType.Yoyo).SetAutoKill(false);
        rotY = transform.DOBlendableRotateBy(Vector3.up * 360, 1.7f, RotateMode.FastBeyond360).SetEase(Ease.Linear).SetLoops(-1, LoopType.Incremental).SetAutoKill(false);
        rotZ = transform.DOBlendableRotateBy(Vector3.forward * 360, 1.5f, RotateMode.FastBeyond360).SetEase(Ease.Linear).SetLoops(-1, LoopType.Incremental).SetAutoKill(false);
    }

    // 주사위 이동 값(= curDice +1)
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            RandomDice();
    }

    // 주사위(Basic Model 활성화)
    public void DiceSetActive()
    {
        if(pv.IsMine == false)
            return;

        isRolling = false;
        
        Transform camTr = GameManager.Instance.cameraObj.transform;
        transform.position = camTr.position + (camTr.forward * 2);

        RandomDiceList();
        
        baseDice.SetActive(true);
        // //주사위 차례를 알려주는 UI활성화
        StartCoroutine(DiceTurnUI());

        effectparticle.transform.position = transform.position + (Vector3.down * 0.486f);
        
        transform.DOScale(Vector3.one, 0.4f);
        transform.DOMove(transform.position, 0.4f).ChangeStartValue(transform.position + Vector3.down * 1.5f);
        
        rotX.Play();
        rotY.Play();
        rotZ.Play();
    }

    // 1. 주사위 나왔을 때 Ray를 쏘고 ,
    // 2. 주사위 결과가 나오고
    // 3. 주사위 수만큼 움직이게 하는 함수
    public void RandomDice()
    {
        if(pv.IsMine == false)
            return;
              
        if(GameManager.Instance.IsMyTurn == false)
            return;
        
        if(isRolling)
            return;
        
        isRolling = true;
        
        // 2. 1~6 주사위 랜덤 활성화
        curDice = Random.Range(0, DiceList.Count);
        moveValue = curDice+1;

        rotX.Pause();
        rotY.Pause();
        rotZ.Pause();
        
        transform.DOPunchScale(Vector3.one * 0.5f, 0.4f, 1);
        transform.DORotate(Vector3.zero, 0.5f);
        transform.DOScale(Vector3.zero, 0.2f).SetDelay(1.5f);

        pv.RPC("SetDice", RpcTarget.All, curDice);
        
        // 3. 1초 뒤 숫자 UI  + 파티클 + 효과음 재생
        // 4. 1초 뒤 숫자 UI  + 파티클 + 효과음 비활성화

        // 5. 2초 뒤 Player를 주사위 수만큼 이동시킴 
        Invoke("MoveToNext", 2.5f);
    }
    
    // 랜덤 주사위 비활성화 
    private void RandomDiceList()
    {
        if(pv.IsMine == false)
            return;
        
        pv.RPC("ResetDice", RpcTarget.All, curDice);
    }

   // Player를 주사위 숫자만큼 이동
   private void MoveToNext()
    {
        if(pv.IsMine == false)
            return;
        
        if (isMoving || (moveValue <= 0))
            return;

        isMoving = true;
        Icecube.GetNext(MoveTo);
    }


    public void MoveTo(IceCube target)
    {
        if(pv.IsMine == false)
            return;
        
        Icecube = target;
        Vector3 goalPos = target.transform.position;
        goalPos.y = player.transform.position.y;

        // player를 이동시킨다
        player.transform.DOMove(goalPos, 0.8f).OnComplete(MoveDone);
        player.transform.DOLookAt(goalPos, 0.5f);
        // player walk 애니메이션 실행
        BoardGamePlayer.Instance.anim.SetBool("Idle", false);
        BoardGamePlayer.Instance.anim.SetBool("Walk", true);
    }

    private void MoveDone()
    {
        if(pv.IsMine == false)
            return;
        
        moveValue--;
        isMoving = false;

        // 애니메이션 Idle 모드
        if(moveValue <= 1)
        {
            BoardGamePlayer.Instance.anim.SetBool("Walk", false);
            BoardGamePlayer.Instance.anim.SetBool("Idle", true);
        }
        if (moveValue > 0)
            MoveToNext();
        else
        {
            if (Icecube.trigger)
            {
                Invoke("TriggerAction", 5f);
            }
            else
            {
                DoneAction();
            }
        }
    }

    private void TriggerAction()
    {
        switch (Icecube.trigger.type)
        {
            case Trigger.Type.A:
                int random = Random.Range(1, 4);
                moveValue = random;
                MoveToNext();
                break;
            
            case Trigger.Type.B:
                List<Photon.Realtime.Player> playerList = new List<Photon.Realtime.Player>();
                foreach (var player in PhotonNetwork.PlayerList)
                {
                    if(player.IsLocal)
                        continue;
                    
                    if(player.CustomProperties.ContainsKey("island"))
                        continue;
                    
                    playerList.Add(player);
                }

                if (playerList.Count == 0)
                    DoneAction();
                else
                {
                    int randomPlayer = Random.Range(0, playerList.Count);
                    int playerViewId = (int)playerList[randomPlayer].CustomProperties["PlayerView"];

                    {
                        PhotonView pv = PhotonView.Find(playerViewId);
                        BoardGamePlayer bPlayer = pv.GetComponent<BoardGamePlayer>();
                        BoardGamePlayer mPlayer = GameManager.Instance.MyPlayer.GetComponent<BoardGamePlayer>();

                        Vector3 bPlayerPos = bPlayer.transform.position;
                        Vector3 mPlayerPos = mPlayer.transform.position;

                        bPlayer.SetPosition(mPlayerPos);
                        mPlayer.SetPosition(bPlayerPos);
                    }

                    {
                        int diceView = (int)playerList[randomPlayer].CustomProperties["DiceView"];
                        PhotonView pv = PhotonView.Find(diceView);
                        Dice bDice = pv.GetComponent<Dice>();
                        
                        int mCubeIdx = StageManager.Instance.GetIceCubeIdx(Icecube);

                        int bCubeIdx = 0;
                        if(playerList[randomPlayer].CustomProperties.ContainsKey("IceCubeIdx"))
                            bCubeIdx = (int)playerList[randomPlayer].CustomProperties["IceCubeIdx"];
                        
                        Debug.Log($"bCubeIdx : {bCubeIdx}      mCubeIdx : {mCubeIdx}");
                        
                        bDice.SetCube(mCubeIdx);
                        SetCube(bCubeIdx);
                    }

                    Invoke("DoneAction", 1.5f);
                    Invoke("DiceTurnUI", 1.5f);
                }

                break;
            
            case Trigger.Type.C:
                DiceSetActive();
                break;
        }
    }

    private void DoneAction()
    {
        int cubePosition = StageManager.Instance.GetIceCubeIdx(Icecube); 
        GameManager.Instance.SavePosition(cubePosition);
        GameManager.Instance.NextTurn();
            
        // if(cubePosition == StageManager.Instance.islandCubeIdx)
        //     GameManager.Instance.GoToIsland();
    }


    [PunRPC]
    public void SetDice(int idx)
    {
        baseDice.gameObject.SetActive(false);
        DiceList[idx].gameObject.SetActive(true);

        // 주사위 결과 나올 때 파티클,
        effectparticle.SetActive(true);

      
        // 효과음 활성화
        effectaudio.Stop();
        effectaudio.Play();
        
    }
    
    [PunRPC]
    public void ResetDice(int idx)
    {
        effectparticle.SetActive(false);
        baseDice.gameObject.SetActive(true);
        DiceList[idx].gameObject.SetActive(false);

    
    }

    
    public void SetCube(int idx)
    {
        pv.RPC("RecvCube", RpcTarget.All, idx);
    }

    [PunRPC]
    public void RecvCube(int idx)
    {
        Debug.Log($"RecvCube : {idx}");
        Icecube = StageManager.Instance.iceCubes[idx];
    }

    //주사위 차례를 알려주는 UI활성화
    IEnumerator DiceTurnUI()
    {
        yield return new WaitForSeconds(4.5f);
        diceturnui.SetActive(true);
        // 2) 5초 기다림
        yield return new WaitForSeconds(5.0f);
        diceturnui.SetActive(false);
        
    }
}


