using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawnHelper : MonoBehaviour
{

    public ObjectSpawnBluePrint objectSpawnBluePrint;

    public ObjectSpawnHelper(ObjectSpawnBluePrint targetBluePrint)
    {
        objectSpawnBluePrint = targetBluePrint;
        objectSpawnBluePrint.untilSpawnTime = objectSpawnBluePrint.spawnCoolDown;
    }

    public void SpawnObjectIfSpawnTimeIsUp(Vector3 targetPosition)
    {
        if (objectSpawnBluePrint.checkToSpawn)
        {
            objectSpawnBluePrint.untilSpawnTime -= Time.deltaTime;
            if (objectSpawnBluePrint.untilSpawnTime <= 0)
            {
                SpawnEnvironmentObject(targetPosition, objectSpawnBluePrint.objectPrefab);
                objectSpawnBluePrint.untilSpawnTime = objectSpawnBluePrint.spawnCoolDown;
            }
        }
    }
    private void SpawnEnvironmentObject(Vector3 targetPosition, GameObject obj)
    {
        Vector3 spawnPoint = targetPosition;
        spawnPoint.x += Random.Range(-objectSpawnBluePrint.spawnRangeOffset, objectSpawnBluePrint.spawnRangeOffset);
        spawnPoint.z += Random.Range(-objectSpawnBluePrint.spawnRangeOffset, objectSpawnBluePrint.spawnRangeOffset);
        spawnPoint.y = obj.transform.position.y;

        GameObject environmentObject = Instantiate(obj, spawnPoint, Quaternion.Euler(0f, Random.Range(0f, 360f), 0f));

        if(objectSpawnBluePrint.checkToDistoryOverTime)
        Destroy(environmentObject, objectSpawnBluePrint.spawnObjLiveTime);
    }



}
