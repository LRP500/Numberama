using Sirenix.OdinInspector;
using UnityEngine;

namespace Numberama
{
    [CreateAssetMenu(menuName = "Numberama/Grid Action")]
    public class GridActionInfo : ScriptableObject
    {
        [SerializeField]
        private float _reloadTime = 1f;
        public float ReloadTime => _reloadTime;

        [SerializeField]
        [InlineEditor(InlineEditorModes.GUIAndPreview)]
        private Sprite _icon = null;
        public Sprite Icon => _icon;
    }
}
