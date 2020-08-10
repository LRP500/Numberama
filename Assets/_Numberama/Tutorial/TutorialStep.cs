using Sirenix.OdinInspector;
using Tools;
using UnityEngine;

namespace Numberama
{
    [CreateAssetMenu(menuName = "Numberama/Tutorial Step")]
    public class TutorialStep : ScriptableObject
    {
        public enum HighlightType
        {
            None,
            SingleCell,
            Move,
            Action
        }

        [SerializeField]
        private HighlightType _highlight = default;
        public HighlightType Highlight => _highlight;

        [SerializeField]
        [ShowIf("@ _highlight == HighlightType.SingleCell || _highlight == HighlightType.Move")]
        private Vector2Int _firstCell = default;
        public Vector2Int FirstCell => _firstCell;

        [SerializeField]
        [ShowIf("@ _highlight == HighlightType.Move")]
        private Vector2Int _secondCell = default;
        public Vector2Int SecondCell => _secondCell;

        [SerializeField]
        [ShowIf("@ _highlight == HighlightType.Action")]
        private GridActionInfo _action = null;
        public GridActionInfo Action => _action;

        [SerializeField]
        [ShowIf("@ _infoMessage != null")]
        [InlineEditor(InlineEditorObjectFieldModes.Hidden)]
        private InfoMessage _infoMessage = null;
        public InfoMessage InfoMessage => _infoMessage;

#if UNITY_EDITOR

        [Button]
        [ShowIf("@ _infoMessage == null")]
        private void AddInfoMessage()
        {
            _infoMessage = this.CreateSubAsset<InfoMessage>();
        }

        [Button]
        [ShowIf("@ _infoMessage != null")]
        private void RemoveInfoMessage()
        {
            this.RemoveSubAsset(_infoMessage);
        }

#endif
    }
}
