using UnityEngine;

namespace Antura.AnturaSpace
{
    /// <summary>
    /// Controls the shadows of bones thrown to Antura in AnturaSpace.
    /// Keeps the shadow at the same y value
    /// </summary>
    public class BoneShadowController : MonoBehaviour
    {
#pragma warning disable 649
        [SerializeField]
        private float m_fWorldY;

        [SerializeField]
        private Transform m_oTarget;
#pragma warning restore 649

        private Quaternion m_oOriginalRotation;

        void Start()
        {
            m_oOriginalRotation = gameObject.transform.rotation;
        }

        void Update()
        {
            //restore rotation to default
            gameObject.transform.rotation = m_oOriginalRotation;

            // position under the target at the given height
            gameObject.transform.position = new Vector3(m_oTarget.position.x, m_fWorldY, m_oTarget.position.z);
        }
    }
}
