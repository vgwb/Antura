using UnityEngine;

namespace Antura.Discover
{
    public class Billboard : MonoBehaviour
    {
        #region Serialized

        [SerializeField] bool keepConstantSize = false;
        [SerializeField] float constantSizeFactor = 1f;

        #endregion

        Transform trans;

        #region Unity

        void Awake()
        {
            trans = this.transform;
        }

        void LateUpdate()
        {
            trans.rotation = CameraManager.I.MainCamTrans.rotation;
            if (keepConstantSize)
                trans.localScale = GetProjectedScale(CameraManager.I.MainCam);
        }

        #endregion

        #region Methods

        Vector3 GetProjectedScale(Camera cam)
        {
            float distance = (cam.transform.position - trans.position).magnitude;
            float size = distance * constantSizeFactor * cam.fieldOfView * 0.0035f;
            return Vector3.one * size;
        }

        #endregion
    }
}