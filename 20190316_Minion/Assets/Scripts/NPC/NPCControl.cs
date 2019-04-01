using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCControl : Control
{
    public float seekRange = 20f;
    public float stopRange=5f;


    private Transform playerTransform;
    private PlayerHealth playerHealth;
    [SerializeField]
    private Transform targetTransform;

    private int shootableMask = 9;
    
    void Awake()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        if(targetTransform==null)
        targetTransform = playerTransform;
        playerHealth = playerTransform.GetComponent<PlayerHealth>();

        shootableMask = LayerMask.GetMask("Shootable");
        InvokeRepeating("Seek", 0f, 0.1f);
    }

    protected override void Update()
    {
        base.Update();

        if (IsCantMove())
            return;
        if (weapon != null)
        {
            if (targetTransform == null)
                return;
            Health _targetHealth = targetTransform.GetComponent<Health>();
            if (_targetHealth != null)
            {
                if (TargetInRange() && _targetHealth.teamClass.team != base.thisHealth.teamClass.team)
                {
                    weapon.Shoot();
                }
            }
        }
    }

    private void FixedUpdate()
    {
        if (IsCantMove())
            return;
        SetMovement();
    }
    private void SetMovement()
    {
        if (targetTransform != null) { 
        movement = targetTransform.position - this.transform.position;
        movement.y = 0;
        }
        if (weapon != null)
        {
            if (movement.magnitude < stopRange)
            {
                Turning();
                return;
            }
        }

        Turning();
        Move();
    }
    protected override void Move()
    {
        base.Move();
    }
    protected override void Turning()
    {

        Quaternion newRotation = Quaternion.LookRotation(movement);
        newRotation=Quaternion.Slerp(transform.rotation, newRotation, 0.1f);
        thisRigidbody.MoveRotation(newRotation);
    }

    private void Seek()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, seekRange, shootableMask);
        if (colliders == null)
            targetTransform = playerTransform;
        else
        {
            if (weapon == null)
            {//findweapon
                float shortestSqrMagnitude = Mathf.Infinity;
                foreach (Collider collider in colliders)
                {
                    Weapons _targetWeapon = collider.GetComponent<Weapons>();
                    if (_targetWeapon != null)
                    {
                        float sqrMagnitudeToTarget = (transform.position - _targetWeapon.transform.position).sqrMagnitude;
                        if (shortestSqrMagnitude > sqrMagnitudeToTarget)
                        {
                            shortestSqrMagnitude = sqrMagnitudeToTarget;
                            targetTransform = _targetWeapon.transform;
                        }
                    }
                }
            }
            else
            {//findEnemyTeam
                float shortestSqrMagnitude = Mathf.Infinity;
                foreach (Collider collider in colliders)
                {
                    Health _targetHealth = collider.GetComponent<Health>();
                    if (_targetHealth != null && _targetHealth.teamClass.team != TeamClass.Teams.Dead && _targetHealth.teamClass.team != thisHealth.teamClass.team)
                    {
                        float sqrMagnitudeToTarget = (transform.position - _targetHealth.transform.position).sqrMagnitude;
                        if (shortestSqrMagnitude > sqrMagnitudeToTarget)
                        {
                            shortestSqrMagnitude = sqrMagnitudeToTarget;
                            targetTransform = _targetHealth.transform;
                        }
                    }
                }
                if (shortestSqrMagnitude == Mathf.Infinity)
                    targetTransform = playerTransform;
            }
        }
    }

    private bool TargetInRange()
    {

        if ((transform.position - targetTransform.position).magnitude < weapon.range)
            return true;
        else
            return false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, stopRange);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, seekRange);
    }
}
