using System.Collections;
using System.Collections.Generic;
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
        // 랜덤하게 회전 속력을 변경한다. (오차범위 -1 ~ +1)
        speed = speed + Random.Range(-1.0f, 1.0f);
    }


    // Update is called once per frame
    void Update()
    {
        // 자기 자신을 임의의 속도(=speed)와 특정 방향(=direction)으로 회전시킨다.
        // Time.deltaTime (=frame단위가 아니라 초시간 기준으로 코드를 호출)
       // transform.M(direction * speed * Time.deltaTime);

    }
}
