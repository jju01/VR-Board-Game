using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using Hashtable = ExitGames.Client.Photon.Hashtable;


public class ScoreManager : MonoBehaviour
{
    public TextMeshProUGUI[] scoreText;
    public TextMeshProUGUI[] nameText;

    public List<int> scoreList;

    // Start is called before the first frame update
    void Start()
    {
        ShowScore();
    }

    // Update is called once per frame
    void Update()
    {

    }

    // 선택정렬하기
    public void ShowScore()
    {
        // 플레이어 수만큼 점수 판 생성
        //int i = 0;
        //foreach (Photon.Realtime.Player p in PhotonNetwork.CurrentRoom.Players.Values)
        //{
        //    if (p.CustomProperties.ContainsKey("MiniGameFruitScore"))
        //    {
        //        nameText[i].text = p.NickName;

        //        // scoreText.text = p.CustomProperties["MiniGameFruitScore"].ToString;
        //        if (p.CustomProperties.TryGetValue("MiniGameFruitScore", out object score))
        //        {
        //            scoreText[i].text = score.ToString();
        //        }
        //    }
        //    i++;
        //}

        scoreList = new List<int>();
        foreach (Photon.Realtime.Player p in PhotonNetwork.CurrentRoom.Players.Values)
        {
            if (p.CustomProperties.ContainsKey("MiniGameFruitScore"))
            {
                if (p.CustomProperties.TryGetValue("MiniGameFruitScore", out object score))
                {
                    scoreList.Add((int)score);
                }
                // scoreList
                // scoreList.Sort((a, b) => b.CompareTo(a));              
            }
        }
        for (int i = 0; i < scoreList.Count; i++)
        {
            int min = i;
            for (int j = i + 1; j < scoreList.Count; j++)
            {
                // 최소값 비교
                if (scoreList[min] < scoreList[j])
                {
                    min = j;
                }
            }
            // SWAP
            if (i != min)
            {
                int tmp = scoreList[min];
                scoreList[min] = scoreList[i];
                scoreList[i] = tmp;
            }
        }

        // 플레이어 수만큼 점수 판 생성
        foreach (Photon.Realtime.Player p in PhotonNetwork.CurrentRoom.Players.Values)
        {
            if (p.CustomProperties.ContainsKey("MiniGameFruitScore"))
            {
                int score = (int)p.CustomProperties["MiniGameFruitScore"];
                int idx = scoreList.IndexOf(score);

                Debug.Log($"score : {score}      idx : {idx}");

                nameText[idx].text = p.NickName;
                scoreText[idx].text = score.ToString();
                // 우승자 정보 저장
                if (idx == 0)
                {
                    if (p.IsLocal)
                    {
                        Hashtable CustomProperties = new Hashtable();

                        CustomProperties.Add($"winner1", p.NickName);

                        PhotonNetwork.SetPlayerCustomProperties(CustomProperties);
                        
                        GameManager.Instance.SetWinnerItem();
                    }
                }
            }
        }
    }
}

