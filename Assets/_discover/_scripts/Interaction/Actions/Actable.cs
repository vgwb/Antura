using UnityEngine;

namespace Antura.Discover
{
    /// <summary>
    /// Attach to any GameObject that needs to be "actionable" (i.e. can perform an action when Trigger() is called).
    /// </summary>
    public class Actable : ActionAbstract
    {

        void Awake()
        {
        }

        public override void OnTrigger()
        {
        }
    }
}
