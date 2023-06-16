using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 오큘러 컨트롤러 Button one을 누르면 3인칭 카메로 시점으로 본다

public class ThirdPersonView : MonoBehaviour
{

    // 3인칭 카메라
    [SerializeField] private Transform ThirdPesrsonView;


    private void Update()
    {

        // A. VR Controller 사용 모드인 경우
        if (GameManager.Instance.useVRController)
        {
            // 컨트롤러 Button One을 눌렀을 때..
            if (OVRInput.Get(OVRInput.Button.One))
            {
                // 3인칭 카메라 시점 뷰로 바뀐다            
                GameManager.Instance.SetThirdPersonView(ThirdPesrsonView);
            }
            // 버튼을 떼면 ...
            if (OVRInput.GetUp(OVRInput.Button.One))
            {
                // 다시 OVRCameraview 시점으로
                GameManager.Instance.SetOVRCameraView();
            }

        }
    }
}
