using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance = null;

    public Text scoreText;
    //Didnt Show
    [HideInInspector]
    public Text trapCountText;
    public Text npcPlayerTeamCountText;
    public Text npcEnemyTeamCountText;

    [Header("gameOverUI")]
    public GameObject gameoverUI;
    public Text gameoverScoreText;



    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        DontDestroyOnLoad(this.gameObject);
        instance = this;
    }

    public void Retry()
    {
        GameManager.instance.SetUp();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Exit()
    {
        Debug.Log("Exciting...");
        Application.Quit();
    }

    public static void UpdateCountTextUI()
    {
        UpdateScoreTextUI(GameManager.instance.score);
        UpdateTrapCountTextUI(GameManager.instance.trapCount);
        UpdateNpcPlayerTeamCountTextUI(GameManager.instance.npcPlayerTeamCount);
        UpdatenpcEnemyTeamCountTextUI(GameManager.instance.npcEnemyTeamCount);
    }

    public static void UpdateScoreTextUI(int score)
    {
        if (instance == null)
            return;

        instance.scoreText.text = "Score: " + score.ToString();
    }

    public static void UpdateTrapCountTextUI(float trapCount)
    {
        if (instance == null)
            return;

        //instance.trapCountText.text = trapCount.ToString();
    }

    public static void UpdateNpcPlayerTeamCountTextUI(int npcPlayerTeamCount)
    {
        if (instance == null)
            return;

        instance.npcPlayerTeamCountText.text = "Allies: "+npcPlayerTeamCount.ToString();
    }

    public static void UpdatenpcEnemyTeamCountTextUI(int npcEnemyTeamCount)
    {
        if (instance == null)
            return;

        instance.npcEnemyTeamCountText.text = "Enemy: " + npcEnemyTeamCount.ToString();
    }

    public static void DisplayGameOverUI(int score)
    {
        if (instance == null)
            return;

        instance.gameoverScoreText.text = score.ToString();
        instance.gameoverUI.SetActive(true);
    }
}
