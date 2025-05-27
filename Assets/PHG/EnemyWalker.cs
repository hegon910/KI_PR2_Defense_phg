using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class EnemyWalker : MonoBehaviour
{

    public float moveSpeed = 2f;
    [SerializeField] private Animator animator;


    private void Awake()
    {
        if (animator == null)
        {
            animator = GetComponentInChildren<Animator>(true);
        }
    }
    private void Start()
    {
        if (animator == null || animator.runtimeAnimatorController == null)
        {
            Debug.LogError("Animator or AnimatorController is missing on: " + name);
            return;
        }

        if (!animator.enabled)
        {
            Debug.LogError("Animator component is disabled on: " + name);
            return;
        }

        animator.SetBool("IsWalking", true);
    }

    private void Update()
    {
        transform.position += transform.forward * moveSpeed * Time.deltaTime;
    }
}
