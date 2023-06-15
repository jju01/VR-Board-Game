using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 역할 : Target(Player)를 쫓아다니는 역할
public class CameraControllor : MonoBehaviour
{
    // - Target(= NetWorkManager_JAH의 player가 됨)
    public GameObject target;

    // - 쫓아다닐 위치(Target으로부터 얼마나 떨어져있는지)
    private Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        offset = transform.position - target.transform.position;
    }




    // Update is called once per frame
    void LateUpdate()
    {
        if (target == null)
            return;

        // 2. Target 으로부터 Offset만큼 떨어진 위치로 Camera 이동
        transform.position = target.transform.position + offset;
        // 3, Target의 방향으로 회전
        transform.rotation = target.transform.rotation;
    }
}
