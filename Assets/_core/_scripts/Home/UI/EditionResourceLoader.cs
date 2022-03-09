using Antura.Core;
using UnityEngine;

namespace Antura.UI
{
    public class EditionResourceLoader : MonoBehaviour
    {
        public EditionResourceID ResourceId;

        void Start()
        {
            var resource = AppManager.I.ContentEdition.GetResource(ResourceId);
            if (resource == null)
                return;
            var go = Instantiate(resource, transform);
            go.transform.localScale = Vector3.one;
            go.transform.localPosition = Vector3.zero;
        }

    }
}
