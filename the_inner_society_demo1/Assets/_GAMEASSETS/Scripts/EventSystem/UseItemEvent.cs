namespace InnerSociety.Events
{
    public class UseItemEvent
    {
        private Item usedItem;

        public UseItemEvent(Item usedItem)
        {
            this.usedItem = usedItem;
        }

        public Item UsedItem => usedItem;
    }
}