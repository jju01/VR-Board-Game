using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class VRManager : MonoBehaviour
{
    // VR Controller ��� ����
    public bool useVRController = false;
    // �̱��� �غ�
    public static VRManager Instance;

    private void Awake()
    {
        // ����, �� �ڽ�(=this)�� ����ִ� ���¶��
        if (Instance == null)
        {
            // Instance�� �� �ڽ��� �Ҵ��Ѵ�.
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

    // �ڵ� VR Controller ��뿩�� üũ
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
