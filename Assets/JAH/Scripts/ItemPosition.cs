using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using Random = UnityEngine.Random;
using Hashtable = ExitGames.Client.Photon.Hashtable;

// 역할 : Item(trigger 포함)을 SpawnPositions에 랜덤 배치한다
public class ItemPosition : MonoBehaviour
{
    public static ItemPosition Instance;

    // Item 리스트
    public List<GameObject> items;
    // Item 리스트
    public List<GameObject> triggers;
    
    // Gitem이 생성될 위치 리스트
    public List<Transform> spawnPositions;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        // 존재하는 GItem 모두 가져온다. 
        GetAllGItem();
        // 존재하는 Trigger 모두 가져온다. 
        GetAllTrigger();
        // 생성될 위치 모두 가져온다.
        //GetAllSpawnPos();

        // 랜덤한 위치에 아이템 생성
        //itemPosition();
    }

    // 존재하는 GItem 모두 가져온다. 
    private void GetAllGItem()
    {
        // 0. 리스트 초기화
        items = new List<GameObject>();
        // 1. 내 자식 중에 Gitem을 모아둔 "Items" 오브젝트를 찾는다.
        GameObject itemsObj = transform.Find("Items").gameObject;
        // 2. itemsObj의 자식 개수를 불러온다.
        int childLength = itemsObj.transform.childCount;
        // 3. 자식 개수만큼 반복해서, 리스트에 추가한다.
        for (int i = 0; i < childLength; i++)
        {
            // 2. Items 오브젝트에 자식으로 모아둔 GItem 을 가져와서 
            GameObject childItem = itemsObj.transform.GetChild(i).gameObject;
            // 3. Item 리스트에 각각 넣어준다.
            items.Add(childItem);
            childItem.SetActive(false);
        } 
    }
    
    // 존재하는 Trigger 모두 가져온다. 
    private void GetAllTrigger()
    {
        // 0. 리스트 초기화
        triggers = new List<GameObject>();
        // 1. 내 자식 중에 Trigger을 모아둔 "Triggers" 오브젝트를 찾는다.
        GameObject triggerObj = transform.Find("Triggers").gameObject;
        // 2. triggerObj 자식 개수를 불러온다.
        int childLength = triggerObj.transform.childCount;
        // 3. 자식 개수만큼 반복해서, 리스트에 추가한다.
        for (int i = 0; i < childLength; i++)
        {
            // 2. Triggers 오브젝트에 자식으로 모아둔 Trigger 을 가져와서 
            GameObject childItem = triggerObj.transform.GetChild(i).gameObject;
            // 3. Triggers 리스트에 각각 넣어준다.
            triggers.Add(childItem);
            childItem.SetActive(false);
        } 
    }

        // 생성될 위치 모두 가져온다.
    private void GetAllSpawnPos()
        {
            // 0. 리스트 초기화
            spawnPositions = new List<Transform>();
            // 1. 내 자식 중에 SpawnPositions 을 모아둔 "SpawnPositions" 오브젝트를 찾는다.
              GameObject spawnObj = transform.Find("SpawnPositions").gameObject;
            // 2. Spawn PositionsObj의 자식 개수를 불러온다.
            int childLength = spawnObj.transform.childCount;
            // 3. 자식 개수만큼 반복해서, 리스트에 추가한다.
            for (int i = 0; i < childLength; i++)
            {
                 // 2. SpawnPositions 오브젝트의 자식오브젝트들을  가져와서 
                 Transform childItem = spawnObj.transform.GetChild(i).transform;
                // 3. spawnPositions 리스트에 각각 넣어준다.
                 spawnPositions.Add(childItem);
            }
    }
    // Item들을 랜덤하게 생성 위치들에 넣어준다.
    private void itemPosition()
    {
        // 1. 내가 가진 아이템 개수 만큼 반복한다.
        for (int i = 0; i < items.Count; i++)
        {
            // 2. 랜덤으로 n번째 생성위치에 놓을지 번호를 뽑는다.
            // ( 최소 : 0 ~ 최대 : SpawnPositions.Count(리스트의 개수)
            int randomPosIndex = Random.Range(0, spawnPositions.Count);
            // 3. 그 생성 위치에 첫번째 Item부터 위치시킨다.
            items[i].transform.position = spawnPositions[randomPosIndex].position;
            // 4. 생성한 위치는 또다시 사용될 수 없도록 리스트에서 제거한다.
            spawnPositions.RemoveAt(randomPosIndex);
        }
    }


    public void SetItemFromNetwork()
    {
        Hashtable roomProps = PhotonNetwork.CurrentRoom.CustomProperties;
        List<IceCube> cubes = StageManager.Instance.iceCubes;
        
        if (roomProps.ContainsKey("itemSetting"))
        {
            int[] itemSettings = (int[])roomProps["itemSetting"];
            for (int i = 0; i < itemSettings.Length; i++)
            {
                int itemPosIdx = itemSettings[i];

                if (itemPosIdx < 0)
                {
                    items[i].gameObject.SetActive(false);
                    continue;
                }

                Vector3 itemPos = cubes[itemPosIdx].transform.position;
                itemPos.y = items[i].transform.position.y;

                items[i].transform.position = itemPos;
                items[i].gameObject.SetActive(true);
            }
        }
    }
    
    public void SetTriggerFromNetwork()
    {
        Hashtable roomProps = PhotonNetwork.CurrentRoom.CustomProperties;
        List<IceCube> cubes = StageManager.Instance.iceCubes;
        
        if (roomProps.ContainsKey("triggerSetting"))
        {
            int[] triggerSettings = (int[])roomProps["triggerSetting"];
            for (int i = 0; i < triggerSettings.Length; i++)
            {
                int triggerPosIdx = triggerSettings[i];
                Vector3 triggerPos = cubes[triggerPosIdx].transform.position;
                triggerPos.y = triggers[i].transform.position.y;

                triggers[i].transform.position = triggerPos;
                triggers[i].gameObject.SetActive(true);
            }
        }
    }


}

