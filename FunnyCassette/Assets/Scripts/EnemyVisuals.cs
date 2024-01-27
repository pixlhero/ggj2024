using DG.Tweening;
using UnityEngine;

public class EnemyVisuals : MonoBehaviour
{
    [SerializeField] TMPro.TMP_Text shownText;

    private const string invisibleTag = "<color=#0000>";

    private Sequence _reactSequence;

    private void Awake()
    {
        GameManager.EnemyTalksStarted += OnEnemeyTalksState;
        GameManager.EnemyReactionStarted += OnReact;
    }

    private void OnEnemeyTalksState(DialogPhrase newPhrase)
    {
        var concatenatedText = "";
        foreach (var text in newPhrase.text)
        {
            concatenatedText += text + " ";
        }

        shownText.text = concatenatedText;

        GameManager.Singleton.EnemyFinishedTalking();
    }

    private void OnReact(bool isGood, string text)
    {
        shownText.text = "";
        _reactSequence?.Kill();
        _reactSequence = DOTween.Sequence();
        
        _reactSequence.InsertCallback(2f,
            () => { shownText.text = text; });
        
        _reactSequence.InsertCallback(4f, () => { GameManager.Singleton.RegisterFailure(); });
        
        _reactSequence.InsertCallback(6f, () => { shownText.text = "Ok, next joke."; });

        _reactSequence.AppendInterval(1f);
        
        _reactSequence.OnComplete(() => { GameManager.Singleton.EnemyReactionFinished(); });
    }
}