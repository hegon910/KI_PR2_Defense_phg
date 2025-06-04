using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerUIManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TMPro.TextMeshProUGUI ammoText;
    [SerializeField] private UnityEngine.UI.Slider healthSlider;
    [SerializeField] private TMPro.TextMeshProUGUI reloadText;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TMPro.TextMeshProUGUI scoreText;
    [SerializeField] private TMPro.TextMeshProUGUI toolTipText;

    private void Update()
    {
        CloseToolTip();
    }
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

    public void CloseToolTip()
    {
        if (Input.GetKeyDown(KeyCode.X) && toolTipText != null)
        {
            bool isActive = toolTipText.gameObject.activeSelf;
            toolTipText.gameObject.SetActive(!isActive);
        }
  
    }
    public void OnRetryClicked()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnReturnToTitleClicked()
    {
        Time.timeScale = 1f;

        GameManager.Instance?.ResetGame();

        var titleUI = FindObjectOfType<TtileUIManager>();
        if(titleUI != null)
        {
            titleUI.ResetToTitle();
        }

    }
}
