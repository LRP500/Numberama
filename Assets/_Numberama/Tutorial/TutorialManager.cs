using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace Numberama.Tutorial
{
    public class TutorialManager : MonoBehaviour
    {
        [SerializeField]
        private List<TutorialStep> _steps = null;

        [SerializeField]
        private List<RectTransform> _masks = null;

        [SerializeField]
        private int _maskBorderThickness = 4;

        [SerializeField]
        private InfoMessagePanel _infoMessagePanel = null;

        [SerializeField]
        private Grid _grid = null;

        [SerializeField]
        private GridActionManager _gridActionManager = null;

        [SerializeField]
        private TutorialManagerVariable _runtimeReference = null;

        private int _currentStep = 0;

        private void Awake()
        {
            _runtimeReference.SetValue(this);
        }

        private void OnDestroy()
        {
            _runtimeReference.Clear();
        }

        public void Play()
        {
            InitializeStep(_steps[_currentStep]);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                NextStep();
            }
        }

        [Button]
        [ShowIf("@ UnityEngine.Application.isPlaying")]
        public void NextStep()
        {
            if (_currentStep + 1 < _steps.Count)
            {
                _currentStep++;
                InitializeStep(_steps[_currentStep]);
            }
        }

        [Button]
        [ShowIf("@ UnityEngine.Application.isPlaying")]
        public void PreviousStep()
        {
            if (_currentStep - 1 >=  0)
            {
                _currentStep--;
                InitializeStep(_steps[_currentStep]);
            }
        }

        private void InitializeStep(TutorialStep step)
        {
            Clear();

            _infoMessagePanel.Open(step.InfoMessage);

            switch (step.Highlight)
            {
                case TutorialStep.HighlightType.None: break;
                case TutorialStep.HighlightType.SingleCell:
                    {
                        HighlightCell(0, _grid.GetCell(step.FirstCell));
                        break;
                    }
                case TutorialStep.HighlightType.Move:
                    {
                        HighlightCell(0, _grid.GetCell(step.FirstCell));
                        HighlightCell(1, _grid.GetCell(step.SecondCell));
                        break;
                    }
                case TutorialStep.HighlightType.Action:
                    {
                        HighlightAction(_gridActionManager.GetAction(step.Action));
                        break;
                    }
                default: break;
            }
        }

        private void Clear()
        {
            foreach (RectTransform rect in _masks)
            {
                rect.gameObject.SetActive(false);
            }
        }

        private void Highlight(ref RectTransform mask, RectTransform target)
        {
            if (target == null || mask == null) return;
            Vector2Int border = new Vector2Int(_maskBorderThickness, _maskBorderThickness);
            mask.sizeDelta = target.sizeDelta + border;
            mask.position = target.position;
            mask.gameObject.SetActive(true);
        }

        private void HighlightCell(int index, GridCell cell)
        {
            RectTransform mask = _masks[index];
            Highlight(ref mask, cell.GetComponent<RectTransform>());
        }

        private void HighlightAction(GridAction action)
        {
            RectTransform mask = _masks[0];
            Highlight(ref mask, action.GetComponent<RectTransform>());
        }
    }
}
