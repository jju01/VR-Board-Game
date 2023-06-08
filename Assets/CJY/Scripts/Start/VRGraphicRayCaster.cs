using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

// 역할 :  카메라를 통해 World Canvas UI를 선택할수 있도록 해준다.
//         Graphic RayCaster를 발사한다.
// - Graphic Raycaster (Canvas에 붙어있다)
// - UI 충돌체를 담을 그릇
// - Canvas 상의 가상의 마우스 포인터 위치 및 정보
// - 가상의 마우스 커서 역할을 대신할 카메라
// - LineRenderer

// 역할 : Ray 끝나는 지점(Line)에 CrossHair 조준점 가져다 둔다.
// - CrossHair
// - 크기가 일정할 수 있도록 비율 (원래크기 + 거리 * 보정값)
//     -> 보정값 
//     -> 원래 크기

// + 별도의 라인을 사용하여 주석처리 하였음.



public class VRGraphicRayCaster : BaseInputModule
{
    // - Graphic Raycaster (Canvas에 붙어있다)
    public GraphicRaycaster graphicRaycaster;
    // - UI 충돌체를 담을 그릇
    public List<RaycastResult> raycastResults;
    // - Canvas 상의 가상의 마우스 포인터 위치 및 정보
    private PointerEventData pointerEventData;
    // - 가상의 마우스 커서 역할을 대신할 카메라
    public Camera cam;
    // - LineRenderer
    private LineRenderer lr;
    // - LR그릴 선의 길이
    public float lineDis = 1.0f;
    [Space]
    // - CrossHair
    public Transform crossHair;
    //     -> 보정값 
    public float adjustSize = 0.5f;
    //     -> 원래 크기
    private Vector3 originSize;

    // Start is called before the first frame update
    protected override void Start()
    {

        // pointerEventData 초기화
        pointerEventData = new PointerEventData(null);
        // pointerEventData 가상의 마우스 포인터 위치를 화면 정중앙으로
        pointerEventData.position = new Vector2(cam.pixelWidth * 0.5f, cam.pixelHeight * 0.5f);
        // UI 충돌체를 담을 그릇 리스트 초기화
        raycastResults = new List<RaycastResult>();
        // LR 초기화 : 카메라의 부모에 붙어있는(Hand) 에게 붙어있는 LineRenderer 가져와 
        lr = cam.GetComponentInParent<LineRenderer>();

        // crossHair 의 원래 사이즈 저장 
        originSize = crossHair.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        GraphicRayCast();
    }

    private void GraphicRayCast()
    {

        // pointerEventData 가상의 마우스 포인터 위치를 화면 정중앙으로
        pointerEventData.position = new Vector2(cam.pixelWidth * 0.5f, cam.pixelHeight * 0.5f);


        //1. Ray를 만듦과 동시에 충돌시키고, 충돌한 정보를 리스트에 담는다.
        // - Ray를 어디에서 쏠 지(특정 캔버스)
        // - Ray를 어디에 쏠 지 (가상의 마우스 위치 = pointerEventData.position)
        // - Ray를 쏴서 부딪힌 정보를 담을 그릇 (raycastResults)
        graphicRaycaster.Raycast(pointerEventData, raycastResults);

        // 2. 충돌한 물체(UI)가 있다면?
        if (raycastResults.Count > 0)
        {
            //   a. 충돌한 물체를 모두 탐색한다.
            for (int i = 0; i < raycastResults.Count; i++)
            {
                //   b. 각 충돌 물체에게 Mouse Hovering 이벤트 전달
                Debug.Log(raycastResults[i].gameObject.name);
                HandlePointerExitAndEnter(pointerEventData, raycastResults[i].gameObject);
                //   c. 충돌한 상태에서 특정 Input 버튼을 누르면 
                if (Input.GetKeyDown(KeyCode.Space) || OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch))
                {                    
                    //     - 해당 UI에 클릭 이벤트를 전달한다.
                    ExecuteEvents.Execute(
                        raycastResults[i].gameObject,   // Event 전달할 물체
                        new BaseEventData(eventSystem), // Event 관리하는 관리자
                        ExecuteEvents.submitHandler     // 해당 Event 동작(실행)
                        );

                    if (raycastResults[i].gameObject.GetComponent<Button>())
                        return;
                }
            }
            //   d. 충돌한 곳까지 LR 그려주기
            //DrawLine(raycastResults[0].distance);

            //   e. 충돌한 위치에 crossHair를 가져다 둔다.
            //SetCrossHairPosition(raycastResults[0].distance);

        }
        // 3. 충돌한 물체(UI)가 없다면?
        else
        {
            //  a. Mouse Hovering이벤트 리셋. (= 호버링 벗어남)
            HandlePointerExitAndEnter(pointerEventData, null);
            //  b. 기본 Ray 길이만큼 LR 그려주기.
            // DrawLine(lineDis);
            //  c. Line이 끝나는 위치에 crossHair를 가져다 둔다.
            // SetCrossHairPosition(lineDis);
        }

        // 4. 충돌한 정보를 모아놓은 리스트 Reset 해준다.
        raycastResults.Clear();


    }

    private void SetCrossHairPosition(float distance)
    {
        //    위치 : cam의 위치 + cam의 향하는 방향 * 충돌한 물체와 떨어진 거리 or 라인이 끝나는 거리
        Vector3 crossHairPos = cam.transform.position + cam.transform.forward * distance;
        crossHair.position = crossHairPos;
        //    크기 : 원래 크기 * 거리* 보정값
        float dis = Vector3.Distance(crossHairPos, Camera.main.transform.position);
        crossHair.localScale = originSize * dis * adjustSize;
        //    방향 : 항상 나(메인 카메라)를 바라보고 있도록
        Vector3 dir = crossHairPos - Camera.main.transform.position;
        crossHair.forward = dir.normalized;
    }


    //라인 렌더러그리는 함수
    private void DrawLine(float distance)
    {
        // 라인의 첫번째 점 위치 설정(시작 포인트) : 내위치
        lr.SetPosition(0, cam.transform.position);
        // 라인의 두번째 점 위치 설정(끝 포인트) : 내위치 + 내 방향 *거리(distance)
        lr.SetPosition(1, cam.transform.position + cam.transform.forward * distance);
    }


    #region BaseInputModule 상속받아서 필수적으로 선언해야하는 함수
    public override void Process() { }
    #endregion
}
