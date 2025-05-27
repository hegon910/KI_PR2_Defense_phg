using AdvancedPeopleSystem;
using System.Collections.Generic;
using UnityEngine;

public class FakeDevInitializer : MonoBehaviour
{
    [Header("�� ����")]
    [SerializeField] private GameObject humanModel;
    [SerializeField] private GameObject monsterModel;

    [Header("ĳ���� Ŀ���͸���¡")]
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

            // ���� �������� �õ� ����
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