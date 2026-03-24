using Numberama.Tools.Variables;
using Numberama.UI;
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
