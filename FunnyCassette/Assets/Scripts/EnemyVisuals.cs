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
        GameManager.LivesChanged += OnLivesChanged;

        AnimationEvents.OnHitEvent += OnHitEvent;
    }
    
    private void OnDestroy()
    {
        GameManager.EnemyTalksStarted -= OnEnemeyTalksState;
        GameManager.EnemyReactionStarted -= OnReact;
        GameManager.LivesChanged -= OnLivesChanged;

        AnimationEvents.OnHitEvent -= OnHitEvent;
    }
    
    private void OnLivesChanged(int lives)
    {
        if (lives <= 0)
        {
            _animator.SetBool("isMad", true);
        }
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

        if (_animator.GetBool("isMad"))
            return;

        _reactSequence?.Kill();
        _reactSequence = DOTween.Sequence();
        _reactSequence.AppendInterval(3f); // suspense

        _reactSequence.AppendCallback(() =>
        {
            _animator.SetBool("isHappy", isGood);
            _animator.SetBool("isUpset", !isGood);
            if (_animator.GetBool("isMad")) return;
            textPresenter.PresentText(text);
        });
        _reactSequence.AppendInterval(textPresenter.CalculateSpeechTime(text) + 2f);

        _reactSequence.AppendCallback(() =>
        {
            _animator.SetBool("isHappy", false);
            _animator.SetBool("isUpset", false);
            if (_animator.GetBool("isMad")) return;
            textPresenter.PresentText(nextText);
        });

        _reactSequence.AppendInterval(textPresenter.CalculateSpeechTime(nextText) + 2f);

        _reactSequence.OnComplete(() => GameManager.Singleton.EnemyReactionFinished());

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