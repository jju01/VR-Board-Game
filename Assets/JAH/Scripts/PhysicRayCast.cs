using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 역할 : ray로 주사위를 쏜다

public enum LeftRight
{
    LEFT, RIGHT
}
public class PhysicRayCast : MonoBehaviour
{
    //  - 손의 위치(Left? Right?)
    public LeftRight m_hand;
    // - LineRenderer
    private LineRenderer lr;
    // - PlayerHand
    public Transform playerHand;
    public float lineDistance = 100f;

    public float lineWidth = 0.5f;

    public Color startColor = Color.white;
    public Color endColor = Color.white;


    // 어느 손인지에 따라 OVR Controller 다르게 설정
    OVRInput.Controller controllertouch;

    void Start()
    {

        lr = GetComponent<LineRenderer>();
        lr.startWidth = lineWidth;
        lr.endWidth = lineWidth * 0.2f;
        lr.startColor = startColor;
        lr.endColor = endColor;


        // 왼쪽 손일 때 or 오른쪽 손일 때에 따라 OVR Touch 방향 설정
        if (VRManager.Instance.useVRController)
        {
            switch (m_hand)
            {
                case LeftRight.LEFT: controllertouch = OVRInput.Controller.LTouch; break;
                case LeftRight.RIGHT: controllertouch = OVRInput.Controller.RTouch; break;
            }
        }
    }

    void Update()
    {

        MakeLineActive();

        // A. VR Controller 사용 모드인 경우
        if (VRManager.Instance.useVRController)
        {
            // Oculus Quest2 연동 후 실행..
            if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch) || OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.LTouch))

            {
                // 주사위에 레이를 쏘면 결과가 나온다
                Ray ray = new Ray(transform.position, transform.forward);

                RaycastHit hitInfo = new RaycastHit();

                // 주사위만 ray를 맞도록
                int layerMask = 1 << LayerMask.NameToLayer("Dice");
                if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity, layerMask))
                {
                    Debug.Log("aaaa : " + hitInfo.collider.gameObject.name);
                    // Ray 맞은게 주사위(Basic Model)이라면 
                    if (hitInfo.transform.name == "Basic Model")
                    {
                        // 1. Basic Model 비활성화
                        hitInfo.collider.gameObject.SetActive(false);
                        // 주사위 결과가 나온다
                        GameManager.Instance.MyDice.RandomDice();
                    }
                }
            }
        }

        else
        {
            if (Input.GetButtonDown("Fire1"))
            {
                // 주사위에 레이를 쏘면 결과가 나온다
                Ray ray = new Ray(transform.position, transform.forward);

                RaycastHit hitInfo = new RaycastHit();

                if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity))
                {
                    // Ray 맞은게 주사위(Basic Model)이라면 
                    if (hitInfo.transform.name == "Basic Model")
                    {
                        // 1. Basic Model 비활성화
                        hitInfo.collider.gameObject.SetActive(false);
                        // 주사위 결과가 나온다
                        GameManager.Instance.MyDice.RandomDice();
                    }
                }
            }
        }
    }


    // LineRenderer 활성화 함수 
    private void MakeLine()
    {

        lr.SetPosition(0, transform.position);

        lr.SetPosition(1, transform.position + transform.forward * lineDistance * 100);
        // LineRender에서 시작해서 마우스 클릭한 지점으로 레이를 쏜다
        Ray ray = new Ray(transform.position, transform.forward);
        // Ray시선이 부딪혔을 때 부딪힌 정보를 담아둘 그릇 만든다(RaycastHit)
        RaycastHit hitInfo = new RaycastHit();

        if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity))
        {

            //  - LR 부딪힌 곳까지만 그린다.
            //  - LR 시작점 => 내 위치
            lr.SetPosition(0, transform.position); // = transform.position
            //    LR 끝점   => 내 위치 기준 + Ray의 방향에서 부딪힌 곳의 위치만큼 떨어진 곳
            //lr.SetPosition(1, hitInfo.point);
            lr.SetPosition(1, transform.position + transform.forward * hitInfo.distance);

            print("충돌");
        }
    }

    // LineRenderer 활성화 -> 1. PrimayHandTrigger / Fire2
    private void MakeLineActive()
    {
        // A. VR Controller 사용
        if (VRManager.Instance.useVRController)
        {
            // 눌렀을 떄
            if (OVRInput.Get(OVRInput.Button.PrimaryHandTrigger, OVRInput.Controller.LTouch) || OVRInput.Get(OVRInput.Button.PrimaryHandTrigger, OVRInput.Controller.RTouch))
            {
                lr.enabled = true;

                MakeLine();
            }
 
            // 눌렀다가 뗐을 때
            else if (OVRInput.GetUp(OVRInput.Button.PrimaryHandTrigger, OVRInput.Controller.LTouch) || OVRInput.GetUp(OVRInput.Button.PrimaryHandTrigger, OVRInput.Controller.RTouch))
            {
                lr.enabled = false;
            }
        }


        // B. VR Controller 미사용
        else
        {

            // B-1. Mouse 가운데 버튼 누르면 
            if (Input.GetButtonDown("Fire2"))
            {
                MakeLine(); 
            }

            // b-3. Mouse 가운데 버튼 떼면 LR 비활성화
            else if (Input.GetButtonUp("Fire2"))
            {
                lr.enabled = false;

            }
        }
    }


}
