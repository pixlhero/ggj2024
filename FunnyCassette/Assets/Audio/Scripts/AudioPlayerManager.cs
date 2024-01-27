using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayerManager : MonoBehaviour
{
    public static AudioPlayerManager singleton;

    private void Awake()
    {
        singleton = this;
    }

    public IEnumerator PlayChildLaughRandomly()
    {
        yield return new WaitForSeconds(5);
        Debug.Log("Now Playing ChildLaugh Sound");
        AudioHandler.singleton.Play_Effect_ChildLaugh();
    }

    public IEnumerator PlayHeartbeatRandomly()
    {
        yield return new WaitForSeconds(15);
        Debug.Log("Now Playing Heartbeat Sound");
        AudioHandler.singleton.Play_Effect_Heartbeat();
    }

    public IEnumerator PlayKnockingRandomly()
    {
        yield return new WaitForSeconds(25);
        Debug.Log("Now Playing Knocking Sound");
        AudioHandler.singleton.Play_Effect_Knocking();
    }


}
