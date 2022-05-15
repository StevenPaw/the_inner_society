using System;
using System.Collections.Generic;
using InnerSociety.Events;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace InnerSociety
{
    public class DialogueDisplay : MonoBehaviour
    {
        [SerializeField] private Dialogue firstDialogue;
        [SerializeField] private TMP_Text headline;
        [SerializeField] private TMP_Text content;
        [SerializeField] private Transform buttonHolder;
        [SerializeField] private GameObject buttonPrefab;
        [SerializeField] private Image backgroundImage;
        [SerializeField] private Sprite defaultBackground;
        [SerializeField] private Image character1;
        [SerializeField] private Image character2;
        [SerializeField] private Sprite defaultCharacter;
        [SerializeField] private List<Dialogue> dialogues;

        private void Start()
        {
            ChangeDialogue(firstDialogue);
        }

        private void OnEnable()
        {
            Message<DialogueChangeEvent>.Add(OnDialogueChangeEvent);
        }
        
        private void OnDisable()
        {
            Message<DialogueChangeEvent>.Remove(OnDialogueChangeEvent);
        }

        private void ChangeDialogue(Dialogue dialogueToChangeTo)
        {
            headline.text = dialogueToChangeTo.DialogueHeadline;
            content.text = dialogueToChangeTo.ContentText;
            CleanDialogueOptions();
            foreach (DialogueOption d in dialogueToChangeTo.options)
            {
                DialogueButton dialogueButton = Instantiate(buttonPrefab, buttonHolder).GetComponent<DialogueButton>();
                dialogueButton.Option = d;
                dialogueButton.ReloadTexts();
            }

            if (dialogueToChangeTo.BackgroundSprite != null)
            {
                backgroundImage.sprite = dialogueToChangeTo.BackgroundSprite;
            }
            else
            {
                backgroundImage.sprite = defaultBackground;
            }

            if (dialogueToChangeTo.Character1Sprite != null)
            {
                character1.sprite = dialogueToChangeTo.Character1Sprite;
            }
            else
            {
                character1.sprite = defaultCharacter;
            }
            
            if (dialogueToChangeTo.Character2Sprite != null)
            {
                character2.sprite = dialogueToChangeTo.Character2Sprite;
            }
            else
            {
                character2.sprite = defaultCharacter;
            }
            
        }

        private void ShowError(string errorcode, string errormessage)
        {
            headline.text = "ERROR: " + errorcode;
            content.text = errormessage;
            CleanDialogueOptions();
            DialogueButton dialogueButton = Instantiate(buttonPrefab, buttonHolder).GetComponent<DialogueButton>();
            dialogueButton.Option = new DialogueOption("Zurück zum Hauptmenü", "mainmenu");
            dialogueButton.ReloadTexts();
        }

        private void CleanDialogueOptions()
        {
            foreach (Transform child in buttonHolder)
            {
                Destroy(child.gameObject);
            }
        }

        public void OnBackToMenuClick()
        {
            SceneManager.LoadScene(Scenes.MAINMENU);
        }

        private void OnDialogueChangeEvent(DialogueChangeEvent ctx)
        {
            foreach (Dialogue d in dialogues)
            {
                if (d.DialogueIdentifier == ctx.NewDialogue)
                {
                    ChangeDialogue(d);
                    return;
                }
            }

            if (ctx.NewDialogue == "mainmenu")
            {
                SceneManager.LoadScene(Scenes.MAINMENU);
                return;
            }
            
            ShowError("E01", "!!! CAN'T FIND DIALOGUE '" + ctx.NewDialogue + "'!!!");
        }
    }
}