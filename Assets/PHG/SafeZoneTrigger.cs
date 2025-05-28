using UnityEngine;

public class SafeZoneTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        var initializer = other.GetComponent<FakeDevInitializer>();
        if (initializer == null) return;

        if (initializer.IsMonster())
            Debug.Log("괴물이 SafeZone에 접근함 - 처리가 필요할 수 있음");
        else
            Debug.Log("인간이 SafeZone에 접근함");

        var poolManager = FindObjectOfType<FakeDevPoolManager>();
        if (poolManager != null)
            poolManager.ReturnToPool(other.gameObject);
    }
}