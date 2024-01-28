using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // state events
    public static event Action StartingStarted;
    public static event Action<DialogPhrase> EnemyTalksStarted;
    public static event Action PlayerTurnStarted;
    public static event EnemyReacted EnemyReactionStarted;
    public delegate void EnemyReacted(bool isGood, string text, string nextText);

    public static event Action EndingGoodStarted;
    public static event Action EndingBadStarted;

    // other events
    public static event Action<int> OnRoundNumberChanged;
    public static event Action<int> LivesChanged;

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

    [SerializeField] private CinemachineImpulseSource cinemachineImpulseSource;

    void Awake()
    {
        Singleton = this;

        _dialog = Dialog.CreateFromJSON();
        _maxRounds = _dialog.dialogs.Count;
    }

    private void Start()
    {
        InitializeAudio();

        State = GameState.Starting;
        /*
        OnRoundNumberChanged?.Invoke(RoundNumber);
        EnemyTalksStarted?.Invoke(CurrentDialogPhrase);
        */
        
        StartingStarted?.Invoke();
    }

    public void StartGame()
    {
        State = GameState.EnemyTalks;
        OnRoundNumberChanged?.Invoke(RoundNumber);
        EnemyTalksStarted?.Invoke(CurrentDialogPhrase);
    }

    public void EnemyFinishedTalking()
    {
        State = GameState.PlayerTurn;
        PlayerTurnStarted?.Invoke();
    }

    public void RegisterCassetteChoice(Cassette cassette)
    {
        if (State == GameState.Starting)
        {
            IntroController.Singleton.ChoseCassette(cassette);
            return;
        }
        
        var correct = CurrentDialogPhrase.IsCorrectOption(cassette.Type);
        State = GameState.EnemyReaction;
        var reactionText = correct ? CurrentDialogPhrase.goodReaction : CurrentDialogPhrase.badReaction;
        EnemyReactionStarted?.Invoke(correct, reactionText, CurrentDialogPhrase.next);
    }

    public void RegisterFailure()
    {
        AudioHandler.singleton.Play_Effect_VeryBad();
        AudioHandler.singleton.Play_Effect_Bonk();
        cinemachineImpulseSource.GenerateImpulse();
        AudioHandler.singleton.Stop_Ambiance_Drone();

        PlayerLives--;

        LivesChanged?.Invoke(PlayerLives);

        if (PlayerLives <= 0)
        {
            State = GameState.Ending_Bad;
            Debug.Log("Bad Ending");
        }
    }

    public void EnemyReactionFinished()
    {
        // go to next round
        if(State == GameState.Ending_Bad){
            EndingBadStarted?.Invoke();
            return;}
            

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

    private void InitializeAudio()
    {
        AudioHandler.singleton.Play_Ambiance_Drone();
        AudioHandler.singleton.Play_Ambiance_Vinyl();
    }

    public static void Reset()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }
}