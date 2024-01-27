using Cinemachine;
using DG.Tweening;
using UnityEngine;

public class EnemyVisuals : MonoBehaviour
{
    [SerializeField] TextPresenter textPresenter;

    [SerializeField] private Animator _animator;

    private const string invisibleTag = "<color=#0000>";

    private Sequence _reactSequence;

    private void Awake()
    {
        GameManager.EnemyTalksStarted += OnEnemeyTalksState;
        GameManager.EnemyReactionStarted += OnReact;

        
    }

    private void OnEnemeyTalksState(DialogPhrase newPhrase)
    {
        _animator.SetBool("isReading", true);
        var concatenatedText = "";
        foreach (var text in newPhrase.text)
        {
            concatenatedText += text + " ";
        }

        textPresenter.PresentText(concatenatedText);
        
        GameManager.Singleton.EnemyFinishedTalking();
        _reactSequence = DOTween.Sequence();
        _reactSequence.AppendInterval(textPresenter.CalculateSpeechTime(concatenatedText));
        _reactSequence.AppendCallback(() => { _animator.SetBool("isReading", false); });
    }

    private void OnReact(bool isGood, string text)
    {
        textPresenter.PresentText("");
        
        _reactSequence?.Kill();
        _reactSequence = DOTween.Sequence();
        _reactSequence.AppendInterval(2f); // suspense
        
        _reactSequence.AppendCallback(() =>
        {
             _animator.SetBool("isUpset", !isGood);
            textPresenter.PresentText(text);
        });
        _reactSequence.AppendInterval(textPresenter.CalculateSpeechTime(text) + 2f);
        
        if (!isGood)
        {
            _reactSequence.AppendCallback(() => { GameManager.Singleton.RegisterFailure(); });
        }

        var okNextText = "Anyway...";
        
        _reactSequence.AppendCallback(() =>
        {
             _animator.SetBool("isUpset", false);
            textPresenter.PresentText(okNextText);
        });
        _reactSequence.AppendInterval(textPresenter.CalculateSpeechTime(okNextText) + 2f);
        
        _reactSequence.OnComplete(() =>
        {
            GameManager.Singleton.EnemyReactionFinished();
        });
    }
}