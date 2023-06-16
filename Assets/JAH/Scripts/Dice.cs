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
    public GameObject returnUI;

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
    public GameObject effectparticle;

    private void Start()
    {
        pv = GetComponent<PhotonView>();
        player = GameManager.Instance.MyPlayer;

        effectaudio = GetComponent<AudioSource>();
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


        transform.DOScale(Vector3.one, 0.4f);
        transform.DOMove(transform.position, 0.4f).ChangeStartValue(transform.position + Vector3.down * 1.5f);
        
        rotX = transform.DOBlendableRotateBy(Vector3.right * 80, 0.7f).SetEase(Ease.InOutQuad).SetLoops(-1, LoopType.Yoyo).SetAutoKill(false);
        rotY =transform.DOBlendableRotateBy(Vector3.up * 360, 1.7f, RotateMode.FastBeyond360).SetEase(Ease.Linear).SetLoops(-1, LoopType.Incremental).SetAutoKill(false);
        rotZ =transform.DOBlendableRotateBy(Vector3.forward * 360, 1.5f, RotateMode.FastBeyond360).SetEase(Ease.Linear).SetLoops(-1, LoopType.Incremental).SetAutoKill(false);
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
    }

    private void MoveDone()
    {
        if(pv.IsMine == false)
            return;
        
        moveValue--;
        isMoving = false;

        if (moveValue > 0)
            MoveToNext();
        else
            GameManager.Instance.NextTurn();
    }

    
    [PunRPC]
    public void SetDice(int idx)
    {
        baseDice.gameObject.SetActive(false);
        DiceList[idx].gameObject.SetActive(true);

        // 순서를 나타내는 UI(" "님 차례입니다. 주사위를 굴리세요) 활성화
        returnUI.SetActive(true);

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

        // 순서를 나타내는 UI 비활성화
        returnUI.SetActive(false);
    
    }
}


