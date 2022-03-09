using Kore.Utils;
using System.Collections.Generic;

namespace Antura.Assessment
{
    /// <summary>
    /// Implement IUpdater
    /// </summary>
    public class Updater : SceneScopedSingleton<Updater>
    {
        List<ITimedUpdate> updates = null;

        public void AddTimedUpdate(ITimedUpdate timedUpdate)
        {
            if (updates == null)
            {
                updates = new List<ITimedUpdate>();
            }
            updates.Add(timedUpdate);
        }

        public void UpdateDelta(float delta)
        {
            if (updates != null)
            {
                foreach (var u in updates)
                {
                    u.Update(delta);
                }
            }
        }

        public void Clear()
        {
            updates = null;
        }
    }
}
