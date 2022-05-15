using InnerSociety.Events;
using TMPro;
using UnityEngine;

namespace InnerSociety
{
    public class DialogueButton : MonoBehaviour
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
            Message.Raise(new DialogueChangeEvent(option.newTextIdentifier));
            if (option.receivedItems != null)
            {
                foreach (Item i in option.receivedItems)
                {
                    Message.Raise(new CollectItemEvent(i));
                }
            }
            
            if(option.receivedStatusEffects != null)
            {
                foreach (StatusEffect s in option.receivedStatusEffects)
                {
                    Message.Raise(new ReceiveStatusEffectEvent(s));
                }
            }

            if (option.sympathyChange != null)
            {
                foreach (SympathyValue sv in option.sympathyChange)
                {
                    Message.Raise(new SympathyChangeEvent(sv.character, sv.sympathyValue));
                }
            }
        }
    }
}