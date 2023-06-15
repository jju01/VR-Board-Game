using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

// 역할 : Hand에 붙어있는 Camera를 통해 WorldCanvasUI를 선택할 수있도록 해주는 Graphic Raycaster를 발사한다.
//  - Graphic Raycaster (*Canvas에 붙어있다)
//  - UI 충돌체를 담을 그릇 (*한번에 여러개가 충돌한다)
//  - Canvas 상의 가상의 마우스 포인터 위치 밎 정보
//  - 가상의 마우스 커서 역할을 대신할 Camera
//  - LineRenderer

// 역할 : Ray 끝나는 지점(Line)에 CrossHair 조준점 가져다 둔다.
//  - CrossHair
//  - 크기가 일정할 수 있도록 비율 ( 원래크기 + 거리 * 보정값)
//   > 보정값
//   > 원래 크기
public class GraphicCast : BaseInputModule
{
    // 싱글톤으로 관리
    public static GraphicCast Instance;

    //  - Graphic Raycaster (*Canvas에 붙어있다)
    public GraphicRaycaster graphicRaycaster;
    //  - UI 충돌체를 담을 그릇 (*한번에 여러개가 충돌한다)
    public List<RaycastResult> raycastResults;
    //  - Canvas 상의 가상의 마우스 포인터 위치 밎 정보
    private PointerEventData pointerEventData;
    //  - 가상의 마우스 커서 역할을 대신할 Camera
    public Camera cam;
    //  - LineRenderer
    private LineRenderer lr;
    [Space]
    //  - LR을 그릴 선의 길이
    public float lineDistance = 50.0f;
    public float lineWidth = 0.5f;

    public Color startColor = Color.white;
    public Color endColor = Color.white;
    [Space]
    //  - CrossHair마커
    public Transform marker;
    //   > 보정값
    public float adjustSize = 0.1f;
    //   > 원래 크기
    private Vector3 originSize;

    protected override void Awake()
    {
        if (Instance == null) Instance = this;
    }
    protected override void Start()
    {
        // pointerEventData 초기화
        pointerEventData = new PointerEventData(null);
        // pointerEnvetData : 가상의 마우스 포인터 위치 화면 정중앙으로 설정
        pointerEventData.position = new Vector2(cam.pixelWidth * 0.5f, cam.pixelHeight * 0.5f);
        // UI 충돌체를 담을 그릇 리스트를 초기화
        raycastResults = new List<RaycastResult>();
        // LR 초기화 : UI Camera에게 붙어있는 LR
        lr = cam.GetComponent<LineRenderer>();

        lr.enabled = false;
        // 마커 활성화
        marker.gameObject.SetActive(true);


        // CrossHair의 원래 크기 저장
        originSize = marker.localScale;
    }

    void Update()
    {
        GraphicRaycast();
    }

  
    private void GraphicRaycast()
    {

        //Ray ray = new Ray(physicsRay.position, physicsRay.forward);
        // pointerEnvetData : 가상의 마우스 포인터 위치 화면 정중앙으로 설정
        pointerEventData.position = new Vector2(cam.pixelWidth * 0.5f, cam.pixelHeight * 0.5f);

        // 1. Ray를 만듦과 동시에 충돌시키고, 충돌한 정보를 리스트에 담는다.
        //  - Ray를 어디에서 쏠 지 (특정 Canvas)
        //  - Ray를 어디에 쏠 지 (가상의 마우스 위치 = pointerEventData.position)
        //  - Ray를 쏴서 부딪힌 정보를 담을 그릇 (= raycastResult List)        
        graphicRaycaster.Raycast(pointerEventData, raycastResults);

        // 2. 충돌한 물체(UI)가 있다면?
        if (raycastResults.Count > 0)
        {
            //  a. 충돌한 물체(UI)를 모두 탐색한다
            for (int i = 0; i < raycastResults.Count; i++)
            {
                //  b. 각 충돌 물체(UI)에게 Mouse Hovering 이벤트 전달
                HandlePointerExitAndEnter(pointerEventData, raycastResults[i].gameObject);
                //  c. 충돌한 상태에서 Input 버튼을 누르면
                if (Input.GetKeyDown(KeyCode.Space) || OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch) || OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.LTouch))
                {
                    //   - 해당 UI에 '너 클릭 됐어' 라고 이벤트 전달한다.
                    ExecuteEvents.Execute(raycastResults[i].gameObject, new BaseEventData(eventSystem), ExecuteEvents.submitHandler);
                    // 가상의 마우스 클릭(코드구현)   Event 전달할 물체,     Event를 관리하는 관리자,       해당 Event 동작(실행)

                    print("선택");
                }
            }

            //  d. 층돌한 곳까지 LR 그려주기
            DrawLine(raycastResults[0].distance); // 첫번째 충돌한 곳까지의 거리
            // e. 충돌한 위치에 CrossHair를 가져다둔다.
            SetCrossHiarPosition(raycastResults[0].distance);
        }

        // 3. 충돌한 물체(UI)가 없다면?
        else
        {
            //  a. Mouse Hovering 이벤트 Reset (호버링 벗어남)
            HandlePointerExitAndEnter(pointerEventData, null);
            //  b. 기본 Ray 길이만큼 LR 그려주기
            DrawLine(0); // 기본길이

            // c. Line이 끝나는 위치에 CrossHair를 가져다둔다.
            SetCrossHiarPosition(0);


        }

        // 4. 충돌한 정보를 모아놓은 리스트 Reset 해준다.(없다면 계속 쌓일 것)
        raycastResults.Clear(); // = RemoveAll
    }

    private void SetCrossHiarPosition(float distance)
    {
        marker.gameObject.SetActive(true);

        // 위치 : cam의 위치 + cam이 향하는 방향 * 충돌한 물체와 떨어진 거리 or 라인이 끝나는 거리
        Vector3 crossHairPos = cam.transform.position + cam.transform.forward * distance;
        marker.position = crossHairPos;
        // 크기 : 원래크기 * 거리 * 보정값
        float dis = Vector3.Distance(crossHairPos, Camera.main.transform.position);
        marker.localScale = originSize * dis * adjustSize;
        // 방향 : 항상 나(Main Cam)를 바라보고 있어야한다.
        Vector3 dir = crossHairPos - Camera.main.transform.position;
        marker.forward = dir.normalized;

    }

    private void DrawLine(float distance)
    {
        // Line의 1번째 점 위치 설정(시작 Point) : 내 위치(카메라 위치)
        lr.SetPosition(0, cam.transform.position);
        // Line의 2번째 점 위치 설정 (끝 Point)  : 내 위치 + 바라보는 방향 * 거리
        lr.SetPosition(1, cam.transform.position + cam.transform.forward * distance);
    }


    #region // BaseInputModule을 상속 받았기 때문에, 필수적으로 선언해야하는 함수 리스트
    public override void Process() { }
    #endregion

}
