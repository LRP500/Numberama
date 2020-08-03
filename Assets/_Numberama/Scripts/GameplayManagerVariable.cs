using Tools.Variables;
using UnityEngine;

namespace Numberama
{
    [CreateAssetMenu(menuName = "Numberama/References/Gameplay Manager")]
    public class GameplayManagerVariable : Variable<GameplayManager>
    {
        public void OnClickContinue()
        {
            Value.RestartWithSameNumbers();
        }

        public void OnClickRestart()
        {
            Value.RestartWithNewNumbers();
        }
    }
}
