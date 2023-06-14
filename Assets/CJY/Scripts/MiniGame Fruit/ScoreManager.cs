using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;


public class ScoreManager : MonoBehaviour
{
    public TextMeshProUGUI[] scoreText;
    public TextMeshProUGUI[] nameText;


    // Start is called before the first frame update
    void Start()
    {
        ShowScore();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ShowScore()
    {
        // 플레이어 수만큼 점수 판 생성
        int i = 0;
        foreach (Photon.Realtime.Player p in PhotonNetwork.CurrentRoom.Players.Values)
        {
            if (p.CustomProperties.ContainsKey("MiniGameFruitScore"))
            {
                nameText[i].text = p.NickName;

                // scoreText.text = p.CustomProperties["MiniGameFruitScore"].ToString;
                if (p.CustomProperties.TryGetValue("MiniGameFruitScore", out object score))
                {
                    scoreText[i].text = score.ToString();
                }
            }
            i++;
        }
    }

}

