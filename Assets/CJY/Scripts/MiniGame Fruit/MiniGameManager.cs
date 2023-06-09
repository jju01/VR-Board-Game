using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MiniGameManager : MonoBehaviour
{
    private int score = 0;
    public TextMeshProUGUI scoreText;

    private float timer = 30.0f;
    public TextMeshProUGUI timerText;

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
        
    }

    // Update is called once per frame
    void Update()
    {
        StartTimer();
    }

    public void IncreaseScore()
    {
        score++;
        scoreText.text = score.ToString();
    }

    public void StartTimer()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
            timerText.text = timer.ToString("00.00");
        }
        else
        {
            Debug.Log("게임종료");

        }
    }
}
