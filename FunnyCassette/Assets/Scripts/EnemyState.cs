using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;

public class EnemyState :MonoBehaviour
{
    public static EnemyState Singleton;
    
    public DialogPhrase CurrentDialogPhrase => _dialog.dialogs[_currentDialogPhraseIndex];
    private int _currentDialogPhraseIndex = 0;

    private Dialog _dialog;
    
    private void Awake()
    {
        Singleton = this;
        
        _dialog = Dialog.CreateFromJSON();
        
        GameManager.OnGameStateChanged += OnGameStateChanged;
        GameManager.OnRoundNumberChanged += OnRoundNumberChanged;
    }

    private void OnGameStateChanged(GameManager.GameState gameState)
    {
        switch (gameState)
        {
            case GameManager.GameState.Starting:
                break;
            case GameManager.GameState.Playing:
                break;
            case GameManager.GameState.Ending_Bad:
                break;
            case GameManager.GameState.Ending_Good:
                break;
        }
    }
    
    private void OnRoundNumberChanged(int roundNumber)
    {
        _currentDialogPhraseIndex = roundNumber;
    }

    public void ChooseCassetteType(Cassette.CassetteType type)
    {
        var correct = CurrentDialogPhrase.IsCorrectOption(type);
        if (correct)
        {
            GameManager.Singleton.GoToNextRound();
        }
        else
        {
            GameManager.Singleton.RegisterFailure();
            GameManager.Singleton.GoToNextRound();
        }
    }
}