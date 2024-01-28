using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using DG.Tweening;
using UnityEngine;

public class IntroController : MonoBehaviour
{
    [SerializeField] private CassetteLabel startingCassetteLabel;
    [SerializeField] private CassetteLabel creditsCassetteLabel;
    [SerializeField] private CassetteLabel quitCassetteLabel;

    [SerializeField] private Cassette cassettePrefab;

    [SerializeField] private TextPresenter textPresenter;

    [SerializeField] private Animator _enemyAnimator;

    public static IntroController Singleton;

    [SerializeField] private CanvasGroup blackoutCanvasGroup;

    public Cassette ChosenCassette { get; private set; }

    private void Awake()
    {
        Singleton = this;
        GameManager.StartingStarted += OnStartingStarted;
    }
    
    private void OnDestroy()
    {
        GameManager.StartingStarted -= OnStartingStarted;
    }

    private void OnStartingStarted()
    {
        GameManager.Singleton.StartCoroutine(StartIntro());
    }

    private IEnumerator StartIntro()
    {
        textPresenter.PresentText("");
        
        blackoutCanvasGroup.alpha = 1f;

        blackoutCanvasGroup.DOFade(0f, 2f);

        yield return new WaitForSeconds(3f);
        
        var introText = new List<string>()
        {
            "Hi there.",
            "I have a great standup routine for you!"
        };
        textPresenter.PresentText(introText);

        var duration = textPresenter.CalculateSpeechTime(introText);
        yield return new WaitForSeconds(duration + 1f);

        var introCassetteLabels = new List<CassetteLabel>
        {
            startingCassetteLabel,
            creditsCassetteLabel,
            quitCassetteLabel
        };

        var cassettes = new List<Cassette>();
        foreach (var cassetteLabel in introCassetteLabels)
        {
            var newInstance = Instantiate(cassettePrefab);
            newInstance.SetTypeData(cassetteLabel);
            cassettes.Add(newInstance);
        }

        HandCassettesState.Singleton.SetupIntroCassettes(cassettes);

        yield return null;
    }

    public void ChoseCassette(Cassette cassette)
    {
        StartCoroutine(ChoseCassetteCoroutine(cassette));
    }

    private IEnumerator ChoseCassetteCoroutine(Cassette cassette)
    {
        ChosenCassette = cassette;
        switch (cassette.Type)
        {
            case Cassette.CassetteType.Quit:
                yield return new WaitForSeconds(1f);
                
                _enemyAnimator.SetBool("isUpset", true);
                var textList = new List<string>() { "What?", "How dare you!" };
                textPresenter.PresentText(textList);

                var duration = textPresenter.CalculateSpeechTime(textList);

                yield return new WaitForSeconds(duration + 1f);
                _enemyAnimator.SetBool("isMad", true);
                yield return new WaitForSeconds(2f);

                Debug.Log("quit");
                Application.Quit();
                break;
            case Cassette.CassetteType.Credits:
                yield return new WaitForSeconds(1f);
                _enemyAnimator.SetBool("isHappy", true);

                var creditText1 = new List<string>() { "Ah.", "How nice that you're interested!" };
                textPresenter.PresentText(creditText1);
                var creditText1Duration = textPresenter.CalculateSpeechTime(creditText1);

                yield return new WaitForSeconds(creditText1Duration + 1f);
                _enemyAnimator.SetBool("isHappy", false);
                yield return new WaitForSeconds(2f);

                _enemyAnimator.SetBool("isReading", true);
                var creditText2 = new List<string>()
                {
                    "This game was made by Luca Tescari, Lena Schuetz and Raphael Anderegg."
                };
                textPresenter.PresentText(creditText2);
                var creditText2Duration = textPresenter.CalculateSpeechTime(creditText2);
                
                yield return new WaitForSeconds(creditText2Duration);
                _enemyAnimator.SetBool("isReading", false);
                
                yield return new WaitForSeconds(2f);
                
                
                var newCreditsCassette = Instantiate(cassettePrefab);
                newCreditsCassette.SetTypeData(creditsCassetteLabel);
                
                Destroy(ChosenCassette.gameObject);
                ChosenCassette = null;
                
                HandCassettesState.Singleton.AddNewCassette(newCreditsCassette);
                break;
            
            case Cassette.CassetteType.Play:
                yield return new WaitForSeconds(1f);
                
                HandCassettesState.Singleton.RemoveAllCassettes();
                
                _enemyAnimator.SetBool("isHappy", true);
                var startText = new List<string>() { "Great!", "Let's start!" };
                textPresenter.PresentText(startText);
                var startTextDuration = textPresenter.CalculateSpeechTime(startText);
                
                yield return new WaitForSeconds(startTextDuration + 1f);
                
                Destroy(ChosenCassette.gameObject);
                ChosenCassette = null;
                
                _enemyAnimator.SetBool("isHappy", false);
                
                GameManager.Singleton.StartGame();
                break;
        }
    }
}