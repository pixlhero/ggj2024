using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class AnimationEvents : MonoBehaviour
{
    public static event Action OnHitEvent;

    private CinemachineImpulseSource _impulse;

    private void Awake()
    {
        _impulse = GetComponent<CinemachineImpulseSource>();
    }

    public void HitEvent()
    {
        OnHitEvent?.Invoke();
    }

    public void TantrumHit()
    {
        AudioHandler.singleton.Play_Effect_Bonk();
        _impulse.GenerateImpulse();
    }
}
