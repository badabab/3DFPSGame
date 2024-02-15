using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapCamera : MonoBehaviour
{
    public Transform Target;
    public float YDistance = 20f;
    private Vector3 _initalEulerAngles;

    private void Start()
    {
        _initalEulerAngles = transform.eulerAngles;
    }

    private void LateUpdate()
    {
        Vector3 targetPosition = Target.position;
        targetPosition.y += YDistance;
        transform.position = targetPosition;

        Vector3 targetEulerAngles = Target.eulerAngles;
        targetEulerAngles.x = _initalEulerAngles.x;
        targetEulerAngles.z = _initalEulerAngles.z;
        transform.eulerAngles = targetEulerAngles;
    }
}
