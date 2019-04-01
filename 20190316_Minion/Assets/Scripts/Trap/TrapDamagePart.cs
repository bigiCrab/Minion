using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapDamagePart : MonoBehaviour
{
    public float hitForce = 500f;
    public float hitForce_Torque = 500f;
    public float stunnedDuration = 1.5f;
    public float trapDamageAmount=50f;
    [SerializeField]
    private bool debug = false;

    

    private void OnTriggerEnter(Collider target)
    {
        if (target.isTrigger)
            return;
        CheckHasRigibodyAndAddHitForce(target);
        CheckHasPlayerMovementAndAddStunned(target);
        CheckHasHealthAndAddHitDamage(target);
        ShowDebug(target);

    }

    private void CheckHasRigibodyAndAddHitForce(Collider target)
    {
        Rigidbody targetRB = target.GetComponent<Rigidbody>();
        if (targetRB != null)
        {
            targetRB.AddForce((target.transform.position - this.transform.position) * hitForce);
            targetRB.AddTorque(-transform.right * hitForce_Torque, ForceMode.Impulse);
        }
    }

    private void CheckHasHealthAndAddHitDamage(Collider target)
    {
        Health targetHealth = target.GetComponent<Health>();
        if (targetHealth != null)
        {
            targetHealth.TakeDamage(trapDamageAmount);
        }
    }
    
    private void CheckHasPlayerMovementAndAddStunned(Collider target)
    {
        Control targetControl = target.GetComponent<Control>();
        if (targetControl != null)
        {
            targetControl.Stunned(stunnedDuration);
        }
    }
    private void ShowDebug(Collider target)
    {
        if (debug)
        {
            Debug.Log("Trap: "+this.GetComponentInParent<Transform>().parent.name+"\tCollider with: "+target.gameObject+ target.transform.position);
            Debug.DrawRay(transform.position, target.transform.position- transform.position, Color.red, 3);
        }
    }
}
