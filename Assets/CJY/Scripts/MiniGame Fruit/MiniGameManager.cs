using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class MiniGameManager : MonoBehaviour
{
    // 점수 
    public int score = 0;
    public TextMeshProUGUI scoreText;

    // 타이머 
    private float timer = 30.0f;
    public TextMeshProUGUI timerText;

    [Space]
    // 게임 패널
    public GameObject menuPanel;
    public GameObject gameOverPanel;
    public GameObject resultPanel;
    public GameObject itemPanel;
    public GameObject startPanel;

    // ready 체크 
    public bool isready = false;

    public bool isPlaying = false;

    private ExitGames.Client.Photon.Hashtable PlayerCustomProperties = new ExitGames.Client.Photon.Hashtable();
    private string miniGameFruitKey = "MiniGameFruit";
    private string miniGameFruitScore = "MiniGameFruitScore";
    float startTime = 0;


    // 싱글톤으로 만들어서 관리
    public static MiniGameManager Instance;

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
        timerText.text = timer.ToString();

        // 처음점수(=0) 저장
        SaveScore();
    }

    public void SetStartTime(float startTime)
    {
        this.startTime = startTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (startTime < 1)
        {
            return;
        }

        StartTimer();
    }

    // 점수 증가
    public void IncreaseScore(int amount)
    {
        score += amount;
        scoreText.text = score.ToString();
        SaveScore();
    }

    // 점수 감소
    public void DecreaseScore(int amount)
    {
        score -= amount;
        Debug.Log("점수 마이너스");
        scoreText.text = score.ToString();
        SaveScore();
    }

    // 타이머 함수 (30초 끝나면 종료)
    public void StartTimer()
    {
        if (isready == true)
        {
            float time = Time.realtimeSinceStartup - startTime;

            if (time < timer)
            {
                timerText.text = Mathf.Round(timer - time).ToString();
            }
            else
            {
                timerText.text = "0";
                // 타이머 동기화
                if (PlayerCustomProperties.ContainsKey(miniGameFruitKey))
                {
                    PlayerCustomProperties[miniGameFruitKey] = timer;
                }
                else
                {
                    PlayerCustomProperties.Add(miniGameFruitKey, timer);
                }

                PhotonNetwork.SetPlayerCustomProperties(PlayerCustomProperties);

                //Time.timeScale = 0;
                FruiteSpawner.Instance.StopFruit();
                isready = false;
                OnGameEnd();
            }
        }
    }
    void SaveScore()
    {
        // 점수 동기화
        if (PlayerCustomProperties.ContainsKey(miniGameFruitScore))
        {
            PlayerCustomProperties[miniGameFruitScore] = score;
        }
        else
        {
            PlayerCustomProperties.Add(miniGameFruitScore, score);
        }

        PhotonNetwork.SetPlayerCustomProperties(PlayerCustomProperties);
    }

    // 게임 종료 함수 
    public void OnGameEnd()
    {
        // 게임 종료!
        Debug.Log("게임종료!");

        if (isPlaying == false)
        {
            // UI 창 실행
            StartCoroutine(OnPanel());
        }
    }


    // 게임 종료 & 결과 UI 창 나오는 함수
    IEnumerator OnPanel()
    {
        isPlaying = true;

        print("실행");
        yield return new WaitForSecondsRealtime(1f); // WaitForSecondsRealtime = timeScale 에 영향 x
        gameOverPanel.SetActive(true);
        yield return new WaitForSecondsRealtime(2f);
        gameOverPanel.SetActive(false);
        resultPanel.SetActive(true);
        yield return new WaitForSecondsRealtime(5f);
        resultPanel.SetActive(false);
        itemPanel.SetActive(true);
        // timescale = 1 로 만들기 !
        yield return new WaitForSecondsRealtime(5f);
        //Time.timeScale = 1;
        itemPanel.SetActive(false);
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel("Main");
        }
    }

    //IEnumerator OnPanel()
    //{
    //    print("실행");
    //    yield return new WaitForSecondsRealtime(0.8f); // WaitForSecondsRealtime = timeScale 에 영향 x
    //    gameOverPanel.SetActive(true);
    //    yield return new WaitForSecondsRealtime(2f);
    //    gameOverPanel.SetActive(false);
    //    resultPanel.SetActive(true);
    //    yield return new WaitForSecondsRealtime(5f);
    //    resultPanel.SetActive(false);
    //    itemPanel.SetActive(true);
    ////    time Scale = 1로 만들기 !(수정버전)
    //   yield return new WaitForSecondsRealtime(5f);
    //    itemPanel.SetActive(false);
    //    Time.timeScale = 1;
    //    PhotonNetwork.LoadLevel("Main");
    //}
}
