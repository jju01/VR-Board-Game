using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���� : ���콺 �Է°��� ���� ī�޶� ȸ��

public class CameraRotate : MonoBehaviour
{
    // ȸ���ӵ�
    public float rotationSpeed = 5f;
    // ���� ȸ�� ���� ����
    public float limitAngle = 80.0f;
    // ���� ȸ����
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
        // ���콺 �Է°��� ����
        // - ��/��
        float vertical = Input.GetAxis("Mouse Y");
        // - ��/��
        float horizontal = Input.GetAxis("Mouse X");
        // rotateSpeed ��ŭ
        mouseX += horizontal * rotationSpeed * 100.0f * Time.deltaTime;
        mouseY += vertical * rotationSpeed * 100.0f * Time.deltaTime;
        // ���� ȸ�� �� ���� ����
        mouseY = Mathf.Clamp(mouseY, -limitAngle, limitAngle);

        // a. mouseX ȸ�� ���� 360���� Ŀ���� ��
        if (mouseX > 360f)
        {
            mouseX -= 360f;
        }
        // a. mouseX ȸ�� ���� 360���� �۾����� ��
        if (mouseX < 0f)
        {
            mouseX += 360f;
        }

        // ī�޶� ȸ��
        transform.localEulerAngles = new Vector3(mouseY, -mouseX, 0f);
    }
}
