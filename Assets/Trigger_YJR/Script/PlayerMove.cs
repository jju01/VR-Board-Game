using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

// 역할 : 사용자의 입력에 따라서 좌,우,앞,뒤로 움직인다. + jump
// CharacterController 컴포넌트를 이용
// - 속도
// - 방향
// - 중력값 (jump)
// - 중력 누적값
// - 점프값

public class PlayerMove : MonoBehaviour
{
    // - 속도
    public float MoveSpeed = 10f;
    // - 방향 아래에서 구할 것임
    // - 중력값 (jump)
    public float gravity = -7.0f;
    float originGravity;
    // - 중력 누적값
    private float yVelocity;
    // - 점프 값
    public int maxJumpCount = 1;
    // - 현재 점프가능한 횟수
    public int jumpCount;

    public float jumpPower = 10.0f;
  



    // - 캐릭터 컨트롤러 가져오기 
    private CharacterController cc;
    private Rigidbody rb;
    // - 회전 값
    public float rotSpeed = 2.0f;
    // - Y축 회전 누적값
    private float rotY;
    // 
 

    void Start()
    {

        cc = GetComponent<CharacterController>();
        
        rb = GetComponent<Rigidbody>();
        originGravity = gravity;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Vector3 checkPos = transform.position + Vector3.down * 0.1f;
        float radius = 0.1f;
        Gizmos.DrawSphere(checkPos, radius);
    }


    void Update()
    {


       Move();



    }

   

    // 만일 Ground에 닿아있다면 true 반환 / 아니라면 false 반환
    bool IsGroundCheck()
    {
        // 3. Ground 레이어에 있는 Object만 체크한다.
        int layer = 1 << LayerMask.NameToLayer("Ground");
        // 2. Player 기준 아래 방향에 체크할 수 있는 Sphere를 둔다.
        //  - 위치 (내 기준 아래로 -1)
        Vector3 checkPos = transform.position + Vector3.down * 0.1f;
        //  - 거리
        float radius = 0.1f;

        // 1. Ground에 충돌했는지 안했는지 알려줌
        bool result = Physics.CheckSphere(checkPos, radius, layer);

        return result;
    }



    void SimpleMove()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 dir = new Vector3(h, 0, -v);

        Vector3 movement = dir * MoveSpeed * 100f * Time.deltaTime;

        movement.y = rb.velocity.y;

        if (Input.GetButtonDown("Jump") && IsGroundCheck())
        {
            movement.y = jumpPower;
        }

        rb.velocity = movement;
    }


    void Move()
    {
        

        // 사용자가 키보드를 누르면, 앞뒤 입력값을 받음 Z축
        float vertical = Input.GetAxis("Vertical");
        // 움직이는 방향 (x,y)
        // Vector3 dir = new Vector3(0f, 0f, vertical);
        Vector3 dir = transform.forward * vertical;
        // 사용자가 키보드를 누르면, 좌우 입력값을 받음. X축
        float horizontal = Input.GetAxis("Horizontal");

       

        // 만약, player의 발이 바닥에 닿아 있다면 jump -> y축
        //if (cc.collisionFlags == CollisionFlags.Below)
        if (cc.isGrounded)
        {
           

            // 닿은 상태에서 점프 버튼 누르면
            if (Input.GetButtonDown("Jump"))
            {
                yVelocity = jumpPower;//점프 파워만큼 점프
           
            }

        }
        //  중력 값 (y값 계산)
        yVelocity += gravity * Time.deltaTime;
        // vector.3 y값에 넣어줌
        dir.y = yVelocity;

        // 내가 키를 눌렀을 때만, 이동하도록 한다.
        //  -> 내가 눌렀을 때 vertical 에 담기는 크기가 0.1보다 크면, 키를 누른 셈치자
        // 캐릭터 컨트롤러이용한 x,y,z 움직임.
        cc.Move(dir * MoveSpeed * Time.deltaTime);
        

    }

    void RotateDirection()
    {
        // 만일, 좌우 방향키 눌렀을 때
        if (Input.GetButtonDown("Horizontal"))
        {
            // 왼쪽 버튼 누르면
            if (Input.GetAxisRaw("Horizontal") < 0)
            {
                // Y축 기준으로 왼쪽(-방향)으로 90도 회전
                float rotateY = transform.eulerAngles.y - 90f;
                // 회전해야할 각도로 갱신
                transform.eulerAngles = new Vector3(0, rotateY, 0);

            }
            // 오른쪽 버튼 누르면 
            else
            {
                // Y축 기준으로 왼쪽(-방향)으로 90도 회전
                float rotateY = transform.eulerAngles.y + 90f;
                //Y축 기준으로 오른쪽(+방향)으로 90도 회전
                transform.eulerAngles = new Vector3(0, rotateY, 0);
            }
        }


        // 사용자가 키보드를 누르면, 좌우 입력값을 받음. X축
        //float horizontal = Input.GetAxis("Horizontal");


        // 내 회전 데이터
        //rotY += horizontal * (rotSpeed) * Time.deltaTime;

        // Y축 기준으로 나를 회전
        //transform.eulerAngles = new Vector3(0, rotY, 0);
    }

}

