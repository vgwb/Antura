using System;
using Antura.Helpers;
using TMPro;
using UnityEngine;

namespace Antura.UI
{
    /// <summary>
    /// A version of TextMeshPro's text that matches the LL's animations
    /// </summary>
    public class LLText : TextMeshPro
    {
        public bool FixTextAnimation = true;

        private static readonly Vector3 s_DefaultNormal = new Vector3(0.0f, 0.0f, -1f);
        private static readonly Vector4 s_DefaultTangent = new Vector4(-1f, 0.0f, 0.0f, 1f);

        public Transform upperHead;

        int nVertices = 6;  // 6 for a double quad, 4 for a normal quad
        protected override void FillCharacterVertexBuffers(int i, int index_X6)
        {
            if (!FixTextAnimation)
            {
                base.FillCharacterVertexBuffers(i, index_X6);
                return;
            }

            int nChars = m_characterCount;

            if (nVertices == 4)
            {
                // No double quad
                base.FillCharacterVertexBuffers(i, index_X6);
                Warp();
                return;
            }

            int materialIndex = m_textInfo.characterInfo[i].materialReferenceIndex;
            index_X6 = m_textInfo.meshInfo[materialIndex].vertexCount;

            // Check to make sure our current mesh buffer allocations can hold these new Quads.
            if (i == 0)
            {
                // FAKE: we force at least 6 characters, so we know we have enough space for 6 actual chars
                m_textInfo.meshInfo[materialIndex].ResizeMeshInfo(Mathf.NextPowerOfTwo(nVertices*6));
            }

            TMP_CharacterInfo[] characterInfoArray = m_textInfo.characterInfo;
            m_textInfo.characterInfo[i].vertexIndex = index_X6;

            // Setup Vertices for Characters
            //Debug.LogError("Rect " + i + " is " + characterInfoArray[i].vertex_BL.position + " " + characterInfoArray[i].vertex_TL.position + " " + characterInfoArray[i].vertex_TR.position + " " + characterInfoArray[i].vertex_BR.position);
            var halfHeight = (characterInfoArray[i].vertex_TL.position.y - characterInfoArray[i].vertex_BL.position.y) / 2f;
            m_textInfo.meshInfo[materialIndex].vertices[0 + index_X6] = characterInfoArray[i].vertex_BL.position + Vector3.up * halfHeight;
            m_textInfo.meshInfo[materialIndex].vertices[1 + index_X6] = characterInfoArray[i].vertex_TL.position;
            m_textInfo.meshInfo[materialIndex].vertices[2 + index_X6] = characterInfoArray[i].vertex_TR.position;
            m_textInfo.meshInfo[materialIndex].vertices[3 + index_X6] = characterInfoArray[i].vertex_BR.position + Vector3.up * halfHeight;
            m_textInfo.meshInfo[materialIndex].vertices[4 + index_X6] = characterInfoArray[i].vertex_BL.position;
            m_textInfo.meshInfo[materialIndex].vertices[5 + index_X6] = characterInfoArray[i].vertex_BR.position;

            // Setup UVS0
            var halfUV = (characterInfoArray[i].vertex_TL.uv.y - characterInfoArray[i].vertex_BL.uv.y) / 2f;
            m_textInfo.meshInfo[materialIndex].uvs0[0 + index_X6] = characterInfoArray[i].vertex_BL.uv + Vector2.up * halfUV;
            m_textInfo.meshInfo[materialIndex].uvs0[1 + index_X6] = characterInfoArray[i].vertex_TL.uv;
            m_textInfo.meshInfo[materialIndex].uvs0[2 + index_X6] = characterInfoArray[i].vertex_TR.uv;
            m_textInfo.meshInfo[materialIndex].uvs0[3 + index_X6] = characterInfoArray[i].vertex_BR.uv + Vector2.up * halfUV;
            m_textInfo.meshInfo[materialIndex].uvs0[4 + index_X6] = characterInfoArray[i].vertex_BL.uv;
            m_textInfo.meshInfo[materialIndex].uvs0[5 + index_X6] = characterInfoArray[i].vertex_BR.uv;

            // Setup UVS2
            var halfUV2 = (characterInfoArray[i].vertex_TL.uv.y - characterInfoArray[i].vertex_BL.uv.y) / 2f;
            m_textInfo.meshInfo[materialIndex].uvs2[0 + index_X6] = characterInfoArray[i].vertex_BL.uv + Vector2.up * halfUV2;
            m_textInfo.meshInfo[materialIndex].uvs2[1 + index_X6] = characterInfoArray[i].vertex_TL.uv2;
            m_textInfo.meshInfo[materialIndex].uvs2[2 + index_X6] = characterInfoArray[i].vertex_TR.uv2;
            m_textInfo.meshInfo[materialIndex].uvs2[3 + index_X6] = characterInfoArray[i].vertex_BR.uv + Vector2.up * halfUV2;
            m_textInfo.meshInfo[materialIndex].uvs2[4 + index_X6] = characterInfoArray[i].vertex_BL.uv2;
            m_textInfo.meshInfo[materialIndex].uvs2[5 + index_X6] = characterInfoArray[i].vertex_BR.uv2;

            // setup Vertex Colors
            m_textInfo.meshInfo[materialIndex].colors32[0 + index_X6] = characterInfoArray[i].vertex_BL.color;
            m_textInfo.meshInfo[materialIndex].colors32[1 + index_X6] = characterInfoArray[i].vertex_TL.color;
            m_textInfo.meshInfo[materialIndex].colors32[2 + index_X6] = characterInfoArray[i].vertex_TR.color;
            m_textInfo.meshInfo[materialIndex].colors32[3 + index_X6] = characterInfoArray[i].vertex_BR.color;
            m_textInfo.meshInfo[materialIndex].colors32[4 + index_X6] = characterInfoArray[i].vertex_BL.color;
            m_textInfo.meshInfo[materialIndex].colors32[5 + index_X6] = characterInfoArray[i].vertex_BR.color;

            m_textInfo.meshInfo[materialIndex].vertexCount = index_X6 + nVertices;

            var index_X12 = index_X6 * 12 / 6;
            if (i == 0) m_textInfo.meshInfo[materialIndex].triangles = new int[nChars*12];
            m_textInfo.meshInfo[materialIndex].triangles[index_X12 + 0] = 0 + index_X6;
            m_textInfo.meshInfo[materialIndex].triangles[index_X12 + 1] = 1 + index_X6;
            m_textInfo.meshInfo[materialIndex].triangles[index_X12 + 2] = 2 + index_X6;
            m_textInfo.meshInfo[materialIndex].triangles[index_X12 + 3] = 2 + index_X6;
            m_textInfo.meshInfo[materialIndex].triangles[index_X12 + 4] = 3 + index_X6;
            m_textInfo.meshInfo[materialIndex].triangles[index_X12 + 5] = 0 + index_X6;

            m_textInfo.meshInfo[materialIndex].triangles[index_X12 + 6] = 4 + index_X6;
            m_textInfo.meshInfo[materialIndex].triangles[index_X12 + 7] = 0 + index_X6;
            m_textInfo.meshInfo[materialIndex].triangles[index_X12 + 8] = 3 + index_X6;
            m_textInfo.meshInfo[materialIndex].triangles[index_X12 + 9] = 3 + index_X6;
            m_textInfo.meshInfo[materialIndex].triangles[index_X12 + 10] = 4 + index_X6;
            m_textInfo.meshInfo[materialIndex].triangles[index_X12 + 11] = 5 + index_X6;

            // Regenerate normals and tangents for the new data
            if (i == 0) m_textInfo.meshInfo[materialIndex].normals = new Vector3[nChars*6];
            m_textInfo.meshInfo[materialIndex].normals[0 + index_X6] = s_DefaultNormal;
            m_textInfo.meshInfo[materialIndex].normals[1 + index_X6] = s_DefaultNormal;
            m_textInfo.meshInfo[materialIndex].normals[2 + index_X6] = s_DefaultNormal;
            m_textInfo.meshInfo[materialIndex].normals[3 + index_X6] = s_DefaultNormal;
            m_textInfo.meshInfo[materialIndex].normals[4 + index_X6] = s_DefaultNormal;
            m_textInfo.meshInfo[materialIndex].normals[5 + index_X6] = s_DefaultNormal;

            if (i == 0) m_textInfo.meshInfo[materialIndex].tangents = new Vector4[nChars*6];
            m_textInfo.meshInfo[materialIndex].tangents[0 + index_X6] = s_DefaultTangent;
            m_textInfo.meshInfo[materialIndex].tangents[1 + index_X6] = s_DefaultTangent;
            m_textInfo.meshInfo[materialIndex].tangents[2 + index_X6] = s_DefaultTangent;
            m_textInfo.meshInfo[materialIndex].tangents[3 + index_X6] = s_DefaultTangent;
            m_textInfo.meshInfo[materialIndex].tangents[4 + index_X6] = s_DefaultTangent;
            m_textInfo.meshInfo[materialIndex].tangents[5 + index_X6] = s_DefaultTangent;

            //Debug.LogError("Vertices " + m_textInfo.meshInfo[materialIndex].vertices.Length + "\n"+  m_textInfo.meshInfo[materialIndex].vertices.ToJoinedString());
            //Debug.LogError("Triangles " + m_textInfo.meshInfo[materialIndex].triangles.Length + "\n"+  m_textInfo.meshInfo[materialIndex].triangles.ToJoinedString());

            if (i == m_characterCount - 1)
            {
                Warp();
            }
        }

