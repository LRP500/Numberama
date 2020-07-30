using UnityEngine;

namespace Tools.Navigation
{
    public class TrackEditorScene : MonoBehaviour
    {
        [SerializeField]
        private NavigationManager _navigationManager = null;

        [SerializeField]
        private SceneReference _currentScene = null;

        private void Awake()
        {
            _navigationManager.TrackScene(_currentScene);
        }
    }
}
