using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextPresenter : MonoBehaviour
{
    [SerializeField] TMPro.TMP_Text shownText;
    [SerializeField] private AudioClip dialogTypingSoundClip;
    private AudioSource audioSource;

    private void Awake() {
        audioSource = this.gameObject.AddComponent<AudioSource>();
    }

    public void PresentText(string text) {
        shownText.text = "";

        StartCoroutine(PrintText(text));

        // shownText.text = text;
    }

    private IEnumerator PrintText(string text) {
        shownText.maxVisibleCharacters = 0;

        foreach (char letter in text.ToCharArray()) {
            audioSource.PlayOneShot(dialogTypingSoundClip);

            shownText.text += letter;

            shownText.maxVisibleCharacters++;
            yield return new WaitForSeconds(0.1f);
        }
    }
}
