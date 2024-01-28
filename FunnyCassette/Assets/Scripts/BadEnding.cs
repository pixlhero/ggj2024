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
        GameManager.EndingBadStarted += () =>
        {   
            var endText = "You imbissle... You've ruined my mood... You've ruined everything!";

            textPresenter.PresentText(endText);

            var _reactSequence = DOTween.Sequence();
            _reactSequence.AppendInterval(textPresenter.CalculateSpeechTime(endText) + 0f);

            _reactSequence.OnComplete(() =>
            {

            _animator.SetTrigger("tantrum");
                GameManager.Singleton.StartCoroutine(StartOutro());
            });

        };
    }

        private IEnumerator StartOutro()
    {
        yield return new WaitForSeconds(2f);
        textPresenter.PresentText("");
        
        blackoutCanvasGroup.alpha = 0f;

        blackoutCanvasGroup.DOFade(1f, 2f);

        yield return new WaitForSeconds(3f);
        
        
        GameManager.Reset();
    }

    // Update is called once per frame

}
