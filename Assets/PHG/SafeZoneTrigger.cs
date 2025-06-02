using UnityEngine;

public class SafeZoneTrigger : MonoBehaviour
{
    [Header("Monster Sound Clips")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] monsterClips;
    private void OnTriggerEnter(Collider other)
    {
        var initializer = other.GetComponent<FakeDevInitializer>();
        if (initializer == null) return;

        if (initializer.IsMonster())
        {
            Debug.Log("������ SafeZone�� ������");
            GlobalHealthManager.Instance.DecreaseHealth(10f);

            if(monsterClips != null && monsterClips.Length > 0)
            {
                int randIndex = Random.Range(0, monsterClips.Length);
                audioSource.PlayOneShot(monsterClips[randIndex]);
            }
        }
        else
        {
            Debug.Log("�ΰ��� SafeZone�� ������");
            GlobalHealthManager.Instance.AddScroe(100);
        }

        var poolManager = FindObjectOfType<FakeDevPoolManager>();
        if (poolManager != null)
            poolManager.ReturnToPool(other.gameObject);
    }
}