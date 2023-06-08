using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 역할 : 마우스 입력값에 따라 카메라 회전

public class CameraRotate : MonoBehaviour
{
    // 회전속도
    public float rotationSpeed = 5f;
    // 상하 회전 제한 각도
    public float limitAngle = 80.0f;
    // 누적 회전값
    [SerializeField]
    private float mouseX;
    private float mouseY;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButton(0))
        {
            MouseRotate();
        }
    }

    private void MouseRotate()
    {
        // 마우스 입력값에 따라
        // - 상/하
        float vertical = Input.GetAxis("Mouse Y");
        // - 좌/우
        float horizontal = Input.GetAxis("Mouse X");
        // rotateSpeed 만큼
        mouseX += horizontal * rotationSpeed * 100.0f * Time.deltaTime;
        mouseY += vertical * rotationSpeed * 100.0f * Time.deltaTime;
        // 상하 회전 시 각도 제한
        mouseY = Mathf.Clamp(mouseY, -limitAngle, limitAngle);

        // a. mouseX 회전 값이 360보다 커졌을 때
        if (mouseX > 360f)
        {
            mouseX -= 360f;
        }
        // a. mouseX 회전 값이 360보다 작아졌을 때
        if (mouseX < 0f)
        {
            mouseX += 360f;
        }

        // 카메라 회전
        transform.localEulerAngles = new Vector3(mouseY, -mouseX, 0f);
    }
}
