using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

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
                {
                    initializer.Reinitialize(); // �ݵ�� ���⿡ ȣ��
                }
                var walker = obj.GetComponent<EnemyWalker>();
                if (walker != null && goalTransform != null)
                    walker.targetPlace = goalTransform;

                obj.SetActive(true);
                return;
            }
        }
    }

    public void ReturnToPool(GameObject obj)
    {
        obj.SetActive(false);
    }
    

}