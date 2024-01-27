using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioHandler : MonoBehaviour
{
    [SerializeField]
    private AudioSource effects_veryBad;

    public static AudioHandler singleton;

    private void Awake()
    {
        singleton = this;
    }

    public void playVeryBad()
    {
        effects_veryBad.Play();
    }
}
