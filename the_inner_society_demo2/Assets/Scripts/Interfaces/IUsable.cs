namespace farmingsim
{
    public interface IUsable
    {
        public void SetUsed(bool isUsed);
        public UsableTypes GetUsableType();
    }
}