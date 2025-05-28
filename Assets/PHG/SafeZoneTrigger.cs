using UnityEngine;

public class SafeZoneTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        var initializer = other.GetComponent<FakeDevInitializer>();
        if (initializer == null) return;

        if (initializer.IsMonster())
            Debug.Log("������ SafeZone�� ������ - ó���� �ʿ��� �� ����");
        else
            Debug.Log("�ΰ��� SafeZone�� ������");

        var poolManager = FindObjectOfType<FakeDevPoolManager>();
        if (poolManager != null)
            poolManager.ReturnToPool(other.gameObject);
    }
}