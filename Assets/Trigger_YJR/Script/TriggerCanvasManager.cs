using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// UI Canvas 관리

public class TriggerCanvasManager : MonoBehaviour
{

    // trigger cavas UI 
    public GameObject triggerCanvas;
    // ui canvas y 값 세팅
    public float UIyVectorSetting = 5f;

    // TriggerUIcanvas를 킨다. 
    public void UiCanvasON(Vector3 pos)
    {
        // uiCanvas y값 세팅
        pos.y += UIyVectorSetting;
        // UI Canvas에 외부 위치 값 할당
        triggerCanvas.transform.position = pos;

        // UICanvas 비활성화 활성화
        triggerCanvas.gameObject.SetActive(false);
        triggerCanvas.gameObject.SetActive(true);

    }
    public void UiCanvasOFF()
    {
        triggerCanvas.gameObject.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
