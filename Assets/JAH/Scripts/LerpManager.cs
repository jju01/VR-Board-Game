using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 역할 : Target 을 쫓아 Lerp 이동
//  - 이동 속도
//  - Target

public class LerpManager : MonoBehaviour
{
    //  - 이동 속도
    public float speed = 5.0f;
    //  - Target
    public Transform target;
    // 회전속도
    public float rotSpeed = 3.0f;

    public float fowardFactor = 0;
    public float upFactor = 0;

    private void Update()
    {
        if(GameManager.Instance.MyPlayer == null)
            return;

        if (target == null)
            target = GameManager.Instance.MyPlayer.transform;
        
        FollowUI();
    }


    public void FollowUI()
    {
        //  Target 을 쫓아 Lerp 이동
        transform.position
            = Vector3.Lerp(transform.position, target.position + target.transform.forward * fowardFactor + target.transform.up * upFactor, speed * Time.deltaTime);
        // Target이 바라보는 방향으로 회전
        transform.rotation = Quaternion.Lerp(transform.rotation, target.rotation, rotSpeed * Time.deltaTime);
    }
}
