using UnityEngine;

public class CameraWalk : MonoBehaviour
{
    [SerializeField] private GameObject humanModel;
    [SerializeField] private GameObject monsterModel;

    private Renderer[] monsterRenderers;

    private void Start()
    {
        if (monsterModel != null)
            monsterRenderers = monsterModel.GetComponentsInChildren<Renderer>();

        SetMonsterVisible(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("LightZone")) return;

        Debug.Log("Entered LightZone");

        SetHumanVisible(false);
        SetMonsterVisible(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("LightZone")) return;

        SetHumanVisible(true);
        SetMonsterVisible(false);
    }

    //  이 메서드를 Collider 강제 종료 시에도 호출할 수 있도록 제공
    public void OnCustomTriggerExit()
    {
        Debug.Log("Forced Exit Trigger");

        SetHumanVisible(true);
        SetMonsterVisible(false);
    }

    private void SetHumanVisible(bool isVisible)
    {
        if (humanModel != null)
            humanModel.SetActive(isVisible);
    }

    private void SetMonsterVisible(bool isVisible)
    {
        if (monsterRenderers == null) return;

        foreach (var renderer in monsterRenderers)
            renderer.enabled = isVisible;
    }
}