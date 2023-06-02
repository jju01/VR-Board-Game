using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 역할 : VR Player를 대신해서 움직일 수 있는 TestPlayer 움직임(이동/회전) 수행
// - 이동속도
// - 중력가속도
// - CharacterController Component
// 2. 회전 ( a. 좌우 , b. 위아래)
// - 회전 속도
// - 좌우 : Player 회전
//        : 마우스 좌/우 회전값
// - 위아래 : Camera 회전
//        : 마우스 상/하 회전값

// 1. CharacterController를 사용한 움직임
[RequireComponent(typeof(CharacterController))]


public class PlayerController : MonoBehaviour
{
    // - CharacterController Component
    private CharacterController cc;
    // - 이동속도
    public float moveSpeed = 3f;
    // - 중력가속도
    public float gravity = 9.81f;
    // - 회전 속도
    public float rotateSpeed = 3f;
    // - 위아래 : Camera 회전
    public Transform face;
    //  마우스 좌/우 회전값
    private float mouseX;
    //  마우스 상/하 회전값
    private float mouseY;


    // Start is called before the first frame update
    void Start()
    {
        // CharacterController 컴포넌트 가져온다
        cc = GetComponent<CharacterController>();
        // 마우스 커서 안보이게..
        InvisibleCusor();
    }

    // Update is called once per frame
    void Update()
    {
        // 가. Player를 이동시킨다
        Movement();
        // 나. Player를 좌우로 회전한다
        HorizontalRotate();
        // 다. Player의 고개를 위아래로 회전한다
        VerticalRotate();
    }

    // 가. Player를 이동시킨다
    private void Movement()
    {
        // 3. 사용자의 입력에 따른 좌/우/앞/뒤 방향 구함
        //  a. 좌우
        float horizontal = Input.GetAxis("Horizontal");
        //  b. 앞뒤
        float vertical = Input.GetAxis("Vertical");

        // 2. Player가 이동할 방향
        // + World -> Local 즉..Player가 바라보는 방향으로
        Vector3 direction = transform.forward * vertical + transform.right * horizontal;
        

        // + World -> Local 즉...카메라가 바라보는 방향으로
        //direction = Camera.main.transform.TransformDirection(direction);

        //  + Gravity Y 값에 적용
        direction.y -= gravity;
        // + 땅에 붙어있는 경우에는 중력 0 초기화
        if(cc.isGrounded)
        {
            direction.y = 0f;
        }
        // 1. Player를 moveSpeed만큼의 속력으로 CC를 통해 움직인다
        // 방향 * 속력 * Time.deltaTime
        cc.Move(direction * moveSpeed * Time.deltaTime);
    }

    // 나. Player를 좌우로 회전한다
    private void HorizontalRotate()
    {
        // 4. Mouse 움직임에 따른 회전 값 가져오기
        // - 좌/우 움직임
        float horizontal = Input.GetAxis("Mouse X");
        // - 움직임에 의해 회전한 Y값
        mouseX += horizontal * rotateSpeed * 100f * Time.deltaTime;
        // 3. Player의 현재 회전 각도 가져오기(Euler)
        Vector3 myAngle = transform.localEulerAngles;
        // 2. Y축 회전 값 갱신
        myAngle.y = mouseX;
        // 1. Player를 RotateSpeed 만큼의 속도로 회전
        transform.eulerAngles = myAngle;
    }

    // 다. Player의 face를 위아래로 회전한다
    private void VerticalRotate()
    {

        // 4. Mouse 움직임에 따른 입력값
        // - 상/하 입력
        float vertical = Input.GetAxis("Mouse Y");
        // - 상/하 움직임에 대한 X축 값 누적( 방향이 반대 => -1 곱하기 )
        mouseY += vertical * -1 * rotateSpeed * 100f * Time.deltaTime;
        // + 상/하 움직임 제한
       mouseY = Mathf.Clamp(mouseY, -60.0f, 80.0f);
        // 3. Face의 회전값을 가져온다
        Vector3 myAngle = face.localEulerAngles;
        // 2. X축 회전 값 갱신
        myAngle.x = mouseY;
        // 1. 변경한 회전 값을 Face에 적용
        face.localEulerAngles = myAngle;


    }

    // 마우스 고정& 안 보이기
    public void InvisibleCusor()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}
