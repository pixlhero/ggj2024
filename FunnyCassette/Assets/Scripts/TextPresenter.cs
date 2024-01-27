using System.Collections;
using UnityEngine;

public class TextPresenter : MonoBehaviour
{
    [SerializeField] TMPro.TMP_Text shownText;
    [SerializeField] private AudioClip[] dialogTypingSoundClips;
    [SerializeField] private bool stopAudioSource;
    [SerializeField][Range(-3, 3)] private float minPitch = .5f;
    [SerializeField][Range(-3, 3)] private float maxPitch = 3;
    [SerializeField][Range(1, 5)] private int frequencyLevel = 1;
    [SerializeField][Range(0.01f, 0.5f)] private float charWaitingTime = 0.1f;
    [SerializeField] private bool predictableSpeech;


    private const string inbisibleTag = "<color=#0000>";

    private string _shownText;

    private AudioSource audioSource;
    private Coroutine presentTextCoroutine;

    private void Awake()
    {
        audioSource = this.gameObject.AddComponent<AudioSource>();
        audioSource.volume = .2f;
    }

    public float CalculateSpeechTime(string text)
    {
        return charWaitingTime * text.Length;
    }

    public void PresentText(string text)
    {
        _shownText = text;
        if(presentTextCoroutine != null)
            StopCoroutine(presentTextCoroutine);
        
        presentTextCoroutine = StartCoroutine(PrintText());
    }

    private IEnumerator PrintText()
    {
        for (var textIndex = 0; textIndex < _shownText.Length; textIndex++)
        {
            var text = _shownText.Substring(0, textIndex);
            text += inbisibleTag;
            text += _shownText.Substring(textIndex);
            shownText.text = text;
            PlayDialogSound(textIndex, _shownText[textIndex]);
            yield return new WaitForSeconds(charWaitingTime);
        }
    }

    private void PlayDialogSound(int currentCharacterCount, char currentCharacter)
    {
        if (currentCharacterCount % frequencyLevel == 0)
        {
            if (stopAudioSource)
                audioSource.Stop();

            AudioClip soundClip = null;

            if (predictableSpeech)
            {
                int hashCode = currentCharacter.GetHashCode();
                int predictableIndex = hashCode % dialogTypingSoundClips.Length;
                soundClip = dialogTypingSoundClips[predictableIndex];

                int minPitchInt = (int)(minPitch * 100);
                int maxPitchInt = (int)(maxPitch * 100);
                int pitchRangeInt = maxPitchInt - minPitchInt;

                if (pitchRangeInt != 0) {
                    int predictablePitchInt = (hashCode % pitchRangeInt) + minPitchInt;
                    float predictablePitch = predictablePitchInt / 100f;
                    audioSource.pitch = predictablePitch;
                } else {
                    audioSource.pitch = minPitch;
                }
            }
            else
            {
                // select sound clip
                int randomIndex = Random.Range(0, dialogTypingSoundClips.Length);
                soundClip = dialogTypingSoundClips[randomIndex];
                audioSource.pitch = Random.Range(minPitch, maxPitch);

            }

            audioSource.PlayOneShot(soundClip);
        }
    }
}
