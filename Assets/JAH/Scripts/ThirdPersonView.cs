using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// ��ŧ�� ��Ʈ�ѷ� Button one�� ������ 3��Ī ī�޷� �������� ����

public class ThirdPersonView : MonoBehaviour
{

    // 3��Ī ī�޶�
    [SerializeField] private Transform ThirdPesrsonView;


    private void Update()
    {

        // A. VR Controller ��� ����� ���
        if (GameManager.Instance.useVRController)
        {
            // ��Ʈ�ѷ� Button One�� ������ ��..
            if (OVRInput.Get(OVRInput.Button.One))
            {
                // 3��Ī ī�޶� ���� ��� �ٲ��            
                GameManager.Instance.SetThirdPersonView(ThirdPesrsonView);
            }
            // ��ư�� ���� ...
            if (OVRInput.GetUp(OVRInput.Button.One))
            {
                // �ٽ� OVRCameraview ��������
                GameManager.Instance.SetOVRCameraView();
            }

        }
    }
}
