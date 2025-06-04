using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Animator))]
public class EnemyWalker : MonoBehaviour
{
    private NavMeshAgent agent;
    public Transform targetPlace;
    public float moveSpeed = 2f;
    [SerializeField] private Animator animator;
    [SerializeField] private Animator animator2;


    private void Awake()
    {
        if (GameManager.IsGameOver) return;
        agent = GetComponent<NavMeshAgent>();

        if (targetPlace != null)
            agent.SetDestination(targetPlace.position);
        agent.speed = moveSpeed;
        if (animator == null)
            animator = GetComponentInChildren<Animator>(true);

        if (animator2 == null)
            animator2 = GetComponentInChildren<Animator>(true);
    }
    private void Start()
    {
        if (GameManager.IsGameOver) return;

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

    public void Activate()
    {
        if (GameManager.IsGameOver) return;
        if (targetPlace != null && agent != null && agent.isOnNavMesh)
        {
            agent.SetDestination(targetPlace.position);
        }
    }

    private void Update()
    {
      //  transform.position += transform.forward * moveSpeed * Time.deltaTime;
    }
}
