using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BadEnding : MonoBehaviour
{
    [SerializeField] TextPresenter textPresenter;
    [SerializeField] private Animator _animator;

    [SerializeField] private CanvasGroup blackoutCanvasGroup;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.EndingBadStarted += BadEndingStarted;
    }

    private void OnDestroy()
    {
        GameManager.EndingBadStarted -= BadEndingStarted;
    }

    private void BadEndingStarted()
    {
        StartCoroutine(StartOutro());
    }

    private IEnumerator StartOutro()
    {
        yield return new WaitForSeconds(2f);
        
        GameManager.Singleton.SetLivesToZero();
        
        _animator.SetBool("isMad", true);
        var endText = new List<string>()
        {
            "You imbecile!",
            "You ruin EVERYTHING!"
        };
        
        AudioHandler.singleton.Play_Effect_VeryBad();

        textPresenter.PresentText(endText);

        var duration = textPresenter.CalculateSpeechTime(endText);
        yield return new WaitForSeconds(duration);
        _animator.SetTrigger("tantrum");
        yield return new WaitForSeconds(2f);
        
        textPresenter.PresentText("");

        blackoutCanvasGroup.alpha = 0f;

        blackoutCanvasGroup.DOFade(1f, 2f);

        yield return new WaitForSeconds(3f);

        GameManager.Reset();
    }

    // Update is called once per frame
}