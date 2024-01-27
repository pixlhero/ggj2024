using DG.Tweening;
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

    public Sequence Sequence;

    private void OnMouseDown()
    {
        HandCassettesState.Singleton.PlayCassette(this);
    }
}