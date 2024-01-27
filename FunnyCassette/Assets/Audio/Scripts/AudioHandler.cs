using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioHandler : MonoBehaviour
{
    [SerializeField]
    private AudioSource ambiance_drone;

    [SerializeField]
    private AudioSource effects_knocking;

    [SerializeField]
    private AudioSource effects_heartbeat;

    [SerializeField]
    private AudioSource effects_veryBad;

    public static AudioHandler singleton;

    private void Awake()
    {
        singleton = this;
    }

    /*
        Effects when something happens
    */
    public void Play_Effect_VeryBad()
    {
        effects_veryBad.Play();
    }

    public void Play_Effect_Heartbeat()
    {
        effects_heartbeat.Play();
    }

    public void Play_Effect_Knocking()
    {
        effects_knocking.Play();
    }

    /*
        Ambiance and Music Audio Effects
    */
    public void Play_Ambiance_Drone()
    {
        ambiance_drone.Play();
    }

    public void Stop_Ambiance_Drone()
    {
        ambiance_drone.Stop();
    }
}
