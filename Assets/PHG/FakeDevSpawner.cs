using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine.AI;

public class FakeDevPoolManager : MonoBehaviour
{
    [Header("������ ������")]
    public GameObject fakeDevPrefab;

    [Header("���� ��ġ��")]
    public Transform[] spawnPoints;

    [Header("�ʱ� Ǯ ������")]
    public int poolSize = 20;

    [Header("���� �� ������ (��)")]
    public float spawnDelay = 0.05f;

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
        StartCoroutine(SpawnBatchDelayed(poolSize / 2));
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

    private IEnumerator SpawnBatchDelayed(int count)
    {
        for (int i = 0; i < count; i++)
        {
            SpawnFromPool();
            yield return new WaitForSeconds(spawnDelay);
        }
    }

    public void SpawnFromPool()
    {
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
                        Debug.LogWarning($"{obj.name} is not on NavMesh! Cannot set destination.");
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
    }
    

}