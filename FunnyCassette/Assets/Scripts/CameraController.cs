using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float mouseInfluence;
    [SerializeField] private CinemachineVirtualCamera mainVirtualCamera;
    
    private Quaternion _initialCameraRotation;

    private void Awake()
    {
        _initialCameraRotation = mainVirtualCamera.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        var mousePos = Input.mousePosition;
        var normalizedMousePos = Camera.main.ScreenToViewportPoint(mousePos);
        var angleInfluence = normalizedMousePos * mouseInfluence;
        var newCameraRotation = Quaternion.Euler(-angleInfluence.y, angleInfluence.x, 0);
        mainVirtualCamera.transform.rotation = _initialCameraRotation * newCameraRotation;
    }
}
