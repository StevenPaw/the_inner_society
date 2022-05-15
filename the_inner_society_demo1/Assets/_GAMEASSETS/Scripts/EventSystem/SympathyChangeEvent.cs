namespace InnerSociety.Events
{
    public class SympathyChangeEvent
    {
        private Characters characterToChange;
        private int amount;

        public SympathyChangeEvent(Characters characterToChange, int amount)
        {
            this.characterToChange = characterToChange;
            this.amount = amount;
        }

        public Characters CharacterToChange => characterToChange;
        public int Amount => amount;
    }
}