using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using Photon.Pun;
using UnityEngine.SceneManagement;
public class HHJ_EndingAnimation2 : MonoBehaviourPunCallbacks
{
    [SerializeField]
    // 발판
    private Transform iceBlock;
    // 어느정도 뜰 것인가
    public float iceBlockPosY;
    // 속도
    public float upSpeed;

    [Space]
    [SerializeField]
    // 빙수그릇
    private Transform buildObj1;
    [SerializeField]
    // 얼음
    private Transform buildObj2;
    [SerializeField]
    // 팥
    private Transform buildObj3;
    [SerializeField]
    // 과일
    private Transform buildObj4;
    [SerializeField]
    // 숟가락
    private Transform buildObj5;

    [Space]
    [SerializeField]
    // 그릇 떨어질 목표위치
    private Transform buildPoint1;
    [SerializeField]
    // 숟가락 떨어질 목표위치
    private Transform dropSpoonPos;
    [SerializeField]
    // 왕관 떨어질 위치
    private Transform dropCrownPos;
    [SerializeField]
    private Transform dropPlayerPos;

    [Space]
    // 떨어질 속도
    public float dropSpeed;
    // 오브젝트간의 간격
    public float intervalObjY;

    // 플레이어
    private Transform player;
    // 플레이어 회전
    [SerializeField]
    private float rotPlayer;

    private Animator ani;

    [Space]
    [SerializeField]
    // 왕관
    private Transform crown;

    // 회전속도
    public float rotSpeed = 50f;

    [Space]
    [SerializeField]
    private TextMeshProUGUI winnerName;
    [SerializeField]
    private Texture[] textures;


    private bool isMoveToStart = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        ani = player.GetComponent<Animator>();
        crown = GameObject.FindGameObjectWithTag("CrownObj").transform;

        // 오브젝트 비활성화
        SetActiveObj(false);

