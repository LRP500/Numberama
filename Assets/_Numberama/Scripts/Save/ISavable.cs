namespace Numberama
{
    public interface ISavable
    {
        SaveData Save();
        void Restore(SaveData data);
    }
}
