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
    [SerializeField] private TMPro.TextMeshProUGUI gameOvertxt;

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
        gameOvertxt.text =  $"Game OVER \n Your Score : {scoreText.text}";
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
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


    public void OnReturnToTitleClicked()
    {
        Time.timeScale = 1f;
        gameOverPanel.SetActive(false);
        SceneManager.LoadScene("TitleScene");

    }
}
