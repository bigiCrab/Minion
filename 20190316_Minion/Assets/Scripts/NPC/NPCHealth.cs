using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCHealth : Health
{
    new void Start()
    {
        base.Start();
    }

    new void Update()
    {
        base.Update();

    }

    public void Revive(TeamClass.Teams newTeam)
    {
        base.ResetState(newTeam);
    }

    public override void ResetState(TeamClass.Teams newTeam)
    {
        switch (newTeam)
        {
            case TeamClass.Teams.Dead:
                GameManager.AddAndUpdateNpcDeadTeamCount(1);

                if (teamClass.team == TeamClass.Teams.Enemy)
                    GameManager.AddAndUpdateScore(GameManager.killScoreValue);
                break;
            case TeamClass.Teams.Player:
                GameManager.AddAndUpdateNpcPlayerTeamCount(1);

                if (teamClass.team == TeamClass.Teams.Dead)
                    GameManager.AddAndUpdateScore(GameManager.reviveScoreValue);
                break;
            case TeamClass.Teams.Enemy:
                GameManager.AddAndUpdateNpcEnemyTeamCount(1);
                break;
        }
        if (teamClass.team != newTeam)
        {
            switch (teamClass.team)
            {
                case TeamClass.Teams.Dead:
                    GameManager.AddAndUpdateNpcDeadTeamCount(-1);
                    break;
                case TeamClass.Teams.Player:
                    GameManager.AddAndUpdateNpcPlayerTeamCount(-1);
                    break;
                case TeamClass.Teams.Enemy:
                    GameManager.AddAndUpdateNpcEnemyTeamCount(-1);
                    break;
            }
        }
        base.ResetState(newTeam);
    }

    public override void Death()
    {
        if (isDead)
            return;
        if(teamClass.team== TeamClass.Teams.Player)
            Destroy(this.gameObject);
        ResetState(TeamClass.Teams.Dead);
    }

    private void OnDestroy()
    {
        switch (teamClass.team)
        {
            case TeamClass.Teams.Dead:
                GameManager.AddAndUpdateNpcDeadTeamCount(-1);
                break;
            case TeamClass.Teams.Player:
                GameManager.AddAndUpdateNpcPlayerTeamCount(-1);
                break;
            case TeamClass.Teams.Enemy:
                GameManager.AddAndUpdateNpcEnemyTeamCount(-1);
                break;
        }
    }
}
