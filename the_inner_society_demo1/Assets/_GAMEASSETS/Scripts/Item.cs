using UnityEngine;

namespace InnerSociety
{
    /// <summary>
    /// A scriptable object for items
    /// </summary>
    [CreateAssetMenu(fileName = "New Item", menuName = "The Inner Circle/Item", order = 1)]
    public class Item : ScriptableObject
    {
        [SerializeField] private string itemName;
        [SerializeField] private Sprite itemSprite;

        public string ItemName => itemName;
        public Sprite ItemSprite => itemSprite;
    }
}