using System.Collections;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class TtileUIManager : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject titlePanel;
    public GameObject optionsPanel;
    public GameObject titleCanvas;

    [Header("Managers")]
    public PlayerContorl playercontroller;
    public FakeDevPoolManager poolManager;
    public GameObject playerUIManager;
    public PlayableDirector openingTimeline;
    public PlayableDirector gameStartTimeline;
    public Slider mouseSensitivitySlider;

    [Header("Audio")]
    public Slider masterVolumSlider;
    public AudioSource backgroundMusic;
    public AudioClip inGameBGM;

    private void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        titlePanel.SetActive(false);
        optionsPanel.SetActive(false);

        if (backgroundMusic != null)
            masterVolumSlider.value = backgroundMusic.volume;


        masterVolumSlider.onValueChanged.AddListener(OnVolumeChanged);
        if (playerUIManager != null)
            playerUIManager.SetActive(false);

        if (playercontroller != null) playercontroller.DisableControls();
        if (openingTimeline != null)
        {
            openingTimeline.Play();
            openingTimeline.stopped += OnTimelineEnd;
        }
        StartCoroutine(ShowTitleUIWithDelay());

        if (mouseSensitivitySlider != null)
        {
            mouseSensitivitySlider.minValue = 0.001f;
            mouseSensitivitySlider.maxValue = 0.1f;
            mouseSensitivitySlider.value = playercontroller != null ? playercontroller.sensitivity : 0.05f;

            mouseSensitivitySlider.onValueChanged.AddListener(OnMouseSensitivityChanged);
        }

    }
    public void OnMouseSensitivityChanged(float value)
    {
        if (playercontroller != null)
            playercontroller.SetSenseitivity(value);
    }
    private void OnTimelineEnd(PlayableDirector obj)
    {

    }

    private IEnumerator ShowTitleUIWithDelay()
    {
        yield return new WaitForSeconds(6f);
        titlePanel.SetActive(true);
        titleCanvas.SetActive(true);
    }

    public void OnStartGameClicked()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        titlePanel.SetActive(false);
        titleCanvas.SetActive(false);

        if (openingTimeline != null && openingTimeline.state == PlayState.Playing)
        {
            openingTimeline.Stop();
        }

        playercontroller.EnableControls();
        poolManager.StartSpawning();
        if (playerUIManager != null)
            playerUIManager.SetActive(true);

        if (backgroundMusic != null && inGameBGM != null)
        {
            backgroundMusic.clip = inGameBGM;
            backgroundMusic.loop = true;
            backgroundMusic.Play();
        }

        if (gameStartTimeline != null)
        {
            gameStartTimeline.Play();
        }


    }

    public void OnOptionsClciked()
    {
        titlePanel.SetActive(false);
        optionsPanel.SetActive(true);
    }

    public void OnVolumeChanged(float value)
    {
        if (backgroundMusic != null)
            backgroundMusic.volume = value;
    }
    public void OnBackFromOptions()
    {
        optionsPanel.SetActive(false);
        titlePanel.SetActive(true);     //
    }
    public void OnQuitClicked()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif
    }

}
