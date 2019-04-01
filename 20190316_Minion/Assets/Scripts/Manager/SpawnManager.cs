
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager instance = null;
    private Transform player;

    [Header("Trap")]
    public ObjectSpawnBluePrint rotateSpikeBluePrint;
    public ObjectSpawnBluePrint underGroundSpikeBluePrint;
    public ObjectSpawnBluePrint speedDownAreaBluePrint;

    private ObjectSpawnHelper rotateSpike;
    private ObjectSpawnHelper underGroundSpike;
    private ObjectSpawnHelper speedDownArea;


    [Header("NPC")]
    public ObjectSpawnBluePrint npcTeamPlayerBluePrint;
    public ObjectSpawnBluePrint npcTeamEnemyBluePrint;

    private ObjectSpawnHelper npcTeamPlayer;
    private ObjectSpawnHelper npcTeamEnemy;


    [Header("Weapon")]
    public ObjectSpawnBluePrint bazookaBluePrint;
    public ObjectSpawnBluePrint pistolBluePrint;
    public ObjectSpawnBluePrint lazerBluePrint;

    private ObjectSpawnHelper bazooka;
    private ObjectSpawnHelper pistol;
    private ObjectSpawnHelper lazer;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        instance = this;
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        rotateSpike = new ObjectSpawnHelper(rotateSpikeBluePrint);
        underGroundSpike = new ObjectSpawnHelper(underGroundSpikeBluePrint);
        speedDownArea = new ObjectSpawnHelper(speedDownAreaBluePrint);

        npcTeamPlayer = new ObjectSpawnHelper(npcTeamPlayerBluePrint);
        npcTeamEnemy = new ObjectSpawnHelper(npcTeamEnemyBluePrint);

        bazooka = new ObjectSpawnHelper(bazookaBluePrint);
        pistol = new ObjectSpawnHelper(pistolBluePrint);
        lazer = new ObjectSpawnHelper(lazerBluePrint);

    }

    void Update()
    {
        if (GameManager.instance.isGameOver)
            return;


        rotateSpike.SpawnObjectIfSpawnTimeIsUp(player.position);
        underGroundSpike.SpawnObjectIfSpawnTimeIsUp(player.position);
        speedDownArea.SpawnObjectIfSpawnTimeIsUp(player.position);

        npcTeamPlayer.SpawnObjectIfSpawnTimeIsUp(player.position);
        npcTeamEnemy.SpawnObjectIfSpawnTimeIsUp(player.position);

        bazooka.SpawnObjectIfSpawnTimeIsUp(player.position);
        pistol.SpawnObjectIfSpawnTimeIsUp(player.position);
        lazer.SpawnObjectIfSpawnTimeIsUp(player.position);


    }

    public void LevelUp(int level)
    {
        switch (level)
        {
            case 1:
                npcTeamEnemy.objectSpawnBluePrint.spawnCoolDown = 1f;
                break;
            case 2:
                npcTeamEnemy.objectSpawnBluePrint.spawnCoolDown = 0.5f;
                rotateSpike.objectSpawnBluePrint.spawnCoolDown = 1f;
                break;
            case 3:
                npcTeamEnemy.objectSpawnBluePrint.spawnCoolDown = 0.2f;
                npcTeamPlayer.objectSpawnBluePrint.spawnCoolDown = 1f;
                bazooka.objectSpawnBluePrint.spawnCoolDown = 1.1f;
                lazer.objectSpawnBluePrint.spawnCoolDown = 1.2f;
                pistol.objectSpawnBluePrint.spawnCoolDown = 1.3f;
                break;
        }
    }

}
