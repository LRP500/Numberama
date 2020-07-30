using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace Tools.Audio
{
    [CreateAssetMenu(menuName = "Audio/Simple Event")]
    public class SimpleAudioEvent : AudioEvent
    {
        public AudioClip[] _clips = null;

        [MinMaxSlider(0, 1, ShowFields = true)]
        public Vector2 _volume =  Vector2.one;

        [MinMaxSlider(0, 2, ShowFields = true)]
        public Vector2 _pitch = Vector2.one;

        private AudioSource _previewer = null;

        private void Awake()
        {
    #if UNITY_EDITOR
            _previewer = EditorUtility.CreateGameObjectWithHideFlags(
                "Audio preview",
                HideFlags.HideAndDontSave,
                typeof(AudioSource)).GetComponent<AudioSource>();
    #endif
        }

        public override void Play(AudioSource source)
        {
            if (_clips.Length == 0)
            {
                return;
            }

            source.clip = _clips[Random.Range(0, _clips.Length)];
            source.volume = Random.Range(_volume.x, _volume.y);
            source.pitch = Random.Range(_pitch.x, _pitch.y);
            source.Play();
        }

        [Button]
        private void Preview()
        {
            Play(_previewer);
        }
    }
}
