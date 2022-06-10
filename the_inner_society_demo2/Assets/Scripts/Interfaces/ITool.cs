using farmingsim.Utils;
using UnityEngine;
using UnityEngine.UIElements;

namespace farmingsim
{
    public interface ITool : IItem
    {
        public ToolTypes GetToolType();
        public void Use(IUsable usedObject);
    }
}