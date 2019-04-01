using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [HideInInspector]
    public TeamClass.Teams team;

    [Header("General")]
    public float lifeTime = 5f;
    public float fireForce = 100f;
    public float propellerForce = 200f;
    public float directHitDamage = 50;

    private ParticleSystem impactEffect;

    [Header("Explosive")]
    public bool isExplosive = false;
    public float explosionRadius = 3f;
    public float explosionDamage = 10f;
    public float explosionForce = 100f;

    [Header("Autopilot")]
    public bool isAutopilot = false;
    public float seekRange = 10f;

    [SerializeField]
    private bool debug = false;

    private Rigidbody targetRigibody;
    private Rigidbody thisRigidbody;
    private int shootableMask = 9;
    private void Start()
    {
        shootableMask = LayerMask.GetMask("Shootable");
        thisRigidbody = GetComponent<Rigidbody>();
        thisRigidbody.velocity = transform.forward * fireForce;
        impactEffect = GetComponentInChildren<ParticleSystem>();
        impactEffect.Stop();
        Invoke("DestroyThisAfterLifeTime", lifeTime);

        if (isAutopilot)
            InvokeRepeating("Seek", 0f, 0.1f);
    }

    private void DestroyThisAfterLifeTime()
    {
        if (isExplosive)
        {
            Explode();
        }
        DestroyThisAfterEffectOver();
    }
    private void DestroyThisAfterEffectOver()
    {
        this.GetComponent<Light>().enabled = false;
        this.GetComponent<Bullet>().enabled = false;
        this.GetComponent<Renderer>().enabled = false;
        this.GetComponent<Collider>().enabled = false;
        this.GetComponent<Rigidbody>().isKinematic = true;
        CancelInvoke();
        Destroy(gameObject, 5f);
    }
    private void Seek()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, seekRange, shootableMask);
        if (colliders == null)
            targetRigibody = null;
        else
        {
            float shortestSqrMagnitude = Mathf.Infinity;
            foreach (Collider collider in colliders)
            {
                Health _targetHealth = collider.GetComponent<Health>();
                if (_targetHealth != null && _targetHealth.teamClass.team != TeamClass.Teams.Dead && _targetHealth.teamClass.team != team)
                {
                    float sqrMagnitudeToTarget = (transform.position - _targetHealth.transform.position).sqrMagnitude;
                    if (shortestSqrMagnitude > sqrMagnitudeToTarget)
                    {
                        shortestSqrMagnitude = sqrMagnitudeToTarget;
                        targetRigibody = _targetHealth.GetComponent<Rigidbody>();
                    }
                }
            }
        }
    }

    private void OnTriggerEnter(Collider _target)
    {
        if ((1 << _target.gameObject.layer) == shootableMask)
        {
            Rigidbody _targetRigibody = _target.GetComponent<Rigidbody>();
            Health _targetHealth = _target.GetComponent<Health>();
            if (_targetHealth != null)
                if (_targetHealth.teamClass.team == team)
                    return;
            if (_targetRigibody != null)
                DirectHit(_targetRigibody);

            DirectHit();
        }

    }

    private void DirectHit(Rigidbody _targetRigibody)
    {
        if (!isExplosive)
            impactEffect.Play();

        Health _targetHealth = _targetRigibody.GetComponent<Health>();
        if (_targetHealth != null && _targetHealth.teamClass.team != team)
            _targetHealth.TakeDamage(directHitDamage);

        if (isExplosive)
            Explode();

        DestroyThisAfterEffectOver();
    }

    private void DirectHit()
    {
        if (!isExplosive)
            impactEffect.Play();

        if (isExplosive)
            Explode();

        DestroyThisAfterEffectOver();
    }

    private void Explode()
    {

        impactEffect.transform.rotation = Quaternion.LookRotation(Vector3.up);
        impactEffect.Play();
        Vector3 explosionPos = transform.position;
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider collider in colliders)
        {
            Rigidbody targetRigidbody = collider.GetComponent<Rigidbody>();
            if (targetRigidbody != null)
                targetRigidbody.AddExplosionForce(explosionForce, explosionPos, explosionRadius, 1.0F);

            Health targetHealth = collider.GetComponent<Health>();
            if (targetHealth != null && targetHealth.teamClass.team != team)
                targetHealth.TakeDamage(explosionDamage);
        }
        DestroyThisAfterEffectOver();
    }

    private void FixedUpdate()
    {
        AddPropellerForce();
        ShowDebug();
    }

    private void AddPropellerForce()
    {
        if (propellerForce > 0f)
        {
            if (targetRigibody == null || !isAutopilot)
            {
                thisRigidbody.AddForce(transform.forward * propellerForce * 100f * Time.fixedDeltaTime, ForceMode.Force);
                return;
            }
            else
            {
                Vector3 dirWithForce = (targetRigibody.position - transform.position).normalized;
                dirWithForce = dirWithForce * propellerForce * 100f * Time.fixedDeltaTime;
                thisRigidbody.AddForce(dirWithForce, ForceMode.Force);
                transform.LookAt(transform.position + thisRigidbody.velocity);

                if (debug)//show propellerForce
                    Debug.DrawRay(transform.position, dirWithForce, Color.yellow);
            }
        }
    }

    private void ShowDebug()
    {
        if (debug)
        {
            //Self to Target
            if (targetRigibody != null)
                Debug.DrawLine(transform.position, targetRigibody.position, Color.red);

            //velocity
            Debug.DrawRay(transform.position, thisRigidbody.velocity, Color.blue);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        if (isExplosive)
            Gizmos.DrawWireSphere(transform.position, explosionRadius);

        Gizmos.color = Color.yellow;
        if (isAutopilot)
            Gizmos.DrawWireSphere(transform.position, seekRange);
    }
}
