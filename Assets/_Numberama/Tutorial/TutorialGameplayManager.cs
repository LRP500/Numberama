using System.Collections;
using UnityEngine;

namespace Numberama.Tutorial
{
    public class TutorialGameplayManager : GameplayManager
    {
        [SerializeField]
        private TutorialManager _tutorial = null;

        protected override void StartNewGame()
        {
            base.StartNewGame();

            StartCoroutine(StartTutorial());
        }

        private IEnumerator StartTutorial()
        {
            yield return new WaitForEndOfFrame();

            _tutorial.Play();
        }
    }
}
