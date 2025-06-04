using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdentity : MonoBehaviour
{
    public bool IsMonster = false;
    public bool IsRevealed = false;

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
            Debug.Log("몬스터 처치");
            GlobalHealthManager.Instance.AddScroe(100);
        }
        else
        {
            Debug.Log("시민 사살 패터닐 예정");
            GlobalHealthManager.Instance.DecreaseHealth(20f);
            GlobalHealthManager.Instance.AddScroe(-200);
        }

        FakeDevPoolManager.Instance.ReturnToPool(gameObject);
    }



}
