using UnityEngine;

namespace InnerSociety
{
    /// <summary>
    /// A scriptable object for items
    /// </summary>
    [CreateAssetMenu(fileName = "New Status Effect", menuName = "The Inner Circle/Status effect", order = 1)]
    public class StatusEffect : ScriptableObject
    {
        [SerializeField] private string statuseffectName;
    }
}