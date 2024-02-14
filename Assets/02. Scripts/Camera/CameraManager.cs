using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // 처음에는 FPS       
        gameObject.GetComponent<FPSCamera>().enabled = true;
        gameObject.GetComponent<TPSCamera>().enabled = false;
    }

    public void FPSCam()
    {
        gameObject.GetComponent<FPSCamera>().enabled = true;
        gameObject.GetComponent<TPSCamera>().enabled = false;
    }
    public void TPSCam()
    {
        gameObject.GetComponent<FPSCamera>().enabled = false;
        gameObject.GetComponent<TPSCamera>().enabled = true;
    }
}
