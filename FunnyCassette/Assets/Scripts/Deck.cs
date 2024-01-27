using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Deck : MonoBehaviour
{
    [SerializeField] private Cassette cassettePrefab;
    
    public static Deck Singleton;

    private List<CassetteLabel> _cassetteLabels;

    private void Awake()
    {
        Singleton = this;
        
        _cassetteLabels = new List<CassetteLabel>(GetComponentsInChildren<CassetteLabel>());
    }

    public Cassette DrawNewCassette()
    {
        transform.DOShakeRotation(0.3f, new Vector3(0, 10, 0), 20, 90F, false);
        
        var type = GetRandomCassetteType();
        var newCassette = InstantiatePrefab(type);
        return newCassette;
    }

    public List<Cassette> GetStarterCassettes()
    {
        var hahaCassette = InstantiatePrefab(Cassette.CassetteType.Haha);
        var ohNoCassette = InstantiatePrefab(Cassette.CassetteType.OhNo);
        var dotDotDotCassette = InstantiatePrefab(Cassette.CassetteType.DotDotDot);
        
        return new List<Cassette>
        {
            hahaCassette,
            ohNoCassette,
            dotDotDotCassette
        };
    }

    private Cassette InstantiatePrefab(Cassette.CassetteType type)
    {
        var newInstance = Instantiate(cassettePrefab);
        var typeData = GetRandomTypeData(type);
        newInstance.SetTypeData(typeData);
        return newInstance;
    }
    
    private Cassette.CassetteType GetRandomCassetteType()
    {
        var cassetteTypes = Enum.GetValues(typeof(Cassette.CassetteType));
        var random = UnityEngine.Random.Range(0, cassetteTypes.Length);
        return (Cassette.CassetteType) cassetteTypes.GetValue(random);
    }
    
    private CassetteLabel GetRandomTypeData(Cassette.CassetteType type)
    {
        var cassetteLabels = _cassetteLabels.FindAll(label => label.type == type);
        var random = UnityEngine.Random.Range(0, cassetteLabels.Count);
        return cassetteLabels[random];
    }
}