using System.Collections;
using UnityEngine;

public class TextPresenter : MonoBehaviour
{
    [SerializeField] TMPro.TMP_Text shownText;
    [SerializeField] private AudioClip dialogTypingSoundClip;
    [SerializeField] private bool stopAudioSource;
    [SerializeField][Range(-3, 3)] private float minPitch = .5f;
    [SerializeField][Range(-3, 3)] private float maxPitch = 3;
    [SerializeField][Range(1, 5)] private int frequencyLevel = 1;


    private AudioSource audioSource;
    private Coroutine presentTextCoroutine;

    private void Awake()
    {
        audioSource = this.gameObject.AddComponent<AudioSource>();
        audioSource.volume = .2f;
    }

    public void PresentText(string text)
    {
        shownText.text = "";

        presentTextCoroutine = StartCoroutine(PrintText(text));
    }

    private IEnumerator PrintText(string text)
    {
        shownText.maxVisibleCharacters = 0;

        foreach (char letter in text.ToCharArray())
        {
            shownText.text += letter;

            PlayDialogSound(shownText.maxVisibleCharacters);


            shownText.maxVisibleCharacters++;

            yield return new WaitForSeconds(0.1f);
        }
    }

    private void PlayDialogSound(int currentCharacterCound)
    {
        if (currentCharacterCound % frequencyLevel == 0)
        {
            if (stopAudioSource)
                audioSource.Stop();

            audioSource.pitch = Random.Range(minPitch, maxPitch);

            audioSource.PlayOneShot(dialogTypingSoundClip);
        }
    }
}
