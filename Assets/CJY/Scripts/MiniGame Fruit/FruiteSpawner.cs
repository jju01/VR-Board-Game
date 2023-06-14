using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruiteSpawner : MonoBehaviour
{
    // �̱������� ����
    public static FruiteSpawner Instance;

    // ��������
    private Collider spawnArea;

    // ���� list 
    public GameObject[] fruitPrefabs;

    // ��ź
    public GameObject bombPrefabs;

    [Range(0f, 1f)]
    public float bombChance = 0.05f;

    // ���� ���� �ּ� �ִ� �ð�
    public float minSpawnDelay = 0.25f;
    public float maxSpawnDelay = 1f;

    // ���� �ּ� �ִ� ����
    public float minAngle = -15f;
    public float maxAngle = 15f;

    // ���� �ö���� �ּ� �ִ� ��
    public float minForce = 18f;
    public float maxForce = 22f;

    // ������ �ӹ��� �� �ִ� �ִ� �ð�
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
        // Ȱ��ȭ�Ǿ��ִ� ���� �ݺ�
        while (enabled)
        {
            // ���� ���� ����
            GameObject prefab = fruitPrefabs[Random.Range(0, fruitPrefabs.Length)];

            // ���� ��ź ����
            if (Random.value < bombChance)
            {
                prefab = bombPrefabs;
            }

            // ���� ���� ��ġ ����
            Vector3 position = new Vector3();
            position.x = Random.Range(spawnArea.bounds.min.x, spawnArea.bounds.max.x);
            position.y = Random.Range(spawnArea.bounds.min.y, spawnArea.bounds.max.y);
            position.z = Random.Range(spawnArea.bounds.min.z, spawnArea.bounds.max.z);

            // ���� ���� ����
            Quaternion rotation = Quaternion.Euler(0f, 0f, Random.Range(minAngle, maxAngle));

            // �ƽ������� �ð� �� ����
            GameObject fruit = Instantiate(prefab, position, rotation);
            Destroy(fruit, maxLifetime);

            // ���� ������ �ö��
            float force = Random.Range(minForce, maxForce);
            fruit.GetComponent<Rigidbody>().AddForce(fruit.transform.up * force, ForceMode.Impulse);

            yield return new WaitForSeconds(Random.Range(minSpawnDelay, maxSpawnDelay));
        }
    }
}

