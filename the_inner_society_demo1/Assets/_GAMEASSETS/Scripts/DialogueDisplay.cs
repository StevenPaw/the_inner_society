using System;
using TMPro;
using UnityEngine;

namespace InnerSociety
{
    public class DialogueDisplay : MonoBehaviour
    {
        [SerializeField] private Dialogue firstDialogue;
        [SerializeField] private TMP_Text headline;
        [SerializeField] private TMP_Text content;
        [SerializeField] private Transform buttonHolder;
        [SerializeField] private GameObject buttonPrefab;

        private void Start()
        {
            headline.text = firstDialogue.DialogueHeadline;
            content.text = firstDialogue.ContentText;
            foreach (DialogueOption d in firstDialogue.options)
            {
                DialogueButton dialogueButton = Instantiate(buttonPrefab, buttonHolder).GetComponent<DialogueButton>();
                dialogueButton.Option = d;
                dialogueButton.ReloadTexts();
            }
        }
    }
}