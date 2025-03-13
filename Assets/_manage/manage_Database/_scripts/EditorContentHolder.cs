using Antura.Core;
using UnityEngine;

namespace Antura.Database.Management
{
    public class EditorContentHolder : MonoBehaviour
    {
        public ContentConfig InputContent => AppManager.I.ContentEdition;
    }
}
