using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TeamClass
{
    public enum Teams
    {
        Player,
        Enemy,
        Dead
    };

    public Teams team;

    public Material[] teamMaterial;

}
