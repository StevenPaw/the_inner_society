namespace InnerSociety.Events
{
    public class ReceiveStatusEffectEvent
    {
        private StatusEffect receivedEffect;

        public ReceiveStatusEffectEvent(StatusEffect receivedEffect)
        {
            this.receivedEffect = receivedEffect;
        }

        public StatusEffect ReceivedEffect => receivedEffect;
    }
}