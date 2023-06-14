using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class MiniGameManager : MonoBehaviour
{
    // ���� 
    public int score = 0;
    public TextMeshProUGUI scoreText;

    // Ÿ�̸� 
    private float timer = 30.0f;
    public TextMeshProUGUI timerText;

    [Space]
    // �������� �г�
    public GameObject gameOverPanel;
    public GameObject resultPanel;
    public GameObject itemPanel;

    // ready üũ 
    public bool isready = false;

    private ExitGames.Client.Photon.Hashtable PlayerCustomProperties = new ExitGames.Client.Photon.Hashtable();
    private string miniGameFruitKey = "MiniGameFruit";
    private string miniGameFruitScore = "MiniGameFruitScore";
    float startTime = 0;


    // �̱������� ���� ����
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
        gameOverPanel.SetActive(false);
        resultPanel.SetActive(false);
        itemPanel.SetActive(false);
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

    // ���� ����
    public void IncreaseScore(int amount)
    {
        score += amount;
        scoreText.text = score.ToString();
    }

    // ���� ����
    public void DecreaseScore(int amount)
    {
        score -= amount;
        Debug.Log("���� ���̳ʽ�");
        scoreText.text = score.ToString();
    }

    // Ÿ�̸� �Լ� (30�� ������ ����)
    public void StartTimer()
    {
        if (isready == true)
        {
            float time = Time.realtimeSinceStartup - startTime;
            time = time / 1000;
            if (time < timer)
            {
                //timer -= Time.deltaTime;
                timerText.text = $"{timer-time}";
            }
            else
            {
                if (PlayerCustomProperties.ContainsKey(miniGameFruitKey))
                {
                    PlayerCustomProperties[miniGameFruitKey] = true;
                }
                else
                {
                    PlayerCustomProperties.Add(miniGameFruitKey, timer);
                }

                if (PlayerCustomProperties.ContainsKey(miniGameFruitScore))
                {
                    PlayerCustomProperties[miniGameFruitScore] = true;
                }
                else
                {
                    PlayerCustomProperties.Add(miniGameFruitScore, score);
                }

                PhotonNetwork.SetPlayerCustomProperties(PlayerCustomProperties);

                Time.timeScale = 0;
                OnGameEnd();
            }
            timerText.text = Mathf.Round(timer).ToString();
        }
    }

    // ���� ���� �Լ� 
    public void OnGameEnd()
    {
        // ���� ����!
        Debug.Log("��������!");
        // UI â ����
        StartCoroutine(OnPanel());
    }


    // ���� ���� & ��� UI â ������ �Լ�
    IEnumerator OnPanel()
    {
        yield return new WaitForSecondsRealtime(0.8f); // WaitForSecondsRealtime = timeScale �� ���� x
        gameOverPanel.SetActive(true);
        yield return new WaitForSecondsRealtime(2f);
        gameOverPanel.SetActive(false);
        resultPanel.SetActive(true);
        yield return new WaitForSecondsRealtime(2.5f);
        resultPanel.SetActive(false);
        itemPanel.SetActive(true);
    }
}
