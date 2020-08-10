using Tools.Variables;
using UnityEngine;

namespace Numberama.Tutorial
{
    [CreateAssetMenu(menuName = "Numberama/References/Tutorial Manager")]
    public class TutorialManagerVariable : Variable<TutorialManager>
    {
        public void OnClickNext()
        {
            Value.NextStep();
        }
    }
}
