using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Player(OVRCamera)가 내 위치로 오면 방향 결정하는 화살표 오브젝트 활성화
// 클릭한 방향대로 Player 이동

public class Direction : MonoBehaviour
{
    public GameObject Player;
    public GameObject NextIceCube_1;
    public GameObject NextIceCube_2;

    // Update is called once per frame
    void Update()
    {
        // Player(OVRCamera)가 내 위치로 오면 방향 결정하는 화살표 오브젝트 활성화
        if (Player.transform.position == transform.position + new Vector3(0, 3.45f, 0))
        {

        }
    }
}
