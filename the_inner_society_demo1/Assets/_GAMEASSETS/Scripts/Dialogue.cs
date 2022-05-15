using System.Collections.Generic;
using UnityEngine;

namespace InnerSociety
{
    /// <summary>
    /// A scriptable object for all types of peeple
    /// </summary>
    [CreateAssetMenu(fileName = "New Dialogue", menuName = "The Inner Circle/Dialogue", order = 1)]
    public class Dialogue : ScriptableObject
    {
        [SerializeField] private string dialogueIdentifier;
        [SerializeField] private string dialogueHeadline;
        [SerializeField] [TextArea(5, 10)] private string contentText;
        public List<DialogueOption> options;

        public string DialogueIdentifier => dialogueIdentifier;
        public string DialogueHeadline => dialogueHeadline;
        public string ContentText => contentText;

        public List<DialogueOption> Options => options;
    }
}