using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TitleScreenManager : MonoBehaviour
{
    public PlayableDirector timeline;
    public GameObject uiCanvas;
    public GameObject playerController;

    private void Start()
    {
        uiCanvas.SetActive(true);
        timeline.Play();
        playerController.SetActive(false);
        timeline.stopped += OnTimelineEnd;
    }

    private void OnTimelineEnd(PlayableDirector obj)
    {
        uiCanvas.SetActive(false);
        playerController.SetActive(true);
    }

    public void OnStartButtonClicked()
    {
        uiCanvas.SetActive(false);
        playerController.SetActive(true);
    }
}
