namespace InnerSociety.Events
{
    public class CollectItemEvent
    {
        private Item itemToCollect;

        public CollectItemEvent(Item itemToCollect)
        {
            this.itemToCollect = itemToCollect;
        }

        public Item ItemToCollect => itemToCollect;
    }
}