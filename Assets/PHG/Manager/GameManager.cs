using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor.ShaderGraph;

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

    public static bool IsGameOver { get; private set; }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }


    public void StartStage()
    {
        IsGameOver = false;
        stageClearPanel.SetActive(false);
        
        enemiestToSpawn = FakeDevPoolManager.Instance.poolSize + (currentStage - 1) * 10;
        enemiesProcessed = 0;
        StartCoroutine(StageText());
        if (stageInfoText != null)
        {
            stageInfoText.text = $"Stage{currentStage} - �湮 �����ڴ� {enemiestToSpawn} ���Դϴ�.";
        }

        FakeDevPoolManager.Instance.SpawnBatch(enemiestToSpawn, currentStage);
    }
    
    private IEnumerator StageText()
    {
        stageInfoText.gameObject.SetActive(true);

        yield return new WaitForSeconds(3f);
        stageInfoText.gameObject.SetActive(false);

    }

    public void OnEnemyProcessed()
    {
        if (IsGameOver) return;

        enemiesProcessed++;

        if(enemiesProcessed >= enemiestToSpawn)
        {
            Debug.Log("�������� Ŭ����");
            StartCoroutine(HandleStageClear());
        }
    }

    private IEnumerator HandleStageClear()
    {
        stageInfoText.text = $"Stage Clear! \n Next STAGE: {currentStage + 1} Stage, �湮 ������ :{enemiestToSpawn+ 10} ��";
        stageInfoText.gameObject.SetActive(true);
        stageClearPanel.SetActive(true);
        yield return new WaitForSeconds(5f);
        stageClearPanel.SetActive(false);
        stageInfoText.gameObject.SetActive(false);

        currentStage++;
        StartStage();
    
    }



    public void OnPlayerDied()
    {
        IsGameOver = true;
        var player = FindObjectOfType<PlayerContorl>();
        if (player != null)
        {
            player.DisableControls();
        }
    }
}
