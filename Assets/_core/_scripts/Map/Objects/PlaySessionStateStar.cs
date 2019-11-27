using Antura.Core;
using UnityEngine;

namespace Antura.Map
{
    public class PlaySessionStateStar : MonoBehaviour
    {
        public MeshRenderer MeshRenderer;

        public Material obtainedMaterial;
        public Material unobtainedMaterial;

        public void SetObtained(bool choice)
        {
            MeshRenderer.material = choice ? obtainedMaterial : unobtainedMaterial;
        }

    }
}