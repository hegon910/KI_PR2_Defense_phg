using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class EnemyWalker : MonoBehaviour
{

    public float moveSpeed = 2f;
    [SerializeField] private Animator animator;
    [SerializeField] private Animator animator2;


    private void Awake()
    {
        if (animator == null)
        {
            animator = GetComponentInChildren<Animator>(true);
            animator2 = GetComponentInChildren<Animator>(true);
        }
    }
    private void Start()
    {
        if (animator == null || animator.runtimeAnimatorController == null|| animator2 == null || animator2.runtimeAnimatorController == null)
        {
            Debug.LogError("Animator or AnimatorController is missing on: " + name);
            return;
        }

        if (!animator.enabled||!animator2.enabled)
        {
            Debug.LogError("Animator component is disabled on: " + name);
            return;
        }

        animator.SetBool("IsWalking", true);
        animator2.SetBool("IsWalking", true);
    }

    private void Update()
    {
        transform.position += transform.forward * moveSpeed * Time.deltaTime;
    }
}
