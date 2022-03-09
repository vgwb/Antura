using UnityEngine;

namespace Antura.Hacks
{
    /// <summary>
    /// this is needed because for unkown reasons TMPro texts gets masked by some 3D objects that are BEHIND the text!
    /// waiting for Unity or TMPro to fix it, this behaviour on the camera solves the issue
    /// </summary>
    [RequireComponent(typeof(Camera))]
    public class ClearDepthBuffer : MonoBehaviour
    {
        private void OnPostRender()
        {
            GL.Clear(true, false, Color.black);
        }
    }
}
