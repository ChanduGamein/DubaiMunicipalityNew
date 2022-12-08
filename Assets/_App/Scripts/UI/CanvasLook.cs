using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasLook : MonoBehaviour
{
    private GameObject FirstPersonCamera;

    void Start()
    {
        FirstPersonCamera = GameObject.FindGameObjectWithTag("MainCamera");

    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.LookAt(transform.position + FirstPersonCamera.transform.rotation * Vector3.forward, FirstPersonCamera.transform.rotation * Vector3.up);

    }
}
