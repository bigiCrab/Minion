using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : Health
{
    public GameManager gameManager;
    new void Start()
    {
        gameManager = GameManager.instance;
        base.Start();
    }
    
    new void Update()
    {
        base.Update();
        
    }

    public override void Death()
    {
        gameManager.isGameOver = true;
        base.Death();
    }
    
}
