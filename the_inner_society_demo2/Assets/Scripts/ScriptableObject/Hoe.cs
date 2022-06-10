using System;
using farmingsim.Utils;
using UnityEngine;

namespace farmingsim
{
    [Serializable]
    [CreateAssetMenu(fileName = "New Hoe", menuName = "Tools/Hoe", order = 1)]
    public class Hoe : ScriptableObject, ITool, IItem
    {
        [SerializeField] private string hoeName;
        [SerializeField] private Sprite hoeIcon;

        public Sprite GetInventoryIcon()
        {
            return hoeIcon;
        }

        public string GetName()
        {
            return hoeName;
        }

        public ToolTypes GetToolType()
        {
            return ToolTypes.HOE;
        }

        public void Use(IUsable usedObject)
        {
            if (usedObject.GetUsableType() == UsableTypes.FARMFIELD)
            {
                FieldTile fieldTile = usedObject as FieldTile;
                if (!fieldTile.IsReady)
                {
                    fieldTile.IsReady = true;
                }
            }
        }
    }
}