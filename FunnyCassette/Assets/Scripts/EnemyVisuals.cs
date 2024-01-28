using System.Collections.Generic;
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
        GameManager.EndingBadStarted += () => { _animator.SetBool("isMad", true); };

        AnimationEvents.OnHitEvent += OnHitEvent;
    }

    private void OnEnemeyTalksState(DialogPhrase newPhrase)
    {
        _animator.SetBool("isReading", true);

        textPresenter.PresentText(newPhrase.text);

        GameManager.Singleton.EnemyFinishedTalking();
        _reactSequence = DOTween.Sequence();
        _reactSequence.AppendInterval(textPresenter.CalculateSpeechTime(newPhrase.text));
        _reactSequence.AppendCallback(() => { _animator.SetBool("isReading", false); });
    }

    private void OnReact(bool isGood, string text, string nextText)
    {
        textPresenter.PresentText("");

        _reactSequence?.Kill();
        _reactSequence = DOTween.Sequence();
        _reactSequence.AppendInterval(2f); // suspense

        _reactSequence.AppendCallback(() =>
        {
            _animator.SetBool("isHappy", isGood);
            _animator.SetBool("isUpset", !isGood);
            textPresenter.PresentText(text);
        });
        _reactSequence.AppendInterval(textPresenter.CalculateSpeechTime(text) + 2f);

        _reactSequence.AppendCallback(() =>
        {
            _animator.SetBool("isHappy", false);
            _animator.SetBool("isUpset", false);
            textPresenter.PresentText(nextText);
        });

        _reactSequence.AppendInterval(textPresenter.CalculateSpeechTime(nextText) + 2f);

        _reactSequence.OnComplete(() =>
        {
            GameManager.Singleton.EnemyReactionFinished();
        });

        _reactSequence.AppendCallback(() =>
        {
            AudioHandler.singleton.Play_Ambiance_Drone();
        });
    }

    private void OnHitEvent()
    {
        GameManager.Singleton.RegisterFailure();
    }
}