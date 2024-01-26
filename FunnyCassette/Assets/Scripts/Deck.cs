using System;
using UnityEngine;

public class Deck : MonoBehaviour
{
    [SerializeField] private Cassette testCassettePrefab;
    
    public static Deck Singleton;

    private void Awake()
    {
        Singleton = this;
    }

    public Cassette DrawNewCassette()
    {
        var newCassette = Instantiate(testCassettePrefab);
        return newCassette;
    }
}