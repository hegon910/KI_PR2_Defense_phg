using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdentity : MonoBehaviour
{
    public bool IsMonster = false;
    public bool IsRevealed = false;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] humanClips;


    public void InitializeIdentity(bool isMonster)
    {
        IsMonster = isMonster;
        IsRevealed = false;
    }
    public void Reveal(bool isMonster)
    {
        IsRevealed = true;
        IsMonster = isMonster;
    }

    public void HandleHit()
    {
  
        if(IsMonster)
        {
            GlobalHealthManager.Instance.AddScroe(100);
        }
        else
        {
            if (humanClips != null && humanClips.Length > 0 && audioSource != null)
            {
                int randIndex = Random.Range(0, humanClips.Length);
                audioSource.clip = humanClips[randIndex];
                audioSource.Play();
                Debug.Log("음성 재생");

                StartCoroutine(WaitForAudioThenReturn(audioSource.clip.length));
            }
            else
            {
                FakeDevPoolManager.Instance.ReturnToPool(gameObject);
            }

            GlobalHealthManager.Instance.DecreaseHealth(20f);
            GlobalHealthManager.Instance.AddScroe(-200);
        }

        FakeDevPoolManager.Instance.ReturnToPool(gameObject);
    }

    private IEnumerator WaitForAudioThenReturn(float delay)
    {
        yield return new WaitForSeconds(delay);
        FakeDevPoolManager.Instance.ReturnToPool(gameObject);
    }


}
