using System.Collections.Generic;
using UnityEngine;

namespace InnerSociety
{
    /// <summary>
    /// A scriptable object for dialogues
    /// </summary>
    [CreateAssetMenu(fileName = "New Dialogue", menuName = "The Inner Circle/Dialogue", order = 1)]
    public class Dialogue : ScriptableObject
    {
        [SerializeField] private string dialogueIdentifier;
        [SerializeField] private string dialogueHeadline;
        [SerializeField] [TextArea(5, 10)] private string contentText;
        [SerializeField] private Sprite backgroundSprite;
        public List<DialogueOption> options;

        public string DialogueIdentifier => dialogueIdentifier;
        public string DialogueHeadline => dialogueHeadline;
        public string ContentText => contentText;
        public Sprite BackgroundSprite => backgroundSprite;
        public List<DialogueOption> Options => options;
    }
}