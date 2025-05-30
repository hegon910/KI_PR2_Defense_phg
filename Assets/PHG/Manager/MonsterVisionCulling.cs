using UnityEngine;

public class MonsterVisionCulling : MonoBehaviour
{
    private GameObject humanModel;
    private GameObject monsterModel;

    private void Awake()
    {
        humanModel = transform.Find("HumanModel")?.gameObject;
        monsterModel = transform.Find("MonsterModel")?.gameObject;
    }

    private void LateUpdate()
    {
        if (humanModel == null || monsterModel == null) return;

        int layerHuman = LayerMask.NameToLayer("Human");
        int layerMonster = LayerMask.NameToLayer("Monster");
        int layerIgnore = LayerMask.NameToLayer("IgnoreVision");

        if (layerHuman < 0 || layerMonster < 0 || layerIgnore < 0)
        {
            Debug.LogError("[MonsterVisionCulling] 레이어 설정 오류");
            return;
        }

        bool monsterVisible = false;
        Renderer[] monsterRenderers = monsterModel.GetComponentsInChildren<Renderer>(true);
        foreach (var r in monsterRenderers)
        {
            if (r.enabled)
            {
                monsterVisible = true;
                break;
            }
        }

        if (monsterVisible)
        {
            // 악마가 보이기 시작했을 때
            SetLayerRecursively(monsterModel, layerMonster);
            SetLayerRecursively(humanModel, layerIgnore);
        }
        else
        {
            // 평상시: Human은 보이고 Monster는 숨김
            SetLayerRecursively(humanModel, layerHuman);
            SetLayerRecursively(monsterModel, layerIgnore);
        }
    }

    private void SetLayerRecursively(GameObject obj, int newLayer)
    {
        if (obj == null) return;
        obj.layer = newLayer;
        foreach (Transform child in obj.transform)
        {
            SetLayerRecursively(child.gameObject, newLayer);
        }
    }
}