using UnityEngine;

namespace Numberama.Tools
{
    public abstract class Action : ScriptableObject
    {
        public abstract void Execute(GameObject target);
    }
}