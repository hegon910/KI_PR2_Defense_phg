using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalHealthManager : MonoBehaviour
{
    public static GlobalHealthManager Instance;
    public float MaxHealth = 100f;
    private float currentHealth;

    void Awake()
    {
        Instance = this;
        currentHealth = MaxHealth;
    }

    public void DecreaseHealth(float amount)
    {
        currentHealth -= amount;
        if(currentHealth <= 0)
        {
            Debug.Log("GameOver");
            //TODO : GameOver Ã³¸®
        }
    }

}
