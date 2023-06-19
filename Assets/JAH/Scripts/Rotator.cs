using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

// 역할 ; 특정 방향으로 오브젝트(=자기자신)를 회전시킨다.

public class Rotator : MonoBehaviour
{
    // 회전 방향
    public Vector3 direction = new Vector3(15, 30, 45);

    // 회전 속도
    public float speed = 5;

    private void Start()
    {
        // 랜덤하게 회전 속력을 변경한다. (오차범위 1 ~ 2)
        speed = Random.Range(1, 2);
        transform.DORotate(transform.localEulerAngles + direction, speed).SetEase(Ease.Linear).SetLoops(-1, LoopType.Incremental);
        transform.DOLocalMoveY(transform.position.y + 0.2f, 1.5f).SetLoops(-1, LoopType.Yoyo);
    }
}
