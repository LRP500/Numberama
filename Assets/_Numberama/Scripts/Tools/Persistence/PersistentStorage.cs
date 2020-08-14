using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Tools.Persistence
{
    public class PersistentStorage : MonoBehaviour
    {
        protected string _savePath = string.Empty;

        private Stack<string> _history = null;

        public bool SaveFileExists => File.Exists(_savePath);
        public int HistorySize => _history.Count;

        private void Awake()
        {
            _history = new Stack<string>();
            SetSavePath();
        }

        protected virtual void SetSavePath()
        {
            _savePath = Path.Combine(Application.persistentDataPath, "save");
        }

        public void Save(IPersistable persistable)
        {
            using (BinaryWriter writer = new BinaryWriter(File.Open(_savePath, FileMode.Create)))
            {
                persistable.Save(new GameDataWriter(writer));
            }
        }

        public void Load(IPersistable persistable)
        {
            using (BinaryReader reader = new BinaryReader(File.Open(_savePath, FileMode.Open)))
            {
                persistable.Load(new GameDataReader(reader));
            }
        }

        #region History

        public void Record(IPersistable persistable)
        {
            string text = File.ReadAllText(_savePath);
            _history.Push(text);
        }

        public void Undo(IPersistable persistable)
        {
            if (_history.Count == 0)
            {
                return;
            }

            string previous = _history.Pop();

            using (Stream stream = GenerateStreamFromString(previous))
            {
                persistable.Load(new GameDataReader(new BinaryReader(stream)));
            }
        }

        public void ClearUndoHistory(IPersistable persistable)
        {
            _history.Clear();
        }

        #endregion History

        public static Stream GenerateStreamFromString(string value)
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(value);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }
    }
}