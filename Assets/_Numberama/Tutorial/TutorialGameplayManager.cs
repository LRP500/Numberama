using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Numberama.Tutorial
{
    public class TutorialGameplayManager : GameplayManager
    {
        [SerializeField]
        private TutorialManager _tutorial = null;

        [SerializeField]
        private List<int> _initialNumbers = null;

        protected override void Start()
        {
            StartNewGame();
        }

        protected override void StartNewGame()
        {
            _grid.Clear();
            _grid.PushRange(_initialNumbers);

            StartCoroutine(StartTutorial());
        }

        private IEnumerator StartTutorial()
        {
            yield return new WaitForEndOfFrame();

            _tutorial.Play();
        }
    }
}
