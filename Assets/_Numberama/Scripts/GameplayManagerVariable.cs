using Tools.Variables;
using UnityEngine;

namespace Numberama
{
    [CreateAssetMenu(menuName = "Numberama/References/Gameplay Manager")]
    public class GameplayManagerVariable : Variable<GameplayManager>
    {
        public void OnClickContinue()
        {
            Value?.ClearCheckNumbers();
        }

        public void OnClickRestartWithSameNumbers()
        {
            Value?.RestartWithSameNumbers();
        }

        public void OnClickRestartWithNewNumbers()
        {
            Value?.RestartWithNewNumbers();
        }

        public void OnClickGiveUp()
        {
            Value?.GiveUp();
        }
    }
}
