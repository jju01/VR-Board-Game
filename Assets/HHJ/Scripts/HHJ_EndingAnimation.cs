using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���������Ѵ�.

public class HHJ_EndingAnimation : MonoBehaviour
{
    [SerializeField]
    // �����׸�
    private Transform buildObj1;
    [SerializeField]
    // ����
    private Transform buildObj2;
    [SerializeField]
    // ��
    private Transform buildObj3;
    [SerializeField]
    // ����
    private Transform buildObj4;
    [SerializeField]
    // ������
    private Transform buildObj5;

    [SerializeField]
    // �׸� ������ ��ǥ��ġ
    private Transform buildPoint1;
    [SerializeField]
    // ������ ������ ��ǥ��ġ
    private Transform dropPoint1;
    [SerializeField]
    // ���� ������ ��ġ
    private Transform dropPoint2;
    [Space]
    // ������ �ӵ�
    public float dropSpeed;
    // ������Ʈ���� ����
    public float intervalObj;

    [Space]
    [SerializeField]
    // ����
    private Transform shavedIce;
    // ȸ���ӵ�
    public float rotSpeed = 50f;

    // ī�޶�
    private Transform cam;

    // ��鸲 �ð�
    public float shakeTime = 1f;
    // ��鸲 �ӵ�
    public float shakeSpeed = 1f;
    // ��鸲 �� 
    public float shakeAmount = 1f;

    // �÷��̾�
    private Transform player;

    [Space]
    [SerializeField]
    // ī�޶� ���� ��ġ����
    private Vector3 startPos;
    [SerializeField]
    // ī�޶� ���� ��������
    private Quaternion startAngle;
    [Space]
    [SerializeField]
    // ī�޶� �� ��ġ����
    private Vector3 endPos;
    [SerializeField]
    // ī�޶� �� ��������
    private Quaternion endAngle;
    [Space]
    [SerializeField]
    private Vector3 movePos;
    [SerializeField]
    private Quaternion moveAngle;
    [Space]
    // ī�޶� �̵��ӵ�
    public float speed = 1f;

    private Animator ani;

    // �ִϸ��̼� ��� ����
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
        // ���� ����� �ִϸ��̼� ���¸� �ҷ��´�.

        // ���� �ִϸ��̼��� 72������ �̻�����Ǿ��ٸ�
        if(ani.GetCurrentAnimatorStateInfo(0).IsName("metarig|Victory") &&
            ani.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.3f && 
            ani.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
        {
            isStart1 = true;
        }
        // ���� �ִϸ��̼��� 232������ �̻�����Ǿ��ٸ�
        if (ani.GetCurrentAnimatorStateInfo(0).IsName("metarig|Victory") && 
            ani.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.7f && 
            ani.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
        {
            isStart1 = false;
            IsStart2 = true;
        }



    }

    // ���� ��ǥ�������� ������
    private void DropObject()
    {
        shavedIce.position = Vector3.Lerp(shavedIce.position, dropPoint2.position, dropSpeed * Time.deltaTime);
    }
    // ������Ʈ ȸ��
    private void RotObject()
    {
        shavedIce.Rotate(new Vector3(0, 1, 0) * rotSpeed);
    }

    // ������� �ױ�
    private void Building()
    {
        // �����׸� ������ ��ġ�� ��ǥ����
        buildObj1.position = Vector3.Lerp(buildObj1.position, buildPoint1.position, dropSpeed * Time.deltaTime);
        // ���� ������ ��ġ�� �����׸� ����
        buildObj2.position = Vector3.Lerp(buildObj2.position, buildObj1.position, dropSpeed * Time.deltaTime);
        // �� ������ ������ ��������
        buildObj3.position = Vector3.Lerp(buildObj3.position, buildObj2.position , dropSpeed * Time.deltaTime);
        // ���� ������ ������ �� ����
        buildObj4.position = Vector3.Lerp(buildObj4.position, buildObj3.position , dropSpeed * Time.deltaTime);

    }
    // ������ ������
    private void DropSpoon()
    {
        // ������ ������ ������ ��ǥ����
        buildObj5.position = Vector3.MoveTowards(buildObj5.position, dropPoint1.position, speed);
    }
    // ī�޶� ������ġ
    public void StartPos()
    {
        // camera�� ��ġ�� startPos�� ��ġ�� �̵���Ų��.
        cam.transform.position = startPos;
        // camerae�� ������ startPos�� ������ �����Ѵ�.
        cam.transform.rotation = startAngle;
    }

    // ī�޶� �̵�
    public void StartMove()
    {
        var pos = cam.transform.position;
        var rot = cam.transform.rotation;

        //// ������������ ī�޶� ������ġ���� �� ��ġ�� �̵���Ų��.
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

    // ī�޶� �� ��ġ
    public void EndPos()
    {
        // camera�� ��ġ�� endPos�� ��ġ�� �̵���Ų��.
        cam.transform.position = endPos;
        // camerae�� ������ endPos�� ������ �����Ѵ�.
        cam.transform.rotation = endAngle;
    }

    public void ShakeMove()
    {
        StartCoroutine(Shake());
    }

    // ī�޶� ��鸲
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