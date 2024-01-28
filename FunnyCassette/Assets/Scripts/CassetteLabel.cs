using System;
using UnityEditor.Rendering;
using UnityEngine;

public class CassetteLabel: MonoBehaviour
{
    public Cassette.CassetteType type;
    public string text;
    public Material material;
    
    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void Play()
    {
        if(_audioSource == null)
            return;
        
        Debug.Log($"playing {gameObject.name}");
        _audioSource.Play();
    }
}