using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine.AI;

public class FakeDevPoolManager : MonoBehaviour
{
    [Header("스폰할 프리팹")]
    public GameObject fakeDevPrefab;

    [Header("스폰 위치들")]
    public Transform[] spawnPoints;

    [Header("초기 풀 사이즈")]
    public int poolSize = 20;

    [Header("스폰 간 딜레이 (초)")]
    public float spawnDelay = 0.05f;

    private List<GameObject> objectPool = new List<GameObject>();

    [Header("명시할 Target")]
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

                obj.SetActive(true); //  먼저 활성화
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
                    walker.Activate(); // 내부에서 isOnNavMesh 체크 포함
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