using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    private void Start()
    {
        GameManager.AddAndUpdateTrapCount(1);
    }
    private void OnDestroy()
    {
        GameManager.AddAndUpdateTrapCount(-1);
    }
}
