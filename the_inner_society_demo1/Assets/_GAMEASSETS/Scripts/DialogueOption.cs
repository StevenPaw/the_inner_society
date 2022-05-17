using UnityEngine;

namespace InnerSociety
{
    [System.Serializable]
    public class DialogueOption
    {
        [Header("Option Text")]
        public string optionTitle;
        public string newTextIdentifier;
        [Header("Change")] 
        public SympathyValue[] sympathyChange;
        public Item[] receivedItems;
        public StatusEffect[] receivedStatusEffects;
        public Item[] requiredItems;
        public Item[] forbiddenItems;
        public Item[] usedItems;

        public DialogueOption(string optionTitle, string newTextIdentifier)
        {
            this.optionTitle = optionTitle;
            this.newTextIdentifier = newTextIdentifier;
        }
    }
}