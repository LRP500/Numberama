namespace Tools.Persistence
{
    public interface IPersistable
    {
        void Save(GameDataWriter writer);
        void Load(GameDataReader reader);
    }
}
