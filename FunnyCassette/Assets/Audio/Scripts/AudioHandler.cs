
using System.Collections.Generic;
using UnityEngine;

public class AudioHandler : MonoBehaviour
{
    [SerializeField]
    private AudioSource ambiance_drone;

    [SerializeField]
    private AudioSource ambiance_vinyl;

    [SerializeField]
    private AudioSource effects_knocking;

    [SerializeField]
    private AudioSource effects_heartbeat;

    [SerializeField]
    private AudioSource effects_veryBad;

    [SerializeField]
    private AudioSource effects_childLaugh;

    [SerializeField]
    private AudioSource effects_bonk;

    [SerializeField]
    private AudioSource effects_putinCassette;

    [SerializeField]
    private AudioSource effects_dropCassette;

    private AudioSource shared_source;
    private AudioClip current_clip;
    private List<AudioSource> all_sources = new List<AudioSource>();
    public float minWaitBetweenPlays = 10f;
    public float maxWaitBetweenPlays = 50f;
    public float waitTimeCountdown = -1f;

    public static AudioHandler singleton;

    private void Awake()
    {
        singleton = this;
        shared_source = this.gameObject.AddComponent<AudioSource>();

        // Sorry no time LULZ
        all_sources.Add(effects_knocking);
        all_sources.Add(effects_childLaugh);
        all_sources.Add(effects_heartbeat);
    }

    void Update()
    {
        // Randomly play audio effects while the game is running
        if (!shared_source.isPlaying)
        {
            if (waitTimeCountdown < 0f)
            {
                shared_source = all_sources[Random.Range(0, all_sources.Count)];
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

    public void Play_Effect_Bonk()
    {
        effects_bonk.Play();
    }

    public void Play_Effect_PutinCassette()
    {
        effects_putinCassette.Play();
    }

    public void Play_Effect_DropCassette()
    {
        effects_dropCassette.pitch = 1f;
        effects_dropCassette.Play();
    }

    public void Play_Effect_LiftCassette()
    {
        // ToDo: Create different audio
        effects_dropCassette.pitch = 2f;
        effects_dropCassette.Play();
    }

    /*
        Ambiance and Music Audio Effects
    */
    public void Play_Ambiance_Drone()
    {
        if (ambiance_drone.isPlaying == true) return;
        
        ambiance_drone.Play();
    }

    public void Stop_Ambiance_Drone()
    {
        if (ambiance_drone.isPlaying == true) ambiance_drone.Stop();
    }

    public void Play_Ambiance_Vinyl()
    {
        if (ambiance_vinyl.isPlaying == true) return;
        
        ambiance_vinyl.Play();
    }

    public void Stop_Ambiance_Vinyl()
    {
        if (ambiance_vinyl.isPlaying == true) ambiance_vinyl.Stop();
    }
}
