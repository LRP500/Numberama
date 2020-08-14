using Sirenix.OdinInspector;
using UnityEngine;

namespace Numberama
{
    [CreateAssetMenu(menuName = "Numberama/Store Item")]
    public class StoreItem : ScriptableObject
    {
        [SerializeField]
        private string _name = string.Empty;
        public string Name => _name;

        [SerializeField]
        private string _description = string.Empty;
        public string Description => _description;

        [SerializeField]
        [InlineEditor(InlineEditorModes.FullEditor)]
        private Sprite _icon = null;
        public Sprite Icon => _icon;

        [SerializeField]
        private float _price = 0f;
        public float Price => _price;
    }
}