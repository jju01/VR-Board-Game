using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 역할 : 얼음이 그릇에 모두 충돌했을 때 게임 클리어

public class MiniGameIce : MonoBehaviour
{
    // 충돌한 물체 수
    public int count = 0;

    // 효과음
    private AudioSource audio;
    // 정지상태 
    private bool IsPause;

    [Space]
    // 게임종료 패널
    public GameObject gameOverPanel;
    public GameObject resultPanel;
    public GameObject itemPanel;

    // 싱글톤으로 만들어서 관리
    public static MiniGameIce Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        gameOverPanel.SetActive(false);
        resultPanel.SetActive(false);
        itemPanel.SetActive(false);
        IsPause = false;
        //audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // 얼음이 그릇에 충돌했을 때 함수
    private void OnTriggerEnter(Collider other)
    {
        Ice ice = other.GetComponent<Ice>();
        if (ice.isture)
        {
            ice.isture = false;
            count++;
        }

        // 점수 카운트 되는 함수 가져옴
        //FindObjectOfType<Ice>().OnTriggerEnter(other);

        // 얼음이 모두(10개) 충돌하면
        if (count == 3)
        {
            // 게임 종료!
            Debug.Log("게임종료!");
            // 게임 화면 일시정지
           // PauseGame();
            // UI 창 실행
            StartCoroutine(OnPanel());
        }
    }

    // 게임 종료 & 결과 UI 창 나오는 함수
    IEnumerator OnPanel()
    {
        yield return new WaitForSecondsRealtime(0.8f); // WaitForSecondsRealtime = timeScale 에 영향 x
        gameOverPanel.SetActive(true);
        yield return new WaitForSecondsRealtime(2f);
        gameOverPanel.SetActive(false);
        resultPanel.SetActive(true);
        yield return new WaitForSecondsRealtime(2.5f);
        resultPanel.SetActive(false);
        itemPanel.SetActive(true);
    }

    // 시간 멈춤 => 게임끝나면 동작일시정지하고 UI창만 나오게끔...
    void PauseGame()
    {
        if (IsPause == false)
        {
            Time.timeScale = 0;
            IsPause = true;
            return;
        }
    }



    // 아이템창 끄기
    public void OnClickItemClose()
    {
        itemPanel.SetActive(false);
        Debug.Log("메인씬으로 쭈고");
    }
}
