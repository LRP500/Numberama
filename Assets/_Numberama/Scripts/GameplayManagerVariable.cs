using Tools.Variables;
using UnityEngine;

namespace Numberama
{
    [CreateAssetMenu(menuName = "Numberama/References/Gameplay Manager")]
    public class GameplayManagerVariable : Variable<GameplayManager>
    {
        public void OnClickContinue()
        {
            Value.Continue();
        }

        public void OnClickRestart()
        {
            Value.Restart();
        }
    }
}
