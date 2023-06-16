using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class VRManager : MonoBehaviour
{
    // VR Controller 사용 여부
    public bool useVRController = false;
    // 싱글톤 준비
    public static VRManager Instance;

    private void Awake()
    {
        // 만일, 나 자신(=this)이 비어있는 상태라면
        if (Instance == null)
        {
            // Instance에 나 자신을 할당한다.
            Instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        AutoControllerSetting();
    }

    // Update is called once per frame
    void Update()
    {

    }

    // 자동 VR Controller 사용여부 체크
    private void AutoControllerSetting()
    {
        if (GameObject.Find("Player"))
        {
            useVRController = false;
        }
        if (GameObject.Find("OVRCameraRig"))
        {
            useVRController = true;
        }
    }
}
