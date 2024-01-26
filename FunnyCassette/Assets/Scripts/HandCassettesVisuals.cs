using System;
using UnityEngine;

public class HandCassettesVisuals : MonoBehaviour
{
    [SerializeField] private float cassettesDistance;
    
    private void Awake()
    {
        HandCassettesState.CassetteAdded += OnCassetteAdded;
        HandCassettesState.CassetteRemoved += OnCassetteRemoved;
    }
    
    private void OnCassetteAdded(Cassette newCassette)
    {
        newCassette.transform.SetParent(transform);
        SetPositions();
    }

    private void OnCassetteRemoved(Cassette cassette)
    {
        Destroy(cassette);
        SetPositions();
    }

    private void SetPositions()
    {
        var cassettesCount = HandCassettesState.Singleton.Cassettes.Count;
        var leftOffset = cassettesDistance * (cassettesCount - 1f) / 2f;

        for(int i= 0; i< cassettesCount; i++)
        {
            var cassette = HandCassettesState.Singleton.Cassettes[i];
            cassette.transform.localPosition = new Vector3(i * cassettesDistance - leftOffset, 0f, 0f);
        }
    }
}