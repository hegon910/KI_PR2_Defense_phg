using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GlobalHealthManager : MonoBehaviour
{
    public static GlobalHealthManager Instance;
    public float MaxHealth = 100f;
    private float currentHealth;

    public int Score { get; private set; } = 0;

    void Awake()
    {
        Instance = this;
        currentHealth = MaxHealth;
    }
    public void AddScroe(int amount)
    {
        Score += amount;
        Debug.Log($"현재점수{Score}");
    }

    public void DecreaseHealth(float amount)
    {
        currentHealth -= amount;
        if(currentHealth <= 0)
        {
            Debug.Log("GameOver");
            FindObjectOfType<PlayerUIManager>()?.SHowGameOver();
        }
    }

    public float GetHealthRatio()
    {
        return Mathf.Clamp01(currentHealth / MaxHealth);
    }
}
