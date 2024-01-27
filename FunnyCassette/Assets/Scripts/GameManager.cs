using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static event Action<GameState> OnGameStateChanged;
    public static event Action<int> OnRoundNumberChanged;
    public static event Action<int> OnGameLivesChanged;
    
    private int _maxRounds;
    public int RoundNumber { get; private set; } = 0;
    
    public int PlayerLives { get; private set; } = 3;
    
    public static GameManager Singleton;
    
    public GameState State { get; private set; } = GameState.Starting;
    
    public enum GameState
    {
        Starting,
        Playing,
        Ending_Good,
        Ending_Bad
    }
    
    void Awake()
    {
        Singleton = this;

        var dialog = Dialog.CreateFromJSON();
        _maxRounds = dialog.dialogs.Count;
    }

    private void Start()
    {
        State = GameState.Playing;
        OnGameStateChanged?.Invoke(State);
        OnRoundNumberChanged?.Invoke(RoundNumber);

        AudioHandler.singleton.Play_Ambiance_Drone();
    }

    public void RegisterFailure()
    {
        AudioHandler.singleton.Play_Effect_VeryBad();
        
        PlayerLives--;
        if(PlayerLives <= 0)
        {
            Debug.Log("Bad Ending");
            State = GameState.Ending_Bad;
            OnGameStateChanged?.Invoke(State);
        }
    }
    
    public void GoToNextRound()
    {
        if(RoundNumber + 1 >= _maxRounds)
        {
            Debug.Log("Good Ending");
            State = GameState.Ending_Good;
            OnGameStateChanged?.Invoke(State);
            return;
        }
        
        RoundNumber++;
        OnRoundNumberChanged?.Invoke(RoundNumber);
    }
}
