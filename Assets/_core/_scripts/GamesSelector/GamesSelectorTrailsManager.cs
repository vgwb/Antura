using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Antura.GamesSelector
{
    /// <summary>
    /// Draws a trail behind the player's cursor when interacting with the GamesSelector.
    /// </summary>
    public class GamesSelectorTrailsManager : MonoBehaviour
    {
        public TrailRenderer TrailPrefab;

        private readonly List<TrailRenderer> pool = new List<TrailRenderer>();
        private float defZ;

        #region Unity

        void Awake()
        {
            defZ = TrailPrefab.transform.position.z;
            TrailPrefab.gameObject.SetActive(false);
            pool.Add(TrailPrefab);
        }

        void OnDestroy()
        {
            this.StopAllCoroutines();
        }

        #endregion

        #region Public Methods

        public TrailRenderer Spawn(Vector3 _position)
        {
            _position.z = defZ;
            TrailRenderer spawnTarget = null;
            foreach (TrailRenderer trail in pool)
            {
                if (trail.gameObject.activeSelf)
                {
                    continue;
                }
                trail.transform.position = _position;
                spawnTarget = trail;
            }
            if (spawnTarget == null)
            {
                spawnTarget = (TrailRenderer)Instantiate(TrailPrefab, _position, Quaternion.identity, this.transform);
                pool.Add(spawnTarget);
            }
            spawnTarget.gameObject.SetActive(true);
            return spawnTarget;
        }

        public void Despawn(TrailRenderer _trail, bool _waitForTrailToComplete = true)
        {
            if (_waitForTrailToComplete)
            {
                this.StartCoroutine(CO_TrailDespawnCoroutine(_trail));
            }
            else
            {
                DoDespawn(_trail);
            }
        }

        public void SetPosition(TrailRenderer _trail, Vector3 _position)
        {
            _trail.transform.position = _position;
        }

        #endregion

        #region Methods

        void DoDespawn(TrailRenderer _trail)
        {
            _trail.Clear();
            _trail.gameObject.SetActive(false);
        }

        IEnumerator CO_TrailDespawnCoroutine(TrailRenderer _trail)
        {
            yield return new WaitForSeconds(_trail.time);
            DoDespawn(_trail);
        }

        #endregion
    }
}
