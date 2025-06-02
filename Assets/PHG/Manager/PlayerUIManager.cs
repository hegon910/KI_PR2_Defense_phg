using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUIManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TMPro.TextMeshProUGUI ammoText;
    [SerializeField] private UnityEngine.UI.Slider healthSlider;
    [SerializeField] private TMPro.TextMeshProUGUI reloadText;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TMPro.TextMeshProUGUI scoreText;

    public void UpdateAmmo(int current, int max)
    {
        ammoText.text = $"Ammo : {current} / {max}";
    }

    public void UpdateHealth(float ratio)
    {
        healthSlider.value = ratio;
    }

    public void SetReloading(bool isReloading)
    {
        reloadText.gameObject.SetActive(isReloading);
    }

    public void UpdateScore(int score)
    {
        scoreText.text = $"Score : {score}";
    }

    public void SHowGameOver()
    {
        gameOverPanel.SetActive(true);
        Time.timeScale = 0f;
    }
}
