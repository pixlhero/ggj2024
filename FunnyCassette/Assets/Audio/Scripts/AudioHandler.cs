using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioHandler : MonoBehaviour
{
    [SerializeField]
    private AudioSource effects_veryBad;

    [SerializeField]
    private AudioSource ambiance_drone;

    public static AudioHandler singleton;

    private void Awake()
    {
        singleton = this;
    }

    public void Play_Effect_VeryBad()
    {
        effects_veryBad.Play();
    }

    public void Play_Ambiance_Drone()
    {
        ambiance_drone.Play();
    }

    public void Stop_Ambiance_Drone()
    {
        ambiance_drone.Stop();
    }
}
