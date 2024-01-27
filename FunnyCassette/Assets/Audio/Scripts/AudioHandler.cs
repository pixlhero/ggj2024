using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
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

    [SerializeField]
    private AudioSource effects_childLaugh;

    private AudioSource shared_source;
    private AudioClip current_clip;
    private List<AudioClip> all_clips = new List<AudioClip>();
    public float minWaitBetweenPlays = 10f;
    public float maxWaitBetweenPlays = 50f;
    public float waitTimeCountdown = -1f;

    public static AudioHandler singleton;

    private void Awake()
    {
        singleton = this;
        shared_source = this.gameObject.AddComponent<AudioSource>();

        // Sorry no time LULZ
        all_clips.Add(effects_knocking.clip);
        all_clips.Add(effects_childLaugh.clip);
        all_clips.Add(effects_heartbeat.clip);
    }

    void Update()
    {
        // Randomly play audio effects while the game is running
        if (!shared_source.isPlaying)
        {
            if (waitTimeCountdown < 0f)
            {
                current_clip = all_clips[Random.Range(0, all_clips.Count)];
                shared_source.clip = current_clip;
                shared_source.Play();
                waitTimeCountdown = Random.Range(minWaitBetweenPlays, maxWaitBetweenPlays);
            }
            else
            {
                waitTimeCountdown -= Time.deltaTime;
            }
        }
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

    public void Play_Effect_ChildLaugh()
    {
        effects_childLaugh.Play();
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
