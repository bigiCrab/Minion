using UnityEngine;
using System.Collections;

[System.Serializable]
public class ObjectSpawnBluePrint
{
    public GameObject objectPrefab;
    public bool checkToSpawn=true;
    [Range(0.2f, 60f)]
    public float spawnCoolDown=5f;
    [Range(1f, 60f)]
    public float spawnRangeOffset=40f;

    public bool checkToDistoryOverTime = false;
    [Range(1f, 60f)]
    public float spawnObjLiveTime=20f;
    [HideInInspector]
    public float untilSpawnTime;
}