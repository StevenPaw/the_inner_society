namespace InnerSociety.Events
{
    public class DialogueChangeEvent
    {
        private string newDialogue;

        public DialogueChangeEvent(string newDialogue)
        {
            this.newDialogue = newDialogue;
        }

        public string NewDialogue => newDialogue;
    }
}