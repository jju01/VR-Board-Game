using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

// ���� ; Ư�� �������� ������Ʈ(=�ڱ��ڽ�)�� ȸ����Ų��.

public class Rotator : MonoBehaviour
{
    // ȸ�� ����
    public Vector3 direction = new Vector3(15, 30, 45);

    // ȸ�� �ӵ�
    public float speed = 5;

    private void Start()
    {
        // �����ϰ� ȸ�� �ӷ��� �����Ѵ�. (�������� 1 ~ 2)
        speed = Random.Range(1, 2);
        transform.DORotate(transform.localEulerAngles + direction, speed).SetEase(Ease.Linear).SetLoops(-1, LoopType.Incremental);
        transform.DOLocalMoveY(transform.position.y + 0.2f, 1.5f).SetLoops(-1, LoopType.Yoyo);
    }
}
