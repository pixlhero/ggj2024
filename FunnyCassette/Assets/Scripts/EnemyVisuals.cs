    using UnityEngine;

    public class EnemyVisuals : MonoBehaviour
    {
        [SerializeField]
        TMPro.TMP_Text shownText;
        
        private const string invisibleTag = "<color=#0000>";
        
        private void Awake()
        {
            EnemyState.DialogPhraseChanged += OnDialogPhraseChanged;
        }
        
        private void OnDialogPhraseChanged(DialogPhrase newPhrase)
        {
            var concatenatedText = "";
            foreach (var text in newPhrase.text)
            {
                concatenatedText += text + " ";
            }
            shownText.text = concatenatedText;
        }
    }