using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smoothing = 5f;

    [SerializeField]
    private Vector3 offset;
    private GameObject floor;

    private void Start()
    {
        if(target==null)
        target=GameObject.FindGameObjectWithTag("Player").transform;

        floor = GameObject.FindGameObjectWithTag("Floor");
    }

    private void FixedUpdate()
    {
        if (target==null)
            return;

        FollowTarget();
        MoveFloor();
    }

    private void FollowTarget()
    {
        Vector3 targetCamPos = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, targetCamPos, smoothing * Time.deltaTime);
    }
    private void MoveFloor()
    {
        Vector3 movePoint = transform.position;
        movePoint.y = 0f;
        floor.transform.position = movePoint;
    }
}
