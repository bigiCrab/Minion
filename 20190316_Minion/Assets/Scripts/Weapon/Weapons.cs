using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapons : MonoBehaviour
{

    [Header("General")]
    public float range = 15f;
    public Transform firePoint;
    public Light fireLight;
    public float effectsShowTime = 0.05f;
    public float destoryIdleWeaponTime = 10f;

    [Header("Use Bullets (default)")]
    public GameObject bulletPrefab;
    public float projectile_fireRatePerSec = 1f;
    private float Projectile_fireCoolDown = 0f;

    [Header("nonProjectile General")]
    public bool nonProjectile = false;
    public LineRenderer lineRenderer;

    [Header("Use nonProjectile")]
    public float nonProjectile_damage = 30;
    public float nonProjectile_fireRatePerSec = 1;
    private float nonProjectile_fireCoolDown = 0;


    [Header("Use nonProjectile_Lazer")]
    public bool lazer = false;
    public float lazer_damageOverTime = 0;
    public float lazer_slowRatio = 0.5f;
    public float lazer_startCapacity = 5f;
    public float lazer_startCoolDown = 1f;
    private Light laserImpactLight;
    private ParticleSystem laserImpactEffect;
    private float lazer_capacity = 5f;
    private float lazer_coolDown = 1f;


    [HideInInspector]
    public TeamClass.Teams team;
    [HideInInspector]
    public bool isIdle=true;

    private Health targetHealth;
    private Control targetControl;
    private float timer;

    private float disableEffectsCountDown=0;
    private int shootableMask;
    private Ray shootRay;
    private RaycastHit shootHit;



    void Awake()
    {
        if (lazer)
        {
            nonProjectile = true;
            lazer_capacity = lazer_startCapacity;
            lazer_coolDown = lazer_startCoolDown;
            laserImpactLight = GetComponentInChildren<Light>();
            laserImpactEffect = GetComponentInChildren<ParticleSystem>();
        }
        DisableEffects();
        shootableMask = LayerMask.GetMask("Shootable");
        Invoke("DestoryIdleWeapon", destoryIdleWeaponTime);
        timer = Time.time;
    }

    private void Start()
    {
        GameManager.AddAndUpdateWeaponCount(1);
    }
    private void OnDestroy()
    {
        GameManager.AddAndUpdateWeaponCount(-1);
    }

    private void DestoryIdleWeapon()
    {
        if (isIdle)
            Destroy(this.gameObject);
    }

    public void Shoot()
    {
        if (nonProjectile)
        {
            if (lazer)
            {
                if (lazer_capacity > 0f)
                {
                    Shoot_nonProjectile_lazer();
                    lazer_capacity -= Time.deltaTime;
                    if (lazer_capacity <= 0f)
                        timer = Time.time;
                }
                else
                {
                    lazer_coolDown -= Time.time - timer;
                    if (lazer_coolDown <= 0)
                    {
                        lazer_capacity = lazer_startCapacity;
                        lazer_coolDown = lazer_startCoolDown;
                    }
                    timer = Time.time;
                }

            }
            else
            {
                nonProjectile_fireCoolDown -= Time.time - timer;
                timer = Time.time;
                if (nonProjectile_fireCoolDown <= 0)
                {
                    Shoot_nonProjectile();
                    nonProjectile_fireCoolDown = 1f / nonProjectile_fireRatePerSec;

                }
            }
        }
        else
        {
            Projectile_fireCoolDown -= Time.time - timer;
            timer = Time.time;
            if (Projectile_fireCoolDown <= 0)
            {
                Shoot_Projectile();
                Projectile_fireCoolDown = 1f / projectile_fireRatePerSec;
            }
        }
    }

    public void DisableEffects()
    {
        disableEffectsCountDown -= Time.deltaTime;
        if (disableEffectsCountDown <= 0f)
        {
            fireLight.enabled = false;
            if (nonProjectile)
            {
                lineRenderer.enabled = false;

                if (lazer)
                {
                    laserImpactEffect.Stop();
                    laserImpactLight.enabled = false;
                }
            }
        }
    }

    void Shoot_nonProjectile()
    {
        fireLight.enabled = true;
        lineRenderer.enabled = true;
        lineRenderer.SetPosition(0, firePoint.position);

        shootRay.origin = firePoint.position;
        shootRay.direction = firePoint.forward;

        if (Physics.Raycast(shootRay, out shootHit, range, shootableMask))
        {
            targetHealth = shootHit.collider.GetComponent<Health>();
            if (targetHealth != null&&targetHealth.teamClass.team!=team)
            {
                targetHealth.TakeDamage(nonProjectile_damage);
            }
            lineRenderer.SetPosition(1, shootHit.point);
        }
        else
        {
            lineRenderer.SetPosition(1, shootRay.origin + shootRay.direction * range);
        }

        disableEffectsCountDown = effectsShowTime;
    }
    void Shoot_nonProjectile_lazer()
    {
        lineRenderer.SetPosition(0, firePoint.position);

        shootRay.origin = firePoint.position;
        shootRay.direction = firePoint.forward;

        if (Physics.Raycast(shootRay, out shootHit, range, shootableMask))
        {
            targetHealth = shootHit.collider.GetComponent<Health>();
            if (targetHealth != null && targetHealth.teamClass.team != team)
            {
                targetHealth.TakeDamage(lazer_damageOverTime * Time.deltaTime);
                targetControl = shootHit.collider.GetComponent<Control>();
                if(targetControl!=null)
                targetControl.SpeedEffectRatio(lazer_slowRatio,1);
            }
            laserImpactEffect.transform.position = shootHit.point;
            if (!lineRenderer.enabled)
            {
                lineRenderer.enabled = true;
                laserImpactLight.enabled = true;
                fireLight.enabled = true;
            }
            if (!laserImpactEffect.isPlaying)
                laserImpactEffect.Play();
            lineRenderer.SetPosition(1, shootHit.point);
        }
        else
        {
            if (!lineRenderer.enabled)
            {
                lineRenderer.enabled = true;
                laserImpactLight.enabled = true;
                fireLight.enabled = true;
            }
            if (laserImpactEffect.isPlaying)
            {
                laserImpactEffect.Stop();
            }

            laserImpactEffect.transform.position = shootRay.origin + shootRay.direction * range;
            lineRenderer.SetPosition(1, shootRay.origin + shootRay.direction * range);
        }
        disableEffectsCountDown = effectsShowTime;
    }
    void Shoot_Projectile()
    {
        GameObject BulletGO=Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        BulletGO.GetComponent<Bullet>().team = team;
        fireLight.enabled = true;

    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        if (nonProjectile)
            Gizmos.DrawWireSphere(transform.position, range);
    }
}
