using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Health : MonoBehaviour
{
    public TeamClass teamClass;

    public float startHealth = 100;
    public float clearCorpseTime = 10f;

    [HideInInspector]
    public float currentHealth;
    private float stayCorpseTime;

    [Header("HpSetUp")]
    public Transform hpCanvasHolder;
    public Color colorFullHp = Color.white;
    public Color colorNoHp = Color.red;
    public Slider hpSlider;
    public Image[] hpColor;
    public int hpColorArraySelector = 1;
    [Range(0, 1)]
    public float hpBarRotateSpeed = 0.9f;

    private Weapons weapon;

    [HideInInspector]
    public bool isDead = false;
    public void Start()
    {
        #region get set hpUI Component
        hpCanvasHolder = this.transform.Find("HpCanvasHolder");
        hpSlider = GetComponentInChildren<Slider>();
        hpColor = GetComponentsInChildren<Image>();
        colorFullHp = hpColor[hpColorArraySelector].color;
        #endregion
        ResetState(teamClass.team);
    }

    public virtual void ResetState(TeamClass.Teams newTeam)
    {
        switch (newTeam)
        {
            case TeamClass.Teams.Dead:
                currentHealth = 0;

                isDead = true;
                break;
            case TeamClass.Teams.Player:
            case TeamClass.Teams.Enemy:
                currentHealth = startHealth;

                isDead = false;
                break;
            default:
                Debug.Log("some thing wrong in ResetState:" + this);
                break;
        }
        stayCorpseTime = 0;
        teamClass.team = newTeam;

        SetHealthVisual();
        GetComponent<Renderer>().material = teamClass.teamMaterial[(int)teamClass.team];

        weapon = GetComponentInChildren<Weapons>();
        if (weapon != null)
            weapon.team = newTeam;
    }

    public void Update()
    {

        #region test
        //if (Input.GetKeyDown("t"))//TAKE DAMAGE
        //{
        //    TakeDamage(10);
        //}
        //if (Input.GetKeyDown("r"))//CHANGE TEAM
        //{
        //    TeamClass.Teams _team = teamClass.team;
        //    _team = (int)++_team < sizeof(TeamClass.Teams) - 1 ? _team : TeamClass.Teams.Player;
        //    ResetState(_team);
        //}
        //if (Input.GetKeyDown("f"))//FULL HEALTH
        //{
        //    ResetState(teamClass.team);
        //}
        #endregion

        if (isDead)
        {
            DestroyIdleTooLongCorpse(clearCorpseTime);
        }
        if (transform.position.y <= -10f)
        {
            Death();
        }
    }

    private void FixedUpdate()
    {
        RotateHpCanvasHolder();
    }

    void RotateHpCanvasHolder()
    {
        Quaternion counterRotation = Quaternion.Inverse(this.transform.localRotation);
        hpCanvasHolder.transform.localRotation = Quaternion.Lerp(hpCanvasHolder.transform.localRotation, counterRotation, hpBarRotateSpeed * 50f * Time.fixedDeltaTime);
    }

    public void TakeDamage(float amount)
    {
        if (isDead)
            return;
        currentHealth -= amount;
        SetHealthVisual();
        if (currentHealth <= 0)
        {
            this.Death();
        }
    }

    public void GainHealth(float amount)
    {
        if (isDead)
            return;
        currentHealth += amount;
        if (currentHealth > startHealth)
            currentHealth = startHealth;
        SetHealthVisual();
    }

    public void SetHealthVisual()
    {
        float HpPercent = currentHealth / startHealth;

        hpSlider.value = HpPercent;
        hpColor[hpColorArraySelector].color = Color.Lerp(colorNoHp, colorFullHp, HpPercent);
    }

    public virtual void Death()
    {
        if (isDead)
            return;
        ResetState(TeamClass.Teams.Dead);
    }

    public void DestroyIdleTooLongCorpse(float sec)
    {
        stayCorpseTime += Time.deltaTime;
        if (stayCorpseTime > sec)
        {
            Destroy(this.gameObject);
        }
    }
}
