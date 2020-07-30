using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Tools.Audio
{
    [CreateAssetMenu(menuName="Audio/Composite Event")]
    public class CompositeAudioEvent : AudioEvent
    {
        [Serializable]
        public struct CompositeEntry
        {
            public AudioEvent Event;
            public float Weight;
        }

        public CompositeEntry[] Entries = null;

        public override void Play(AudioSource source)
        {
            float totalWeight = 0;
            for (int i = 0; i < Entries.Length; ++i)
            {
                totalWeight += Entries[i].Weight;
            }

            float pick = Random.Range(0, totalWeight);

            for (int i = 0; i < Entries.Length; ++i)
            {
                if (pick > Entries[i].Weight)
                {
                    pick -= Entries[i].Weight;
                    continue;
                }

                Entries[i].Event.Play(source);

                return;
            }
        }
    }
}