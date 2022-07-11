using Antura.Core;
using UnityEngine;

namespace Antura.Database.Management
{
    public class EditorContentHolder : MonoBehaviour
    {
        public ContentEditionConfig InputContent => AppManager.I.ContentEdition;
    }
}
