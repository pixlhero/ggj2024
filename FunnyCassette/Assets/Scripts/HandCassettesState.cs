using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class HandCassettesState : MonoBehaviour
{
    public static event Action<Cassette> CassetteAdded;
    public static event Action<List<Cassette>> CassetteAddedList;
    public static event Action<Cassette> CassetteRemoved;

    public static HandCassettesState Singleton;

    public List<Cassette> Cassettes = new();

    private void Awake()
    {
        Singleton = this;

        GameManager.PlayerTurnStarted += OnPlayerTurn;
    }

    private void OnPlayerTurn()
    {
        if (GameManager.Singleton.RoundNumber == 0)
        {
            for (int i = 0; i < 3; i++)
            {
                var newCassette = Deck.Singleton.DrawNewCassette();
                Cassettes.Add(newCassette);
            }

            CassetteAddedList?.Invoke(Cassettes);
        }
        else
        {
            var newCassette = Deck.Singleton.DrawNewCassette();
            Cassettes.Add(newCassette);
            CassetteAdded?.Invoke(newCassette);
        }
    }

    public void PlayCassette(Cassette cassette)
    {
        if (GameManager.Singleton.State != GameManager.GameState.PlayerTurn)
            return;

        Debug.Log($"Played cassette: {cassette.Type}");

        var cassetteType = cassette.Type;

        var listCassette = Cassettes.Find(listCassette => listCassette == cassette);
        if (listCassette == null)
        {
            Debug.LogError("Cassette not found in hand");
            return;
        }

        Cassettes.Remove(listCassette);
        CassetteRemoved?.Invoke(listCassette);

        GameManager.Singleton.RegisterCassetteChoice(cassetteType);
    }
}