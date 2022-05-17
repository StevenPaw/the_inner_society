using InnerSociety.Events;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace InnerSociety
{
    public class DialogueButton : MonoBehaviour
    {
        [SerializeField] private TMP_Text text;
        [SerializeField] private DialogueOption option;
        [SerializeField] private Button button;

        public DialogueOption Option
        {
            get => option;
            set => option = value;
        }

        public void ReloadTexts()
        {
            bool itemsAquired = true;
            if (option.requiredItems != null)
            {
                foreach (Item requiredItem in option.requiredItems)
                {
                    if (!InventoryManager.Instance.HasItem(requiredItem))
                    {
                        itemsAquired = false;
                    }
                }
                
                foreach (Item forbiddenItem in option.forbiddenItems)
                {
                    if (InventoryManager.Instance.HasItem(forbiddenItem))
                    {
                        itemsAquired = false;
                    }
                }
            }

            if (itemsAquired)
            {
                button.interactable = true;
                text.text = option.optionTitle;
            }
            else
            {
                button.interactable = false;
                text.text = "(Nicht verfügbar)";
            }
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