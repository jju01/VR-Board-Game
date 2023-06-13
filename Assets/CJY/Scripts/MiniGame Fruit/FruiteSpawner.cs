using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruiteSpawner : MonoBehaviour
{
    // 싱글톤으로 관리
    public static FruiteSpawner Instance;

    // 스폰구역
    private Collider spawnArea;

    // 과일 list 
    public GameObject[] fruitPrefabs;

    // 폭탄
    public GameObject bombPrefabs;

    [Range(0f, 1f)]
    public float bombChance = 0.05f;

    // 과일 스폰 최소 최대 시간
    public float minSpawnDelay = 0.25f;
    public float maxSpawnDelay = 1f;

    // 과일 최소 최대 각도
    public float minAngle = -15f;
    public float maxAngle = 15f;

    // 과일 올라오는 최소 최대 힘
    public float minForce = 18f;
    public float maxForce = 22f;

    // 과일이 머무를 수 있는 최대 시간
    public float maxLifetime = 5f;

    private void Awake()
    {
        Instance = this;
        spawnArea = GetComponent<Collider>();
    }

    void Start()
    {
        //StartCoroutine(Spawn());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    public void StartFruit()
    {
        MiniGameManager.Instance.SetStartTime(Time.realtimeSinceStartup);
        StartCoroutine(Spawn());
    }

    public IEnumerator Spawn()
    {
        // 활성화되어있는 동안 반복
        while (enabled)
        {
            // 랜덤 과일 생성
            GameObject prefab = fruitPrefabs[Random.Range(0, fruitPrefabs.Length)];

            // 랜덤 폭탄 생성
            if (Random.value < bombChance)
            {
                prefab = bombPrefabs;
            }

            // 과일 스폰 위치 생성
            Vector3 position = new Vector3();
            position.x = Random.Range(spawnArea.bounds.min.x, spawnArea.bounds.max.x);
            position.y = Random.Range(spawnArea.bounds.min.y, spawnArea.bounds.max.y);
            position.z = Random.Range(spawnArea.bounds.min.z, spawnArea.bounds.max.z);

            // 과일 랜덤 각도
            Quaternion rotation = Quaternion.Euler(0f, 0f, Random.Range(minAngle, maxAngle));

            // 맥스라이프 시간 뒤 삭제
            GameObject fruit = Instantiate(prefab, position, rotation);
            Destroy(fruit, maxLifetime);

            // 랜덤 힘으로 올라옴
            float force = Random.Range(minForce, maxForce);
            fruit.GetComponent<Rigidbody>().AddForce(fruit.transform.up * force, ForceMode.Impulse);

            yield return new WaitForSeconds(Random.Range(minSpawnDelay, maxSpawnDelay));
        }
    }
}

