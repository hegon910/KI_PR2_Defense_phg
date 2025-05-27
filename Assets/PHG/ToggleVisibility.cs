using UnityEngine;
using System.Collections.Generic;

public class SpotlightTriggerToggle : MonoBehaviour
{
    public Light spotLight;
    public Collider triggerZoneCollider;

    private bool isActive = false;
    private readonly HashSet<Collider> objectsInside = new HashSet<Collider>();

    void Start()
    {
        if (spotLight != null)
            spotLight.enabled = false;

        if (triggerZoneCollider != null)
            triggerZoneCollider.enabled = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            isActive = !isActive;

            if (spotLight != null)
                spotLight.enabled = isActive;

            if (triggerZoneCollider != null)
            {
                if (!isActive)
                {
                    // Trigger 끄기 전에 강제 Exit 처리
                    foreach (var obj in objectsInside)
                    {
                        // 수동으로 처리할 로직 넣기
                        obj.SendMessage("OnCustomTriggerExit", SendMessageOptions.DontRequireReceiver);
                    }
                    objectsInside.Clear();
                }

                triggerZoneCollider.enabled = isActive;
            }
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