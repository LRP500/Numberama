using UnityEngine;

namespace Numberama.Tools.Action
{
    public abstract class Action : ScriptableObject
    {
        public abstract void Execute(GameObject target);
    }
}