        protected void Warp()
        {
            //Debug.LogError("Update characters");
            var index_X = 0;
            for (int i = 0; i < m_characterCount; i++)
            {
                int materialIndex = m_textInfo.characterInfo[i].materialReferenceIndex;

                var warpTop = (upperHead.transform.position - transform.position - Vector3.forward* 0.12f);
                warpTop *= 3f; // exxagerate a bit
                warpTop.x = 0f;
                warpTop.y = 0f;
                if (warpTop.z > 0) warpTop.z = 0;
                //Debug.LogError("WARP " + warpTop.z);

                TMP_CharacterInfo[] characterInfoArray = m_textInfo.characterInfo;
                if (m_textInfo.meshInfo == null || m_textInfo.meshInfo[materialIndex].vertices == null)
                {
                    break;
                }
                if (index_X >= m_textInfo.meshInfo[materialIndex].vertices.Length)
                {
                    break;
                }

                if (nVertices == 4)
                {
                    m_textInfo.meshInfo[materialIndex].vertices[0 + index_X] = characterInfoArray[i].vertex_BL.position + warpTop * (Mathf.Max(0, characterInfoArray[i].vertex_BL.position.y));
                    m_textInfo.meshInfo[materialIndex].vertices[1 + index_X] = characterInfoArray[i].vertex_TL.position + warpTop * (Mathf.Max(0, characterInfoArray[i].vertex_TL.position.y));
                    m_textInfo.meshInfo[materialIndex].vertices[2 + index_X] = characterInfoArray[i].vertex_TR.position + warpTop * (Mathf.Max(0, characterInfoArray[i].vertex_TR.position.y));
                    m_textInfo.meshInfo[materialIndex].vertices[3 + index_X] = characterInfoArray[i].vertex_BR.position + warpTop * (Mathf.Max(0, characterInfoArray[i].vertex_BR.position.y));
                }
                else if (nVertices == 6)
                {
                    var halfHeight = (characterInfoArray[i].vertex_TL.position.y - characterInfoArray[i].vertex_BL.position.y) / 2f;
                    m_textInfo.meshInfo[materialIndex].vertices[0 + index_X] = characterInfoArray[i].vertex_BL.position + Vector3.up * halfHeight + warpTop *(Mathf.Max(0, characterInfoArray[i].vertex_BL.position.y + halfHeight));
                    m_textInfo.meshInfo[materialIndex].vertices[1 + index_X] = characterInfoArray[i].vertex_TL.position + warpTop * (Mathf.Max(0, characterInfoArray[i].vertex_TL.position.y));
                    m_textInfo.meshInfo[materialIndex].vertices[2 + index_X] = characterInfoArray[i].vertex_TR.position + warpTop * (Mathf.Max(0, characterInfoArray[i].vertex_TR.position.y));
                    m_textInfo.meshInfo[materialIndex].vertices[3 + index_X] = characterInfoArray[i].vertex_BR.position + Vector3.up * halfHeight + warpTop * (Mathf.Max(0, characterInfoArray[i].vertex_BR.position.y + halfHeight));
                    m_textInfo.meshInfo[materialIndex].vertices[4 + index_X] = characterInfoArray[i].vertex_BL.position + warpTop * (Mathf.Max(0, characterInfoArray[i].vertex_BL.position.y));
                    m_textInfo.meshInfo[materialIndex].vertices[5 + index_X] = characterInfoArray[i].vertex_BR.position + warpTop * (Mathf.Max(0, characterInfoArray[i].vertex_BR.position.y));
                }

                /*Debug.LogError("VERTICES " + i + m_textInfo.meshInfo[materialIndex].vertices[0 + index_X] +
                               " " + m_textInfo.meshInfo[materialIndex].vertices[1 + index_X] +
                               " " + m_textInfo.meshInfo[materialIndex].vertices[2 + index_X] +
                               " " + m_textInfo.meshInfo[materialIndex].vertices[3 + index_X] +
                               " " + m_textInfo.meshInfo[materialIndex].vertices[4 + index_X] +
                               " " + m_textInfo.meshInfo[materialIndex].vertices[5 + index_X]);
                */

                index_X += 6;
            }
            havePropertiesChanged = true;
        }

        public void OnPreRender()
        {
            if (!FixTextAnimation) return;
            Warp();
        }

        public void LateUpdate()
        {
            if (!FixTextAnimation) return;
            Warp();
        }
    }
}
