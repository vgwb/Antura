using Antura.Dog;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Assertions;

namespace Antura.Minigames.MissingLetter
{
    public class AnturaBehaviour : MonoBehaviour
    {

        void Start()
        {
            m_oAnturaCtrl = GetComponent<AnturaAnimationController>();
            Assert.IsNotNull<AnturaAnimationController>(m_oAnturaCtrl, "Add Antura Script to " + name);
            transform.position = m_oStart.position;
            m_oNextPos = m_oEnd;
        }

        /// <summary>
        /// make Antura cross scene, DoShout
        /// </summary>
        /// <param name="_duration"> duration of enter/cross scene action </param>
        public void EnterScene(float _duration)
        {
            m_oAnturaCtrl.State = AnturaAnimationStates.walking;
            m_oAnturaCtrl.IsAngry = true;
            m_oAnturaCtrl.DoShout();
            transform.LookAt(transform.position + Vector3.left * (m_oNextPos.position.x - transform.position.x));
            transform.DOMove(m_oNextPos.position, _duration).OnComplete(delegate
            { m_oAnturaCtrl.State = AnturaAnimationStates.idle; });

            m_oNextPos = m_oNextPos == m_oStart ? m_oEnd : m_oStart;
        }

        [SerializeField, HideInInspector]
        public Transform m_oStart, m_oEnd;

        private Transform m_oNextPos;
        private AnturaAnimationController m_oAnturaCtrl;
    }
}