        PlayerName();
    }

    private void Update()
    {
        // 애니메이션 재생
        StartCoroutine(EndingPlay());

        // 만일 애니메이션이 전부 실행되었다면 일정 시간 후 씬을 이동한다.
        if ((ani.GetCurrentAnimatorStateInfo(0).IsName("metarig|Victory") &&
        ani.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f))
        {
            Invoke("MoveStartScene", 5f);
        }
    }

    // 발판이 위에서 아래로 올라온다.
    private void UpIceBlock()
    {
        var posX = iceBlock.transform.position.x;
        var posZ = iceBlock.transform.position.z;
        var posIce = new Vector3(posX, iceBlockPosY, posZ);

        iceBlock.position = Vector3.Lerp(iceBlock.position, posIce, upSpeed * Time.deltaTime);
    }

    // 우승한 플레이어의 캐릭터가 위에서 아래로 내려옴
    private void DropWinner()
    {
        player.gameObject.SetActive(true);
        var speed = dropSpeed * 100f;

        player.position = Vector3.MoveTowards(player.position, dropPlayerPos.position, speed * Time.deltaTime);
    }

    // 왕관이 회전하면서 떨어짐
    private void DropCrown()
    {
        crown.gameObject.SetActive(true);
        // 왕관 회전한다.
        crown.Rotate(new Vector3(0, 0.1f, 0) * rotSpeed);

        var posY = 10f;

        // 만일 애니메이션이 일정수치 이상 진행되었다면
        if ((ani.GetCurrentAnimatorStateInfo(0).IsName("metarig|Victory") &&
            ani.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.2f &&
            ani.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f))
        {
            // 왕관을 떨어트린다.
            crown.position = Vector3.Lerp(crown.position, dropCrownPos.position, dropSpeed * Time.deltaTime);
        }

        // 만일 애니메이션이 일정수치이상 진행되었다면
        if ((ani.GetCurrentAnimatorStateInfo(0).IsName("metarig|Victory") &&
            ani.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.78f &&
            ani.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f))
        {
            ani.transform.DOLocalRotate(new Vector3(0, 135, 0), 1.5f).SetEase(Ease.Linear);

            // 왕관을 조금 위로 올린다.
            crown.position = Vector3.Lerp(crown.position,
                new Vector3(dropCrownPos.position.x, dropCrownPos.position.y + posY, dropCrownPos.position.z),
                dropSpeed * Time.deltaTime);
        }
        // 만일 애니메이션이 일정수치이상 진행되었다면
        if ((ani.GetCurrentAnimatorStateInfo(0).IsName("metarig|Victory") &&
        ani.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.97f &&
        ani.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f))
        {
            // 왕관을 많이 위로 올린다.
            crown.position = Vector3.Lerp(crown.position,
          new Vector3(dropCrownPos.position.x, dropCrownPos.position.y + posY * 2f, dropCrownPos.position.z),
          dropSpeed * Time.deltaTime);
        }
    }

    private void Build()
    {
        StartCoroutine(Building());
    }
    // 빙수쌓기
    IEnumerator Building()
    {
        buildObj1.gameObject.SetActive(true);
        buildObj2.gameObject.SetActive(true);
        buildObj3.gameObject.SetActive(true);
        buildObj4.gameObject.SetActive(true);

        var speed = dropSpeed * 100f;

        // 빙수그릇 떨어질 위치는 목표지점
        buildObj1.position = Vector3.MoveTowards(buildObj1.position, buildPoint1.position, speed * Time.deltaTime);
        yield return new WaitForSeconds(0.5f);
        // 얼음 떨어질 위치는 빙수그릇 지점
        buildObj2.position = Vector3.MoveTowards(buildObj2.position,
            new Vector3(buildObj1.position.x, buildObj1.position.y + intervalObjY,
            buildObj1.position.z), speed * Time.deltaTime);
        yield return new WaitForSeconds(0.5f);
        // 팥 떨어질 지점은 얼음지점
        buildObj3.position = Vector3.MoveTowards(buildObj3.position,
            new Vector3(buildObj2.position.x, buildObj2.position.y + intervalObjY,
            buildObj2.position.z), speed * Time.deltaTime);
        yield return new WaitForSeconds(0.5f);
        // 과일 떨어질 지점은 팥 지점
        buildObj4.position = Vector3.MoveTowards(buildObj4.position,
            new Vector3(buildObj3.position.x, buildObj3.position.y + intervalObjY,
            buildObj3.position.z), speed * Time.deltaTime);
        yield return new WaitForSeconds(1f);
        DropSpoon();
    }
    // 숟가락 떨어짐
    private void DropSpoon()
    {
        buildObj5.gameObject.SetActive(true);

        var speed = dropSpeed * 100f;
        // 숟가락 떨어질 지점은 목표지점
        buildObj5.position = Vector3.MoveTowards(buildObj5.position, dropSpoonPos.position, speed * Time.deltaTime);
    }

    // 플레이어 애니메이션 재생
    private void AnimationPlay()
    {
        ani.SetBool("isStart", true);
    }

    // 오브젝트 활성화 여부
    private void SetActiveObj(bool active)
    {
        player.gameObject.SetActive(active);
        crown.gameObject.SetActive(active);
        buildObj1.gameObject.SetActive(active);
        buildObj2.gameObject.SetActive(active);
        buildObj3.gameObject.SetActive(active);
        buildObj4.gameObject.SetActive(active);
        buildObj5.gameObject.SetActive(active);
    }

    // 우승자 이름 표시
    private void PlayerName()
    {
        Photon.Realtime.Player winner = (Photon.Realtime.Player)PhotonNetwork.CurrentRoom.CustomProperties["GameWinner"];
        winnerName.text = winner.NickName;

        int avatarIdx = (int)winner.CustomProperties["avatar"];
        SetWinner(avatarIdx);
    }
    public void SetWinner(int textureIdx)
    {
        Transform meshObj = player.transform.FindChildRecursive("RetopoFlow");
        SkinnedMeshRenderer mesh = meshObj.GetComponent<SkinnedMeshRenderer>();
        mesh.materials[0].mainTexture = textures[textureIdx];
    }
    // 스타트씬으로 이동한다
    public void MoveStartScene()
    {
        if (isMoveToStart)
            return;

        isMoveToStart = true;

        // 접속종료
        PhotonNetwork.LeaveRoom();
    }

    IEnumerator EndingPlay()
    {
        yield return new WaitForSeconds(2f);
        UpIceBlock();
        yield return new WaitForSeconds(1f);
        Build();
        yield return new WaitForSeconds(2f);
        DropWinner();
        yield return new WaitForSeconds(3f);
        DropCrown();
        AnimationPlay();
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();

        // 스타트씬으로 이동한다. 
        PhotonNetwork.LoadLevel("CJY 1");
    }

}
