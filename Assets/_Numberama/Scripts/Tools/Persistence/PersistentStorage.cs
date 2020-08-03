using System.IO;
using UnityEngine;

namespace Tools.Persistence
{
    public class PersistentStorage : MonoBehaviour
    {
        public string savePath = string.Empty;

        public bool SaveFileExists => File.Exists(savePath);

        private void Awake()
        {
            savePath = Path.Combine(Application.persistentDataPath, "save");
        }

        public void Save(IPersistable persistable)
        {
            using (BinaryWriter writer = new BinaryWriter(File.Open(savePath, FileMode.Create)))
            {
                persistable.Save(new GameDataWriter(writer));
            }
        }

        public void Load(IPersistable persistable)
        {
            using (BinaryReader reader = new BinaryReader(File.Open(savePath, FileMode.Open)))
            {
                persistable.Load(new GameDataReader(reader));
            }
        }
    }
}