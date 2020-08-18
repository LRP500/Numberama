using Tools.Variables;
using UnityEngine;

namespace Numberama
{
    [CreateAssetMenu(menuName = "Numberama/References/Main Menu Manager")]
    public class MainMenuManagerVariable : Variable<MainMenuManager>
    {
        public void OnSkipTutorial()
        {
            Value?.NavigateToDifficultyChoice();
        }
    }
}
