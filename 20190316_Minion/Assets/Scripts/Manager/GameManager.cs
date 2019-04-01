using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    public bool isGameOver;

    public int score;

    public int trapCount;
    public int npcPlayerTeamCount;
    public int npcEnemyTeamCount;
    public int npcDeadTeamCount;
    public int weaponCount;

    [Header("Some Value")]
    public static int killScoreValue = 100;
    public static int reviveScoreValue = 10;

    private bool gameOverUIIsShow = false;
    public int levelDifficulty = 0;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        instance = this;

        DontDestroyOnLoad(this.gameObject);

    }

    private void Start()
    {
        SetUp();
    }

    public void SetUp()
    {
        isGameOver = false;
        score = 0;
        gameOverUIIsShow = false;
        levelDifficulty = 0;
        UIManager.UpdateCountTextUI();
        UIManager.instance.gameoverUI.SetActive(false);
    }

    void Update()
    {
        if (isGameOver)
        {
            if (!gameOverUIIsShow)
            {
                gameOverUIIsShow = true;
                ShowGameOver();
            }
            return;
        }
        if (score > 2000 && levelDifficulty == 0)
        {
            levelDifficulty = 1;
            SpawnManager.instance.LevelUp(levelDifficulty);
        }
        else if (score > 20000 && levelDifficulty == 1)
        {
            levelDifficulty = 2;
            SpawnManager.instance.LevelUp(levelDifficulty);
        }
        else if (score > 80000 && levelDifficulty == 2)
        {
            levelDifficulty = 3;
            SpawnManager.instance.LevelUp(levelDifficulty);
        }
    }


    public static void PlayerDied()
    {
        if (instance == null)
            return;

        //UIManager

    }

    public static void AddAndUpdateScore(int _amount)
    {
        if (instance == null)
            return;

        instance.score += _amount;

        UIManager.UpdateScoreTextUI(instance.score);
    }

    public static void AddAndUpdateTrapCount(int _amount)
    {
        if (instance == null)
            return;

        instance.trapCount += _amount;

        UIManager.UpdateTrapCountTextUI(instance.trapCount);
    }

    public static void AddAndUpdateNpcPlayerTeamCount(int _amount)
    {
        if (instance == null)
            return;

        instance.npcPlayerTeamCount += _amount;

        UIManager.UpdateNpcPlayerTeamCountTextUI(instance.npcPlayerTeamCount);
    }

    public static void AddAndUpdateNpcEnemyTeamCount(int _amount)
    {
        if (instance == null)
            return;

        instance.npcEnemyTeamCount += _amount;

        UIManager.UpdatenpcEnemyTeamCountTextUI(instance.npcEnemyTeamCount);
    }

    public static void AddAndUpdateNpcDeadTeamCount(int _amount)
    {
        if (instance == null)
            return;

        instance.npcDeadTeamCount += _amount;
        //no UI for now
        // UIManager.UpdateScoreTextUI(instance.npcEnemyTeamCount);
    }

    public static void AddAndUpdateWeaponCount(int _amount)
    {
        if (instance == null)
            return;

        instance.weaponCount += _amount;
        //no UI for now
        //UIManager.UpdateScoreTextUI(instance.weaponCount);
    }

    public static void ShowGameOver()
    {
        if (instance == null)
            return;


        UIManager.DisplayGameOverUI(instance.score);
    }
}
