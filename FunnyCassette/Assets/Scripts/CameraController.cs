using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cinemachine;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float mouseInfluence;
    [SerializeField] private CinemachineVirtualCamera mainVirtualCamera;

    [SerializeField] private Light tableLight;
    private Quaternion _initialCameraRotation;

    private void Awake()
    {
        _initialCameraRotation = mainVirtualCamera.transform.rotation;
    }

    private void Start()
    {
        GameManager.LivesChanged += OnLiveChanged;
    }

    // Update is called once per frame
    void Update()
    {
        var mousePos = Input.mousePosition;
        var normalizedMousePos = Camera.main.ScreenToViewportPoint(mousePos) - (Vector3.one * 0.5f);
        var angleInfluence = normalizedMousePos * mouseInfluence;
        var newCameraRotation = Quaternion.Euler(-angleInfluence.y, angleInfluence.x, 0);
        mainVirtualCamera.transform.rotation = _initialCameraRotation * newCameraRotation;
    }

    private void OnLiveChanged(int lives)
    {
        var intensity = tableLight.intensity;
        tableLight.DOIntensity(intensity * 0.7f, 0.05f)
            .SetLoops(6, LoopType.Yoyo)
            .OnComplete(() => tableLight.intensity = intensity);
    }
}