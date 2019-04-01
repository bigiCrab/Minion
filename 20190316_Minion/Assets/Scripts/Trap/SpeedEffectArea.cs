using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedEffectArea : MonoBehaviour
{
    PlayerControl targetMovement;
    public float speedEffectRatio = 0.3f;
    public const float originalSpeedRatio = 1;
    private void OnTriggerEnter(Collider target)
    {
        if (target.isTrigger)
            return;
        targetMovement = target.GetComponent<PlayerControl>();
        if (targetMovement != null)
        {
            targetMovement.SpeedEffectRatio(speedEffectRatio);
        }
    }

    private void OnTriggerExit(Collider target)
    {
        if (target.isTrigger)
            return;
        targetMovement = target.GetComponent<PlayerControl>();
        if (targetMovement != null)
        {
            targetMovement.SpeedEffectRatio(originalSpeedRatio);
            targetMovement = null;
        }
    }

    private void OnDestroy()
    {
        if (targetMovement != null)
        {
            targetMovement.SpeedEffectRatio(originalSpeedRatio);
        }
    }
}
