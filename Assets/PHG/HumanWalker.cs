using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanWalker : MonoBehaviour
{
    public float moveSpeed = 2f;


    private void Update()
    {
        transform.position += transform.forward * moveSpeed * Time.deltaTime;
    }
}
