﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Tools.Navigation
{
    [CreateAssetMenu(menuName = "Tools/Navigation/Navigation Manager")]
    public class NavigationManager : ScriptableObject
    {
        [SerializeField]
        private SceneReference _master = null;

        private Stack<SceneReference> _history = null;
        public Stack<SceneReference> History
        {
            get
            {
                _history = _history ?? new Stack<SceneReference>();
                return _history;
            }
        }

        public IEnumerator FastLoad(SceneReference scene, System.Action callback = null)
        {
            yield return LoadAdditive(scene);

            callback?.Invoke();

            yield return UnloadAll(new List<Scene>
            {
                SceneManager.GetSceneByPath(scene.path)
            });
        }

        public IEnumerator LoadAdditive(SceneReference scene, bool setActive = true)
        {
            if (!SceneManager.GetSceneByPath(scene.path).isLoaded)
            {
                yield return SceneManager.LoadSceneAsync(scene.path, LoadSceneMode.Additive);

                if (setActive)
                {
                    SceneManager.SetActiveScene(SceneManager.GetSceneByPath(scene.path));
                }

                TrackScene(scene);
            }
        }

        private Scene[] GetLoadedScenes()
        {
            int sceneCount = SceneManager.sceneCount;
            Scene[] loadedScenes = new Scene[sceneCount];

            for (int i = 0, length = SceneManager.sceneCount; i < length; i++)
            {
                loadedScenes[i] = SceneManager.GetSceneAt(i);
            }

            return loadedScenes;
        }

        public IEnumerator UnloadAll(List<Scene> exceptions = null)
        {
            foreach (Scene loadedScene in GetLoadedScenes())
            {
                /// Active scene
                if (loadedScene == SceneManager.GetActiveScene())
                {
                    continue;
                }

                /// Master scene
                if (loadedScene.path == _master.path)
                {
                    continue;
                }

                /// Exceptions
                if (exceptions != null && exceptions.Contains(loadedScene))
                {
                    continue;
                }

                yield return SceneManager.UnloadSceneAsync(loadedScene);
            }
        }

        public IEnumerator UnloadSingle(SceneReference scene)
        {
            yield return SceneManager.UnloadSceneAsync(scene.name);

            /// Pop scene from history stack.
            UntrackScene(scene);
            
            /// Set previous scene active.
            SceneManager.SetActiveScene(SceneManager.GetSceneByPath(_history.Peek().path));
        }

        public static void QuitGame()
        {
#if UNITY_EDITOR
            Debug.Log("Application.Quit()");
            UnityEditor.EditorApplication.isPlaying = false;
#else
			Application.Quit();
#endif
        }

        public void TrackScene(SceneReference scene)
        {
            if (History.Count == 0 || (History.Count > 0 && History.Peek() != scene))
            {
                History.Push(scene);
            }
        }

        public void UntrackScene(SceneReference scene)
        {
            if (History.Count > 0 && History.Peek() == scene)
            {
                History.Pop();
            }
        }
    }
}
