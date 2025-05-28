using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalDetect : MonoBehaviour
{
    public Collider triggerZoneCollider;

    private readonly HashSet<Collider> objectsInside = new HashSet<Collider>();

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
