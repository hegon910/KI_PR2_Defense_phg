using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FakeDevPoolManager : MonoBehaviour
{
    [Header("������ ������")]
    public GameObject fakeDevPrefab;

    [Header("���� ��ġ��")]
    public Transform[] spawnPoints;

    [Header("�ʱ� Ǯ ������")]
    public int poolSize = 30;

    [Header("���� �� ������ (��)")]
    public float spawnDelay = 2f;

    private List<GameObject> objectPool = new List<GameObject>();

    [Header("����� Target")]
    public Transform goalTransform;


    public static FakeDevPoolManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

    }
    private void Start()
    {
        InitializePool();
        //StartCoroutine(SpawnBatchDelayed(poolSize / 2));
    }

    private void InitializePool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(fakeDevPrefab);
            obj.SetActive(false);
            objectPool.Add(obj);
        }
    }

    public void SpawnBatch(int totalCount, int groupSize)
    {
        if (GameManager.IsGameOver) return;
        StartCoroutine(SpawnBatchDelayed(totalCount, groupSize, spawnDelay));
    }


    private IEnumerator SpawnBatchDelayed(int totalCount, int groupSize, float delay)
    {
        int spawned = 0;

        while (spawned < totalCount)
        {
            int batchCount = Mathf.Min(groupSize, totalCount - spawned);

            for (int i = 0; i < batchCount; i++)
            {
                SpawnFromPool();
                spawned++;
            }

            yield return new WaitForSeconds(delay);
        }
    }

    public void SpawnFromPool()
    {
        if (GameManager.IsGameOver) return;
        foreach (GameObject obj in objectPool)
        {
            if (!obj.activeInHierarchy)
            {
                Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
                obj.transform.position = spawnPoint.position;
                obj.transform.rotation = spawnPoint.rotation;

                var initializer = obj.GetComponent<FakeDevInitializer>();
                if (initializer != null)
                    initializer.Reinitialize();

                obj.SetActive(true); //  ���� Ȱ��ȭ
                var agent = obj.GetComponent<NavMeshAgent>();
                if (agent != null)
                {
                    if (!agent.isOnNavMesh)
                    {
                        return;
                    }
                    agent.enabled = false;
                    agent.enabled = true;
                }

                var walker = obj.GetComponent<EnemyWalker>();
                if (walker != null && goalTransform != null)
                {
                    walker.targetPlace = goalTransform;
                    walker.Activate(); // ���ο��� isOnNavMesh üũ ����
                }

                return;
            }
        }
    }


    public void ReturnToPool(GameObject obj)
    {
        obj.SetActive(false);
        GameManager.Instance?.OnEnemyProcessed();
    }


}