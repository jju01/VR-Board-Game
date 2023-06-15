using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 엔딩관리한다.

public class HHJ_EndingAnimation : MonoBehaviour
{
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

    [SerializeField]
    // 그릇 떨어질 목표위치
    private Transform buildPoint1;
    [SerializeField]
    // 숟가락 떨어질 목표위치
    private Transform dropPoint1;
    [SerializeField]
    // 빙수 떨어질 위치
    private Transform dropPoint2;
    [Space]
    // 떨어질 속도
    public float dropSpeed;
    // 오브젝트간의 간격
    public float intervalObj;

    [Space]
    [SerializeField]
    // 빙수
    private Transform shavedIce;
    // 회전속도
    public float rotSpeed = 50f;

    // 카메라
    private Transform cam;

    // 흔들림 시간
    public float shakeTime = 1f;
    // 흔들림 속도
    public float shakeSpeed = 1f;
    // 흔들림 힘 
    public float shakeAmount = 1f;

    // 플레이어
    private Transform player;

    [Space]
    [SerializeField]
    // 카메라 시작 위치지점
    private Vector3 startPos;
    [SerializeField]
    // 카메라 시작 각도조절
    private Quaternion startAngle;
    [Space]
    [SerializeField]
    // 카메라 끝 위치지점
    private Vector3 endPos;
    [SerializeField]
    // 카메라 끝 각도조절
    private Quaternion endAngle;
    [Space]
    [SerializeField]
    private Vector3 movePos;
    [SerializeField]
    private Quaternion moveAngle;
    [Space]
    // 카메라 이동속도
    public float speed = 1f;

    private Animator ani;

    // 애니메이션 재생 순서
    private bool isStart1 = false;
    private bool IsStart2 = false;


    void Start()
    {
        cam = GameObject.FindGameObjectWithTag("MainCamera").transform;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        ani = player.GetComponent<Animator>();
        shavedIce = GameObject.FindGameObjectWithTag("ShavedIce").transform;

        //EndPos();
        StartPos();

        isStart1 = false;
        IsStart2 = false;

    }


    void Update()
    {
        AnimationController();

        if(isStart1 == true)
        {
            StartMove();
        }
        if(IsStart2 == true)
        {
            EndMove();
        }
        Building();
    }
    private void AnimationController()
    {
        //StartMove();
        // 현재 진행된 애니메이션 상태를 불러온다.

        // 만일 애니메이션이 72프레임 이상진행되었다면
        if(ani.GetCurrentAnimatorStateInfo(0).IsName("metarig|Victory") &&
            ani.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.3f && 
            ani.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
        {
            isStart1 = true;
        }
        // 만일 애니메이션이 232프레임 이상진행되었다면
        if (ani.GetCurrentAnimatorStateInfo(0).IsName("metarig|Victory") && 
            ani.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.7f && 
            ani.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
        {
            isStart1 = false;
            IsStart2 = true;
        }



    }

    // 빙수 목표지점으로 떨어짐
    private void DropObject()
    {
        shavedIce.position = Vector3.Lerp(shavedIce.position, dropPoint2.position, dropSpeed * Time.deltaTime);
    }
    // 오브젝트 회전
    private void RotObject()
    {
        shavedIce.Rotate(new Vector3(0, 1, 0) * rotSpeed);
    }

    // 빙수재료 쌓기
    private void Building()
    {
        // 빙수그릇 떨어질 위치는 목표지점
        buildObj1.position = Vector3.Lerp(buildObj1.position, buildPoint1.position, dropSpeed * Time.deltaTime);
        // 얼음 떨어질 위치는 빙수그릇 지점
        buildObj2.position = Vector3.Lerp(buildObj2.position, buildObj1.position, dropSpeed * Time.deltaTime);
        // 팥 떨어질 지점은 얼음지점
        buildObj3.position = Vector3.Lerp(buildObj3.position, buildObj2.position , dropSpeed * Time.deltaTime);
        // 과일 떨어질 지점은 팥 지점
        buildObj4.position = Vector3.Lerp(buildObj4.position, buildObj3.position , dropSpeed * Time.deltaTime);

    }
    // 숟가락 떨어짐
    private void DropSpoon()
    {
        // 숟가락 떨어질 지점은 목표지점
        buildObj5.position = Vector3.MoveTowards(buildObj5.position, dropPoint1.position, speed);
    }
    // 카메라 시작위치
    public void StartPos()
    {
        // camera의 위치를 startPos의 위치로 이동시킨다.
        cam.transform.position = startPos;
        // camerae의 각도를 startPos의 각도로 조절한다.
        cam.transform.rotation = startAngle;
    }

    // 카메라 이동
    public void StartMove()
    {
        var pos = cam.transform.position;
        var rot = cam.transform.rotation;

        //// 선형보간으로 카메라를 시작위치에서 끝 위치로 이동시킨다.
        //cam.transform.position = Vector3.Lerp(Pos, endPos, 1f * Time.deltaTime);
        cam.transform.position = Vector3.Lerp(pos, movePos, speed * Time.deltaTime);
        //cam.transform.rotation = Quaternion.Lerp(startAngle, endAngle, 1f * Time.deltaTime);
        cam.transform.rotation = Quaternion.Lerp(rot, moveAngle, speed * Time.deltaTime);

    }

    public void EndMove()
    {
        var pos = cam.transform.position;
        var rot = cam.transform.rotation;

        cam.transform.position = Vector3.Slerp(pos, endPos, speed * Time.deltaTime);
        cam.transform.rotation = Quaternion.Lerp(rot, endAngle, speed * Time.deltaTime);

    }

    // 카메라 끝 위치
    public void EndPos()
    {
        // camera의 위치를 endPos의 위치로 이동시킨다.
        cam.transform.position = endPos;
        // camerae의 각도를 endPos의 각도로 조절한다.
        cam.transform.rotation = endAngle;
    }

    public void ShakeMove()
    {
        StartCoroutine(Shake());
    }

    // 카메라 흔들림
    IEnumerator Shake()
    {
        Vector3 originPos = cam.localPosition;
        float elapsedTIme = 0.0f;

        while (elapsedTIme < shakeTime)
        {
            Vector3 randomPoint = originPos + Random.insideUnitSphere * shakeAmount;
            cam.localPosition = Vector3.Lerp(cam.localPosition, randomPoint, Time.deltaTime * shakeSpeed);

            yield return null;

            elapsedTIme += Time.deltaTime;
        }
        cam.localPosition = originPos;
    }


}