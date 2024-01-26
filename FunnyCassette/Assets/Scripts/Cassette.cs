using System;
using Unity.VisualScripting;
using UnityEngine;

public class Cassette : MonoBehaviour
{
    public enum CassetteType
    {
        DotDotDot,
        Haha,
        Cry
    }
    
    public CassetteType Type { get; private set; }

    private void OnMouseDown()
    {
        HandCassettesState.Singleton.PlayCassette(this);
    }
}