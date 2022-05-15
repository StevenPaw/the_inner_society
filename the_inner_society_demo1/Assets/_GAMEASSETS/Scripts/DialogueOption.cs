using UnityEngine;

namespace InnerSociety
{
    [System.Serializable]
    public class DialogueOption
    {
        [Header("Option Text")]
        public string optionTitle;
        public string newTextIdentifier;
        [Header("Sympathy Change")]
        public Characters character;
        public int value;

        public DialogueOption(string optionTitle, string newTextIdentifier, Characters character, int value)
        {
            this.optionTitle = optionTitle;
            this.newTextIdentifier = newTextIdentifier;
            this.character = character;
            this.value = value;
        }
    }
}