using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // state events
    public static event Action StartingStarted;
    public static event Action<DialogPhrase> EnemyTalksStarted;
    public static event Action PlayerTurnStarted;
    public static event EnemyReacted EnemyReactionStarted;
    public delegate void EnemyReacted(bool isGood, string text);
    
    public static event Action EndingGoodStarted;
    public static event Action EndingBadStarted;

    // other events
    public static event Action<int> OnRoundNumberChanged;
    public static event Action<int> OnGameLivesChanged;

    private int _maxRounds;
    public int RoundNumber { get; private set; } = 0;

    public int PlayerLives { get; private set; } = 3;

    public static GameManager Singleton;

    public GameState State { get; private set; } = GameState.Starting;

    public DialogPhrase CurrentDialogPhrase => _dialog.dialogs[RoundNumber];
    private Dialog _dialog;
    
    public enum GameState
    {
        Starting,
        EnemyTalks,
        PlayerTurn,
        EnemyReaction,
        Ending_Good,
        Ending_Bad
    }

    void Awake()
    {
        Singleton = this;

        _dialog = Dialog.CreateFromJSON();
        _maxRounds = _dialog.dialogs.Count;
    }

    private void Start()
    {
        InitializeAudio();
        
        State = GameState.EnemyTalks;
        OnRoundNumberChanged?.Invoke(RoundNumber);
        EnemyTalksStarted?.Invoke(CurrentDialogPhrase);
    }

    public void EnemyFinishedTalking()
    {
        State = GameState.PlayerTurn;
        PlayerTurnStarted?.Invoke();
    }

    public void RegisterCassetteChoice(Cassette.CassetteType type)
    {
        var correct = CurrentDialogPhrase.IsCorrectOption(type);
        State = GameState.EnemyReaction;
        var reactionText = correct ? CurrentDialogPhrase.goodReaction : CurrentDialogPhrase.badReaction;
        EnemyReactionStarted?.Invoke(correct, reactionText);
    }

    public void RegisterFailure()
    {
        AudioHandler.singleton.Play_Effect_VeryBad();
        PlayerLives--;
        if (PlayerLives <= 0)
        {
            Debug.Log("Bad Ending");
            State = GameState.Ending_Bad;
            EndingBadStarted?.Invoke();
        }
    }

    public void EnemyReactionFinished()
    {
        // go to next round

        // check if was last round
        if (State != GameState.Ending_Bad && RoundNumber + 1 >= _maxRounds)
        {
            Debug.Log("Good Ending");
            State = GameState.Ending_Good;
            EndingGoodStarted?.Invoke();
            return;
        }

        State = GameState.EnemyTalks;
        RoundNumber++;
        OnRoundNumberChanged?.Invoke(RoundNumber);
        EnemyTalksStarted?.Invoke(CurrentDialogPhrase);
    }

    private void InitializeAudio() {
        AudioHandler.singleton.Play_Ambiance_Drone();
        StartCoroutine(AudioPlayerManager.singleton.PlayHeartbeatRandomly());
        StartCoroutine(AudioPlayerManager.singleton.PlayKnockingRandomly());
    }
}