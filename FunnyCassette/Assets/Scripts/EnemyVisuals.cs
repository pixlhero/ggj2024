using DG.Tweening;
using UnityEngine;

public class EnemyVisuals : MonoBehaviour
{
    [SerializeField] TextPresenter textPresenter;

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

        textPresenter.PresentText(concatenatedText);

        GameManager.Singleton.EnemyFinishedTalking();
    }

    private void OnReact(bool isGood, string text)
    {
        textPresenter.PresentText("");
        
        _reactSequence?.Kill();
        _reactSequence = DOTween.Sequence();
        
        _reactSequence.InsertCallback(2f,
            () => { textPresenter.PresentText(text); });
        
        if (!isGood)
        {
            _reactSequence.InsertCallback(3f, () => { GameManager.Singleton.RegisterFailure(); });
        }
        
        _reactSequence.InsertCallback(6f, () => { textPresenter.PresentText("Ok, next joke."); });

        _reactSequence.AppendInterval(1f);
        
        _reactSequence.OnComplete(() => { GameManager.Singleton.EnemyReactionFinished(); });
    }
}