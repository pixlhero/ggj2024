using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BadEnding : MonoBehaviour
{
    [SerializeField] TextPresenter textPresenter;
    [SerializeField] private Animator _animator;
    // Start is called before the first frame update
    void Start()
    {
        GameManager.EndingBadStarted += () =>
        {   
            var endText = "You imbissle... You've ruined my mood... You've ruined everything!";

            textPresenter.PresentText(endText);
            _animator.SetTrigger("tantrum");

            var _reactSequence = DOTween.Sequence();
            _reactSequence.AppendInterval(textPresenter.CalculateSpeechTime(endText) + 3f);

            _reactSequence.OnComplete(() =>
            {
                GameManager.Reset();
            });

        };
    }

    // Update is called once per frame

}
