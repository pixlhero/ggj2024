using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class HandCassettesState : MonoBehaviour
{
    public static event Action<Cassette> CassetteAdded;
    public static event Action<Cassette> CassetteRemoved;
    
    public static HandCassettesState Singleton;
    
    public List<Cassette> Cassettes = new();
    
    private void Awake()
    {
        Singleton = this;
        
        GameManager.OnRoundNumberChanged += OnRoundNumberChanged;
        GameManager.OnGameStateChanged += OnGameStateChanged;
    }
    
    private void OnGameStateChanged(GameManager.GameState newState)
    {
        if(newState == GameManager.GameState.Playing)
        {
            for(int i= 0; i< 3; i++)
            {
                var newCassette = Deck.Singleton.DrawNewCassette();
                Cassettes.Add(newCassette);
                CassetteAdded?.Invoke(newCassette);
            }
        }
    }
    
    public void OnRoundNumberChanged(int roundNumber)
    {
        var newCassette = Deck.Singleton.DrawNewCassette();
        Cassettes.Add(newCassette);
        CassetteAdded?.Invoke(newCassette);
    }

    public void PlayCassette(Cassette cassette)
    {
        Debug.Log($"Played cassette: {cassette.Type}");
        
        var cassetteType = cassette.Type;
        
        var listCassette = Cassettes.Find(listCassette => listCassette == cassette);
        if(listCassette == null)
        {
            Debug.LogError("Cassette not found in hand");
            return;
        }
        
        Cassettes.Remove(listCassette);
        CassetteRemoved?.Invoke(listCassette);
        
        EnemyState.Singleton.ChooseCassetteType(cassetteType);
    }
}
