using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Control : MonoBehaviour
{

    public float startMoveSpeed = 500f;
    public Transform weaponHoldPlace;

    [SerializeField]
    protected bool debug = true;

    protected Rigidbody thisRigidbody;
    protected Health thisHealth;
    protected Vector3 movement;
    protected float moveSpeed;

    private float verticalSpeedCorrection;
    private bool isStunned;


    private float timer;
    private bool isCallResumeSpeed = false;
    
    protected Weapons weapon = null;
    private SphereCollider thisCollider;

    private void OnEnable()
    {
        verticalSpeedCorrection = Camera.main.transform.rotation.eulerAngles.x;
        verticalSpeedCorrection = Mathf.Sin(verticalSpeedCorrection / 180 * Mathf.PI);
    }

    public void Start()
    {
        thisHealth = GetComponent<Health>();
        thisRigidbody = GetComponent<Rigidbody>();
        thisCollider = GetComponent<SphereCollider>();
        thisCollider.enabled = true;
        isStunned = false;
        moveSpeed = startMoveSpeed;
    }
    protected virtual void Update()
    {
        if(weapon!=null)
            weapon.DisableEffects();
        if (isCallResumeSpeed)
            ResumeSpeed();
                   
    }

    private void OnTriggerEnter(Collider other)
    {
        Weapons _weapon = other.GetComponent<Weapons>();
        if (_weapon != null)
        {
            ReceiveWeapon(_weapon);
        }

    }

    public bool IsCantMove()
    {
        return isStunned || thisHealth.isDead;
    }

    protected virtual void Move()
    {
        //need to set movement in child

        movement = movement.normalized * moveSpeed * Time.deltaTime;
        OrthographicParallaxVerticalSpeedCorrection();

        thisRigidbody.MovePosition(transform.position + movement * Time.fixedDeltaTime);
    }
    protected virtual void Turning() { }
    protected void OrthographicParallaxVerticalSpeedCorrection()
    {
        movement.z /= verticalSpeedCorrection;
    }



    public void Stunned(float duration)
    {
        StartCoroutine(PausePlayerControlled(duration));
    }
    public IEnumerator PausePlayerControlled(float duration)
    {
        isStunned = true;
        yield return new WaitForSeconds(duration);
        isStunned = false;
    }

    public void SpeedEffectRatio(float ratio)
    {

        moveSpeed = startMoveSpeed * ratio;
    }

    public void SpeedEffectRatio(float ratio, float duration)
    {

        SpeedEffectRatio(ratio);
        timer = duration;
        isCallResumeSpeed = true;
    }

    public void ResumeSpeed()
    {
        timer -= Time.deltaTime;
        if (timer < 0)
        {
            SpeedEffectRatio(1);
            isCallResumeSpeed = false;
        }
    }

    public void ReceiveWeapon(Weapons _weapon)
    {
        _weapon.isIdle = false;
        _weapon.GetComponent<Animator>().enabled = false;
        _weapon.GetComponent<Collider>().enabled = false;
        _weapon.transform.position = Vector3.zero;
        _weapon.transform.rotation = Quaternion.identity;
        _weapon.transform.SetParent(weaponHoldPlace.transform, false);
        _weapon.team=thisHealth.teamClass.team;
        if (weapon != null)
            Destroy(weapon.gameObject);
        
        weapon = _weapon;
    }
}



