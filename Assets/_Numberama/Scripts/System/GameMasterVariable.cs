using Tools.Variables;
using UnityEngine;

namespace Numberama
{
    [CreateAssetMenu(menuName = "Numberama/References/Game Master")]
    public class GameMasterVariable : Variable<GameMaster>
    {
        public void OnClickLaunchTutorial()
        {
            Value?.LaunchTutorial();
        }

        public void OnClickLaunchGame(Difficulty difficulty)
        {
            Value?.StartNewGame(difficulty);
        }
    }
}
