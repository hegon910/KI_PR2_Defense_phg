using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Manager")]
    public FakeDevPoolManager poolManager;
    public PlayerUIManager uiManager;
    public GameObject stageClearPanel;
    [SerializeField] private TextMeshProUGUI stageInfoText;

    private int currentStage = 1;
    private int enemiestToSpawn;
    private int enemiesProcessed;

    private bool isGameOver = false;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }


    public void StartStage()
    {
        stageClearPanel.SetActive(false);
        
        enemiestToSpawn = FakeDevPoolManager.Instance.poolSize + (currentStage - 1) * 10;
        enemiesProcessed = 0;

        stageInfoText.text = $"Stage{currentStage} - 방문 예정자는 {enemiestToSpawn} 명입니다.";

        FakeDevPoolManager.Instance.SpawnBatch(enemiestToSpawn);
    }
    

    public void OnEnemyProcessed()
    {
        if (isGameOver) return;

        enemiesProcessed++;

        if(enemiesProcessed >= enemiestToSpawn)
        {
            Debug.Log("스테이지 클리어");
            StartCoroutine(HandleStageClear());
        }
    }

    private IEnumerator HandleStageClear()
    {
        stageInfoText.text = $"Stage {currentStage} Clear\n Next {currentStage + 1} Stage, 방문 예정자 수:{enemiestToSpawn}";
        stageClearPanel.SetActive(true);
        yield return new WaitForSeconds(3f);
        stageClearPanel.SetActive(false);

        currentStage++;
        StartStage();
    
    }

    public void ResetGame()
    {
        StopAllCoroutines();
        isGameOver = false;
        currentStage = 1;
    }


    public void OnPlayerDied()
    {
        isGameOver = true;
    }
}
