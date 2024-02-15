using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapCamera : MonoBehaviour
{
    public Transform Target;
    public float YDistance = 20f;
    private Vector3 _initalEulerAngles;
    bool isLock = false;

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

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isLock = !isLock;
        }
        if(isLock)
            transform.rotation = Quaternion.Euler(90,0,0) ;
    }
}
