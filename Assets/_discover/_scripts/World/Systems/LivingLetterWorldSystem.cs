using System.Collections.Generic;
using UnityEngine;

namespace Antura.Discover
{
    public enum LivingLetterKind
    {
        Letter,
        Word,
        Image,
        Card,
    }

    [System.Serializable]
    public class LivingLettersProfile
    {
        public TopicData Topic;
        [Range(0, 1)] public float LetterRatio = 0.45f;
        [Range(0, 1)] public float WordRatio = 0.35f;
        [Range(0, 1)] public float CardRatio = 0.20f;

        public void Normalize()
        {
            float s = LetterRatio + WordRatio + CardRatio;
            if (s <= 0.0001f)
            { LetterRatio = 0.45f; WordRatio = 0.35f; CardRatio = 0.20f; return; }
            LetterRatio /= s;
            WordRatio /= s;
            CardRatio /= s;
        }
    }

    public interface ILivingLetterSpawner
    {
        float LetterRatio { get; set; }
        float WordRatio { get; set; }
        float CardRatio { get; set; }

        void BuildPoolsFromTopic(TopicData topic, string[] extraWords = null, CardData[] extraCards = null);
        bool SpawnOne();
    }

    public class LivingLetterWorldSystem : MonoBehaviour, IWorldSystem
    {
        [Header("Default (used if no override)")]
        public LivingLettersProfile DefaultProfile = new LivingLettersProfile();

        public Transform SpawnersRoot;
        private readonly List<ILivingLetterSpawner> _spawners = new();

        void Awake() { Refresh(); }
        void OnEnable() { Refresh(); ApplyProfile(DefaultProfile); }
        void OnTransformChildrenChanged() => Refresh();

        void Refresh()
        {
            _spawners.Clear();
            var root = SpawnersRoot ? SpawnersRoot : transform;
            foreach (var mb in root.GetComponentsInChildren<MonoBehaviour>(true))
                if (mb is ILivingLetterSpawner s && !_spawners.Contains(s))
                    _spawners.Add(s);
        }

        public void ApplyProfile(LivingLettersProfile p)
        {
            if (p == null)
                return;
            p.Normalize();
            if (p.Topic == null)
                return;

            foreach (var s in _spawners)
            {
                s.BuildPoolsFromTopic(p.Topic, null, null);
                s.LetterRatio = p.LetterRatio;
                s.WordRatio = p.WordRatio;
                s.CardRatio = p.CardRatio;
            }
        }

        public void SpawnBurst(Vector3 center, int count)
        {
            if (_spawners.Count == 0)
                return;
            for (int i = 0; i < count; i++)
                _spawners[0].SpawnOne();
        }
    }
}
