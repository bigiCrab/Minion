using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : Control
{
    public float reviveRange = 3f;
    public float reviveGainHealthRatio = 0.2f;

    public ParticleSystem reviveParticle;

    private int floorMask;
    private int shootableMask = 9;

    public new void Start()
    {

        floorMask = LayerMask.GetMask("AimLayers");
        shootableMask = LayerMask.GetMask("Shootable");
        if (floorMask == 0)
        {
            Debug.Log("floorMask not found");
        }
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
        if (IsCantMove())
            return;
        if (Input.GetMouseButton(0))
        {
            if (weapon != null)
            {
                weapon.Shoot();
            }
        }
        if (Input.GetMouseButtonDown(1))
        {//revive
            if(reviveParticle != null){
                reviveParticle.Stop();
                reviveParticle.Play();
            }

            Collider[] colliders = Physics.OverlapSphere(transform.position, reviveRange, shootableMask);
            if (colliders != null)
            {
                foreach (Collider collider in colliders)
                {
                    Health _targetHealth = collider.GetComponent<Health>();
                    if (_targetHealth != null && _targetHealth.teamClass.team == TeamClass.Teams.Dead)
                    {
                        _targetHealth.ResetState(TeamClass.Teams.Player);
                        thisHealth.GainHealth(_targetHealth.startHealth * reviveGainHealthRatio);
                    }
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.K))
        {//suicide

            thisHealth.Death();
        }

    }

    private void FixedUpdate()
    {
        if (IsCantMove())
            return;
        Turning();
        Move();
    }

    protected override void Move()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        movement.Set(h, 0f, v);

        base.Move();
    }

    protected override void Turning()
    {
        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit floorHit;

        if (Physics.Raycast(mouseRay, out floorHit, Mathf.Infinity, floorMask))
        {
            Vector3 playerToMouse = floorHit.point - transform.position;
            playerToMouse.y = 0f;

            Quaternion newRotation = Quaternion.LookRotation(playerToMouse);
            thisRigidbody.MoveRotation(newRotation);

            #region debug
            if (debug)
            {
                Debug.DrawRay(mouseRay.origin, mouseRay.direction * floorHit.distance, Color.yellow, 3);
                Debug.DrawRay(transform.position, playerToMouse, Color.green, 3);
            }
            #endregion
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, reviveRange);
    }
}
