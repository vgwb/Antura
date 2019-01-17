using Antura.AnturaSpace.UI;
using Antura.Core;
using Antura.Profile;
using Antura.UI;
using UnityEngine;

namespace Antura.Map
{
    public class AnturaButton : UIButton
    {
        void Start()
        {
            GameObject icoNew = GetComponentInChildren<AnturaSpaceNewIcon>().gameObject;
            icoNew.SetActive(FirstContactManager.I.IsSequenceFinished() && AppManager.I.RewardSystemManager.IsThereSomeNewReward());
        }
    }
}