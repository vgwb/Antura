using Antura.Audio;
using Antura.Minigames;
using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Antura.AnturaSpace
{
    /// <summary>
    /// Controls interactions and dynamics of an object thrown to Antura in AnturaSpace.
    /// </summary>
    public class ThrowableObject : MonoBehaviour
    {
        [Header("Parameters")]
        public bool Edible = false;
        public bool Catchable = false;

        #region EXPOSED MEMBERS
#pragma warning disable 649
        [Header("References")]
        [SerializeField]
        private Rigidbody m_oRigidbody;

        [SerializeField]
        private GameObject m_oParticle;

        [SerializeField]
        private float m_oParticleTime;

        [SerializeField]
        private Sfx m_oSfxOnPoof;

        [Header("Simple Throw")]
        [SerializeField]
        private Vector3 m_v3DirectionMinValues;

        [SerializeField]
        private Vector3 m_v3DirectionMaxValues;

        [SerializeField]
        private float m_fThrowMinMagnitude = 0f;

        [SerializeField]
        private float m_fThrowMaxMagnitude = 10f;

        [SerializeField]
        private ForceMode m_eThrowForceMode;

        [Header("Rotation")]
        [SerializeField]
        private float m_fRotationMinMagnitude = 0f;

        [SerializeField]
        private float m_fRotationMaxMagnitude = 10f;

        [SerializeField]
        private ForceMode m_eRotationForceMode;

        [Header("Drag")]
        public float m_fDragThrowMagnitudeScaling = 0.1f;

        public float m_fTimeSampling = 0.0333f;

        [SerializeField]
        private ForceMode m_eReleaseForceMode;
#pragma warning restore 649
        #endregion

        #region PRIVATE MEMBERS

        private GameObject m_oParticleInstance;
        bool m_bIsDragged = false;
        private Vector3 m_v3LastPosition;
        float m_lastPositionTime = 0;
        private float m_fTimeProgression = 0;

        private float lifetime = 0.0f;
        private float MaxLifetime = 10.0f;

        #endregion

        private static GameObject s_oParticleRootContainer;

        List<Plane> planes = new List<Plane>();

        #region GETTER/SETTER

        public float rotation_MaxMagnitude
        {
            get { return m_fRotationMaxMagnitude; }
            set { m_fRotationMaxMagnitude = value; }
        }

        public float rotation_MinMagnitude
        {
            get { return m_fRotationMinMagnitude; }
            set { m_fRotationMinMagnitude = value; }
        }

        public Rigidbody boneRigidbody { get; set; }

        public bool isDragging()
        {
            return m_bIsDragged;
        }

        public Action OnDeath;
        public Action OnRelease;

        #endregion

        #region INTERNALS

        IAudioSource poofSound;

        void Start()
        {
            planes.AddRange(GeometryUtility.CalculateFrustumPlanes(Camera.main));

            planes.Add(new Plane(Vector3.up, 0));
            planes.Add(new Plane(Vector3.back, 40));

            if (m_fRotationMaxMagnitude < m_fRotationMinMagnitude ||
                m_fThrowMaxMagnitude < m_fThrowMinMagnitude ||
                m_v3DirectionMinValues.x > m_v3DirectionMaxValues.x ||
                m_v3DirectionMinValues.y > m_v3DirectionMaxValues.y ||
                m_v3DirectionMinValues.z > m_v3DirectionMaxValues.z)
            {
                Debug.Log("Warning, unvalid min/max values");
            }

            //build root for cookies particles
            if (s_oParticleRootContainer == null)
            {
                var _oTempBase = new GameObject();
                s_oParticleRootContainer = Instantiate(_oTempBase);
                s_oParticleRootContainer.name = "[CookieParticles]";
                Destroy(_oTempBase);

                s_oParticleRootContainer.transform.position = Vector3.zero;
            }

            //Store dragging data to prepare for the releasing throw
            m_v3LastPosition = transform.position;
            m_fTimeProgression = 0;
            m_lastPositionTime = Time.realtimeSinceStartup;
        }


        void Update()
        {
            //if this object is being dragged
            if (m_bIsDragged)
            {
                m_fTimeProgression += Time.deltaTime;

                if (m_fTimeProgression >= m_fTimeSampling)
                {
                    //Store dragging data to prepare for the releasing throw
                    m_v3LastPosition = transform.position;
                    m_fTimeProgression = 0;
                    m_lastPositionTime = Time.realtimeSinceStartup;
                }


                //set the object position on the pointer(x,y) at it's current distance from the camera
                var _fCameraDistance = 6 + 4 * (Input.mousePosition.y / Screen.height);
                var newPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, _fCameraDistance));

                if (newPos.y < 1)
                {
                    newPos.y = 1;
                }

                transform.position = newPos;
            }
            else
            {
                // Limit inside view frustum
                var newPos = m_oRigidbody.position;
                for (int i = 0, count = planes.Count; i < count; ++i)
                {
                    var distance = planes[i].GetDistanceToPoint(newPos);
                    if (distance < 0)
                    {
                        var planeNormal = planes[i].normal;

                        m_oRigidbody.velocity -= 1.2f * Vector3.Dot(m_oRigidbody.velocity, planeNormal) * planeNormal;

                        newPos = newPos - distance * planeNormal;
                    }
                }
                m_oRigidbody.position = newPos;
            }


            if (!Edible)
            {
                lifetime += Time.deltaTime;
                if (lifetime > MaxLifetime)
                {
                    Destroy(gameObject);
                }
            }
        }

        //private void OnDisable()
        //{
        //    Poof(m_oParticleTime);
        //}

        void OnDestroy()
        {
            if (poofSound != null)
            {
                poofSound.Stop();
                poofSound = null;
            }

            if (OnDeath != null)
            { OnDeath(); }
            CancelInvoke();
        }

        #endregion

        #region PUBLIC FUNCTIONS

        /// <summary>
        /// The bone is throw with the setted forces
        /// </summary>
        public void SimpleThrow()
        {
            //resets actives forces
            m_oRigidbody.isKinematic = true;
            m_oRigidbody.isKinematic = false;

            //disable collision and enabled after 0.5 sec for avoid that Antura collision shot bone away
            m_oRigidbody.GetComponentInChildren<Collider>().enabled = false;
            StartCoroutine(Minigames.MissingLetter.Utils.LaunchDelay(0.5f,
                delegate
                { m_oRigidbody.GetComponentInChildren<Collider>().enabled = true; }));

            ApplyDefaultForces();
        }

        public void Drag()
        {
            //resets actives forces
            m_oRigidbody.isKinematic = true;

            //this way Antura won't eat it since collision won't happen
            gameObject.GetComponentInChildren<Collider>().isTrigger = true;

            m_bIsDragged = true;

            Update();

            m_fTimeProgression = 0;
            m_v3LastPosition = transform.position;
            m_lastPositionTime = Time.realtimeSinceStartup;
        }

        public void LetGo()
        {
            m_oRigidbody.isKinematic = false;

            gameObject.GetComponentInChildren<Collider>().isTrigger = false;

            m_bIsDragged = false;

            m_fTimeProgression = 0;

            //apply stored forces
            ApplyDragForces();

            if (OnRelease != null)
            { OnRelease(); }
        }

        /// <summary>
        /// Plays the particle effect for the given time.
        /// </summary>
        /// <param name="fDuration"></param>
        public void Poof()
        {
            if (m_oParticleInstance == null)
            {
                m_oParticleInstance = Instantiate(m_oParticle);

                //put cookie in the root
                if (s_oParticleRootContainer != null)
                {
                    m_oParticleInstance.transform.SetParent(s_oParticleRootContainer.transform);
                }
            }

            //put particle on cookie
            m_oParticleInstance.transform.position = transform.position;

            m_oParticleInstance.SetActive(true);
            foreach (var particles in m_oParticleInstance.GetComponentsInChildren<ParticleSystem>(true))
            {
                particles.Play();
            }

            poofSound = AudioManager.I.PlaySound(m_oSfxOnPoof);

            //if we were quick maybe the particle hasn't stopped yet, so try to cancel the old one;
            CancelInvoke("StopPoof");
            Invoke("StopPoof", m_oParticleTime);
        }

        #endregion

        /// <summary>
        /// Stop the particle effect.
        /// </summary>
        private void StopPoof()
        {
            if (m_oParticleInstance)
            {
                foreach (var particles in m_oParticleInstance.GetComponentsInChildren<ParticleSystem>(true))
                {
                    particles.Stop();
                }

                m_oParticleInstance.SetActive(false);
            }
        }

        #region PRIVATE FUNCTIONS

        /// <summary>
        /// Apply the default forces on the Rigidbody
        /// </summary>
        private void ApplyDefaultForces()
        {
            Vector3 _v3ThrowDirection = new Vector3(
                Random.Range(m_v3DirectionMinValues.x, m_v3DirectionMaxValues.x),
                Random.Range(m_v3DirectionMinValues.y, m_v3DirectionMaxValues.y),
                Random.Range(m_v3DirectionMinValues.z, m_v3DirectionMaxValues.z)
            );

            //Add rotation with random magnitude
            m_oRigidbody.AddTorque(Random.insideUnitSphere.normalized * Random.Range(m_fRotationMinMagnitude, m_fRotationMaxMagnitude),
                m_eRotationForceMode);
            //Add translation
            m_oRigidbody.AddForce(_v3ThrowDirection.normalized * Random.Range(m_fThrowMinMagnitude, m_fThrowMaxMagnitude),
                m_eThrowForceMode);
        }

        /// <summary>
        /// Apply the drag forces on the Rigidbody
        /// </summary>
        private void ApplyDragForces()
        {
            //Add rotation with random magnitude
            m_oRigidbody.AddTorque(Random.insideUnitSphere.normalized * Random.Range(m_fRotationMinMagnitude, m_fRotationMaxMagnitude),
                m_eRotationForceMode);
            //Add translation
            m_oRigidbody.AddForce(
                (transform.position - m_v3LastPosition) / (Time.realtimeSinceStartup - m_lastPositionTime) * m_fDragThrowMagnitudeScaling,
                m_eReleaseForceMode);
        }

        #endregion
    }
}
