using Antura.Core;
using Antura.UI;
using UnityEngine;

namespace Antura.ReservedArea
{
    public class ReservedAreaScene : SceneBase
    {

        protected override void Start()
        {
            base.Start();

            GlobalUI.ShowPauseMenu(false);
            GlobalUI.ShowBackButton(true);

        }
    }
}
