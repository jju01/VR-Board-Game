using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

// ���� : ������� �Է¿� ���� ��,��,��,�ڷ� �����δ�. + jump
// CharacterController ������Ʈ�� �̿�
// - �ӵ�
// - ����
// - �߷°� (jump)
// - �߷� ������
// - ������

public class PlayerMove : MonoBehaviour
{
    // - �ӵ�
    public float MoveSpeed = 10f;
    // - ���� �Ʒ����� ���� ����
    // - �߷°� (jump)
    public float gravity = -7.0f;
    float originGravity;
    // - �߷� ������
    private float yVelocity;
    // - ���� ��
    public int maxJumpCount = 1;
    // - ���� ���������� Ƚ��
    public int jumpCount;

    public float jumpPower = 10.0f;
  



    // - ĳ���� ��Ʈ�ѷ� �������� 
    private CharacterController cc;
    private Rigidbody rb;
    // - ȸ�� ��
    public float rotSpeed = 2.0f;
    // - Y�� ȸ�� ������
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

   

    // ���� Ground�� ����ִٸ� true ��ȯ / �ƴ϶�� false ��ȯ
    bool IsGroundCheck()
    {
        // 3. Ground ���̾ �ִ� Object�� üũ�Ѵ�.
        int layer = 1 << LayerMask.NameToLayer("Ground");
        // 2. Player ���� �Ʒ� ���⿡ üũ�� �� �ִ� Sphere�� �д�.
        //  - ��ġ (�� ���� �Ʒ��� -1)
        Vector3 checkPos = transform.position + Vector3.down * 0.1f;
        //  - �Ÿ�
        float radius = 0.1f;

        // 1. Ground�� �浹�ߴ��� ���ߴ��� �˷���
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
        

        // ����ڰ� Ű���带 ������, �յ� �Է°��� ���� Z��
        float vertical = Input.GetAxis("Vertical");
        // �����̴� ���� (x,y)
        // Vector3 dir = new Vector3(0f, 0f, vertical);
        Vector3 dir = transform.forward * vertical;
        // ����ڰ� Ű���带 ������, �¿� �Է°��� ����. X��
        float horizontal = Input.GetAxis("Horizontal");

       

        // ����, player�� ���� �ٴڿ� ��� �ִٸ� jump -> y��
        //if (cc.collisionFlags == CollisionFlags.Below)
        if (cc.isGrounded)
        {
           

            // ���� ���¿��� ���� ��ư ������
            if (Input.GetButtonDown("Jump"))
            {
                yVelocity = jumpPower;//���� �Ŀ���ŭ ����
           
            }

        }
        //  �߷� �� (y�� ���)
        yVelocity += gravity * Time.deltaTime;
        // vector.3 y���� �־���
        dir.y = yVelocity;

        // ���� Ű�� ������ ����, �̵��ϵ��� �Ѵ�.
        //  -> ���� ������ �� vertical �� ���� ũ�Ⱑ 0.1���� ũ��, Ű�� ���� ��ġ��
        // ĳ���� ��Ʈ�ѷ��̿��� x,y,z ������.
        cc.Move(dir * MoveSpeed * Time.deltaTime);
        

    }

    void RotateDirection()
    {
        // ����, �¿� ����Ű ������ ��
        if (Input.GetButtonDown("Horizontal"))
        {
            // ���� ��ư ������
            if (Input.GetAxisRaw("Horizontal") < 0)
            {
                // Y�� �������� ����(-����)���� 90�� ȸ��
                float rotateY = transform.eulerAngles.y - 90f;
                // ȸ���ؾ��� ������ ����
                transform.eulerAngles = new Vector3(0, rotateY, 0);

            }
            // ������ ��ư ������ 
            else
            {
                // Y�� �������� ����(-����)���� 90�� ȸ��
                float rotateY = transform.eulerAngles.y + 90f;
                //Y�� �������� ������(+����)���� 90�� ȸ��
                transform.eulerAngles = new Vector3(0, rotateY, 0);
            }
        }


        // ����ڰ� Ű���带 ������, �¿� �Է°��� ����. X��
        //float horizontal = Input.GetAxis("Horizontal");


        // �� ȸ�� ������
        //rotY += horizontal * (rotSpeed) * Time.deltaTime;

        // Y�� �������� ���� ȸ��
        //transform.eulerAngles = new Vector3(0, rotY, 0);
    }

}

