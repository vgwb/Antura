using Antura.LivingLetters;
using Antura.Core;
using Antura.Helpers;
using UnityEngine;

namespace Antura.Intro
{
    /// <summary>
    /// A special LivingLetter character with special animations.
    /// </summary>
    // TODO refactor: remove the references to the Maze minigame
    public class IntroRocketCharacter : MonoBehaviour
    {
        public LivingLetterController LL;
        //public List<GameObject> particles;

        [HideInInspector]
        public float m_Velocity;

        bool m_Move = false;
        Vector3 Destination;
        Vector3 Path;

        void Start()
        {
            LL.Init(AppManager.I.Teacher.GetAllTestLetterDataLL().GetRandom());
            LL.SetState(LLAnimationStates.LL_rocketing);
            LL.Horraying = true;
        }

        public void SetDestination()
        {
            Destination = transform.position;
            Destination -= new Vector3(200, 0, 0);
            Path = transform.position - Destination;
            m_Move = true;
        }

        //public void toggleVisibility(bool value)
        //{
        //    foreach (GameObject particle in particles) particle.SetActive(value);
        //}

        void Update()
        {
            if (m_Move)
            {
                if (transform.position.x > Destination.x)
                {
                    transform.position -= Path * Time.deltaTime * m_Velocity;
                }
                else
                {
                    m_Move = false;
                    gameObject.SetActive(false);
                }
            }
        }
    }
}
