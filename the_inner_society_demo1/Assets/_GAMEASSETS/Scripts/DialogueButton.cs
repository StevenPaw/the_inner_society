using TMPro;
using UnityEngine;

namespace InnerSociety
{
    public class DialogueButton
    {
        [SerializeField] private TMP_Text text;
        [SerializeField] private DialogueOption option;

        public DialogueOption Option
        {
            get => option;
            set => option = value;
        }

        public void ReloadTexts()
        {
            text.text = option.optionTitle;
        }

        public void OnButtonClick()
        {
            Debug.Log("Change Scene and calculate!");
        }
    }
}