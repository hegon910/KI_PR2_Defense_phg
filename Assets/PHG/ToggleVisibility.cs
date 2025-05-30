using UnityEngine;
using System.Collections.Generic;

public class SpotlightTriggerToggle : MonoBehaviour
{
    public Light spotLight;
    public Collider triggerZoneCollider;

    private bool isActive = false;
    private readonly HashSet<Collider> objectsInside = new HashSet<Collider>();

    [SerializeField] private PlayerContorl playerControl;

    void Start()
    {
        if (spotLight != null)
            spotLight.enabled = false;

        if (triggerZoneCollider != null)
        {
            Debug.Log($"{triggerZoneCollider.gameObject.name}");
            triggerZoneCollider.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            // Aiming 중이면 무시
            if (playerControl != null && playerControl.IsAiming())
            {
                Debug.Log("Aiming 중이므로 조명 토글 불가");
                return;
            }

            isActive = !isActive;

            if (spotLight != null)
                spotLight.enabled = isActive;
            if (triggerZoneCollider != null)
                triggerZoneCollider.gameObject.SetActive(isActive);

        }

        // Aiming 중일 때는 강제로 조명 OFF
        if (playerControl != null && playerControl.IsAiming())
        {
            if (spotLight != null && spotLight.enabled)
                spotLight.enabled = false;
            if (triggerZoneCollider != null && triggerZoneCollider.gameObject.activeSelf)
                triggerZoneCollider.gameObject.SetActive(false);

            isActive = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!objectsInside.Contains(other))
            objectsInside.Add(other);
    }

    private void OnTriggerExit(Collider other)
    {
        if (objectsInside.Contains(other))
            objectsInside.Remove(other);
    }
}