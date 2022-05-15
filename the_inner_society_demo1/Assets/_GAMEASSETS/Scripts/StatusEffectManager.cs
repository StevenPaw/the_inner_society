using System;
using System.Collections.Generic;
using InnerSociety.Events;
using TMPro;
using UnityEngine;

namespace InnerSociety
{
    public class StatusEffectManager : MonoBehaviour
    {
        [SerializeField] private TMP_Text statuseffect;
        [SerializeField] private List<StatusEffect> activeEffects;

        private void OnEnable()
        {
            Message<ReceiveStatusEffectEvent>.Add(OnReceiveStatusEffect);
        }
        
        private void OnDisable()
        {
            Message<ReceiveStatusEffectEvent>.Remove(OnReceiveStatusEffect);
        }

        private void OnReceiveStatusEffect(ReceiveStatusEffectEvent ctx)
        {
            activeEffects.Add(ctx.ReceivedEffect);
            UpdateActiveStatusEffects();
        }

        private void UpdateActiveStatusEffects()
        {
            if (activeEffects != null)
            {
                string statuseffects = "";
                foreach (StatusEffect effect in activeEffects)
                {
                    statuseffects = statuseffects + effect.name + ", ";
                }
                statuseffects = statuseffects.Substring(0, statuseffects.Length - 2);
                statuseffect.text = statuseffects;
            }
            else
            {
                statuseffect.text = "Gesund";
            }
        }
    }
}