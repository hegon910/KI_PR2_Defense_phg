using AdvancedPeopleSystem;
using System.Collections.Generic;
using UnityEngine;

public class FakeDevInitializer : MonoBehaviour
{
    [Header("모델 참조")]
    [SerializeField] private GameObject humanModel;
    [SerializeField] private GameObject monsterModel;

    [Header("캐릭터 커스터마이징")]
    [SerializeField] private CharacterCustomization customizableCharacter;
    [SerializeField] private List<CharacterSettings> settingsPool;

    public void Reinitialize()
    {
        if (humanModel != null)
            humanModel.SetActive(true);

        float randomVal = Random.Range(0f, 1f);
        bool hasMonster = randomVal < 0.2f;
        Debug.Log($"Monster chance: {randomVal} => {hasMonster}");

        if (monsterModel != null)
        {
            Renderer[] monsterRenderers = monsterModel.GetComponentsInChildren<Renderer>();
            foreach (var renderer in monsterRenderers)
                renderer.enabled = false;

            monsterModel.SetActive(hasMonster);
        }

        if (customizableCharacter != null && settingsPool.Count > 0)
        {
            var randomSettings = settingsPool[Random.Range(0, settingsPool.Count)];
            customizableCharacter.InitializeMeshes(randomSettings);

            // 완전 독립적인 시드 생성
            int seed = System.Guid.NewGuid().GetHashCode();
            UnityEngine.Random.InitState(seed);

            customizableCharacter.Randomize();
            Debug.Log($"Reinit with preset: {randomSettings.name}, Seed: {seed}");

            var cameraWalk = GetComponent<CameraWalk>();
            if (cameraWalk != null)
                cameraWalk.Reinitialize();
        }
    }
}