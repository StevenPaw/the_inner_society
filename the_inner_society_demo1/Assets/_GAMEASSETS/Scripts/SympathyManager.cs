using System;
using System.Collections.Generic;
using InnerSociety.Events;
using TMPro;
using UnityEngine;

namespace InnerSociety
{
    public class SympathyManager : MonoBehaviour
    {
        [SerializeField] private List<SympathyValue> sympathyValues;
        [SerializeField] private TMP_Text sympathyText;

        private void Start()
        {
            UpdateSympathyDisplay();
        }

        private void OnEnable()
        {
            Message<SympathyChangeEvent>.Add(OnSympathyChangeEvent);
        }

        private void OnDisable()
        {
            Message<SympathyChangeEvent>.Remove(OnSympathyChangeEvent);
        }

        private void OnSympathyChangeEvent(SympathyChangeEvent ctx)
        {
            UpdateSympathyDisplay();
            foreach (SympathyValue sv in sympathyValues)
            {
                if (sv.character == ctx.CharacterToChange)
                {
                    sv.sympathyValue += ctx.Amount;
                    UpdateSympathyDisplay();
                    return;
                }
            }

            Debug.Log("Character not found! " + ctx.CharacterToChange);
        }

        private void UpdateSympathyDisplay()
        {
            if (sympathyValues != null)
            {
                string sympathyValueText = "";
                foreach (SympathyValue entry in sympathyValues)
                {
                    sympathyValueText = sympathyValueText + entry.character + ": " + entry.sympathyValue + " \n";
                }
                sympathyValueText = sympathyValueText.Substring(0, sympathyValueText.Length - 2);
                sympathyText.text = sympathyValueText;
            }
            else
            {
                sympathyText.text = "Error";
            }
        }
    }
}