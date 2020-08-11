using UnityEngine;

namespace Numberama
{
    [CreateAssetMenu(menuName = "Numberama/Difficulty")]
    public class Difficulty : ScriptableObject
    {
        [SerializeField]
        private string _name = string.Empty;
        public string Name => _name;

        [Range(1, 9)]
        [SerializeField]
        private int _numbers = 9;
    }
}
