using System.Collections;
using UnityEngine;

namespace Numberama
{
    public class AudioManager : MonoBehaviour
    {
        [Header("Music")]

        [SerializeField]
        private AudioClip _soundtrack = null;

        [SerializeField]
        private AudioSource _soundtrackSource = null;

        [Space]
        [SerializeField]
        private AudioManagerVariable _runtimeReference = null;

        private void Awake()
        {
            _runtimeReference.SetValue(this);
        }

        private void Start()
        {
            if (_soundtrack)
            {
                _soundtrackSource.clip = _soundtrack;
                _soundtrackSource.Play();
            }
        }

        private void OnDestroy()
        {
            _runtimeReference.Clear();
        }

        public void Mute()
        {
            AudioListener.pause = !AudioListener.pause;
        }

        private IEnumerator FadeVolume(float start, float end, float duration)
        {
            AudioListener.volume = start;

            float elapsed = 0;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                AudioListener.volume = Mathf.Lerp(start, end, elapsed / duration);
                yield return new WaitForEndOfFrame();
            }

            AudioListener.volume = end;
        }
    }
}
