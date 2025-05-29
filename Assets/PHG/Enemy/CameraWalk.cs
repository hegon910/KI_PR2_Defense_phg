using UnityEngine;

public class CameraWalk : MonoBehaviour
{
    [SerializeField] private GameObject humanModel;
    [SerializeField] private GameObject monsterModel;

    private Renderer[] monsterRenderers;
    private bool hasMonster = false;

    public void Reinitialize()
    {
        if (monsterModel != null)
        {
            monsterRenderers = monsterModel.GetComponentsInChildren<Renderer>();
            hasMonster = monsterRenderers != null && monsterRenderers.Length > 0;
            SetMonsterVisible(false);
        }
        else
        {
            hasMonster = false;
            monsterRenderers = null;
        }

        SetHumanVisible(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("LightZone")) return;

        if (monsterModel == null || !monsterModel.activeInHierarchy)
        {
            Debug.Log("Monster inactive or not present, keeping human visible.");
            return;
        }

        Debug.Log("Entered LightZone");
        SetHumanVisible(false);
        SetMonsterVisible(true);
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