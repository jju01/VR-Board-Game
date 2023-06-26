using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class MiniGameItemUI : MonoBehaviour
{
    //public Button btnBack;
    
    public Photon.Realtime.Player[] players;

    public RectTransform rect;

    public GameObject uiSelectPlayer;
    public List<Button> btnPlayers;
    public List<TMP_Text> txtPlayers;
    
    public GameObject uiSelectItem;
    public List<Button> btnItems;
    public List<TMP_Text> txtCounts;
    
    public GameObject uiNotice;
    public TMP_Text winnerName;
    public TMP_Text targetName;
    
    private void Start()
    {
        //btnBack.onClick.AddListener(BackToSelectPlayer);
    }

    public void OpenUI()
    {
        if (HasItemSomeone() == false)
        {
            Hashtable hs = new Hashtable();
            hs.Add("winItemOwner", null);
            PhotonNetwork.CurrentRoom.SetCustomProperties(hs);
            
            return;
        }

        SetPlayerInfo();
        
        uiSelectPlayer.SetActive(true);
        uiSelectItem.SetActive(false);
        uiNotice.SetActive(false);
    }

    private void SetPlayerInfo()
    {
        
        players = PhotonNetwork.PlayerListOthers;
        
        for (int i = 0; i < btnPlayers.Count; i++)
        {
            int idx = i;
            if (players.Length > i)
            {
                btnPlayers[i].onClick.AddListener(() => { SetItemInfo(idx); });
                txtPlayers[i].text = players[i].NickName;
            }
            else
            {
                btnPlayers[i].onClick.RemoveAllListeners();
                txtPlayers[i].text = "";
            }
        }
    }

    private void SetItemInfo(int playerIdx)
    {
        uiSelectPlayer.SetActive(false);
        uiSelectItem.SetActive(true);
        
        Photon.Realtime.Player player = players[playerIdx];

        for (int i = 0; i < btnItems.Count; i++)
        {
            string itemType = $"itemType_{i}";
            
            int count = 0;
            if (player.CustomProperties.ContainsKey(itemType))
                count = (int)player.CustomProperties[itemType];

            if(count == 0)
            {
                txtCounts[i].text = $"X";
                btnItems[i].onClick.RemoveAllListeners();
                continue;
            }
            txtCounts[i].text = $"0";
            btnItems[i].onClick.AddListener(call: () => { SelectDone(player, itemType); });

        }
    }

    private void BackToSelectPlayer()
    {
        uiSelectPlayer.SetActive(true);
        uiSelectItem.SetActive(false);
    }

    private void SelectDone(Photon.Realtime.Player player, string item)
    {
        uiSelectItem.SetActive(false);

        Hashtable hs = new Hashtable();
        hs.Add("winItemOwner", null);
        hs.Add("winPlayer", PhotonNetwork.LocalPlayer);
        hs.Add("targetPlayer", player);
        hs.Add("item", item);

        PhotonNetwork.CurrentRoom.SetCustomProperties(hs);
    }

    public void NoticeItemState(string winnerPlayer, string targetPlayer, string item)
    {
        winnerName.text = winnerPlayer;
        targetName.text = targetPlayer;
        uiNotice.SetActive(true);
        
        LayoutRebuilder.ForceRebuildLayoutImmediate(rect);
        Invoke("NoticeDone", 3);
    }

    void NoticeDone()
    {
        uiNotice.SetActive(false);
    }

    public bool HasItemSomeone()
    {
        for (int i = 0; i < PhotonNetwork.PlayerListOthers.Length; i++)
        {
            for (int x = 0; x < 4; x++)
            {
                string itemType = $"itemType_{x}";
                if (PhotonNetwork.PlayerListOthers[i].CustomProperties.ContainsKey(itemType))
                    return true;
            }   
        }

        return false;
    }
}
