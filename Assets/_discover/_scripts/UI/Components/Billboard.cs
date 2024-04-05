using UnityEngine;

namespace Antura.Minigames.DiscoverCountry
{
    public class Billboard : MonoBehaviour
    {
        Transform trans;
        
        #region Unity

        void Awake()
        {
            trans = this.transform;
        }

        void LateUpdate()
        {
            trans.rotation = CameraManager.I.MainCamTrans.rotation;
        }

        #endregion
    }
}