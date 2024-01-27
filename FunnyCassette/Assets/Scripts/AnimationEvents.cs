using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvents : MonoBehaviour
{
    public static event Action OnHitEvent;
    
    public void HitEvent()
    {
        OnHitEvent?.Invoke();
    }
}
