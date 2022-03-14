using Antura.Helpers;
using UnityEngine;
using TMPro;
using Antura.Language;

namespace Antura.UI
{
    public class TextRenderUtility : MonoBehaviour
    {
        private TMP_Text m_TextComponent;
        private TMP_TextInfo textInfo;

        public int yOffset = 10;

        public void AdjustDiacriticPositions()
        {
            m_TextComponent = gameObject.GetComponent<TMP_Text>();
            m_TextComponent.ForceMeshUpdate();
            textInfo = m_TextComponent.textInfo;

            int characterCount = textInfo.characterCount;

            if (characterCount > 1)
            {
                int newYOffset = 0;
                int charPosition = 1;

                if (LanguageSwitcher.I.GetHelper(LanguageUse.Learning).GetHexUnicodeFromChar(textInfo.characterInfo[0].character) == "0627"
                    && LanguageSwitcher.I.GetHelper(LanguageUse.Learning).GetHexUnicodeFromChar(textInfo.characterInfo[1].character) == "064B")
                {
                    newYOffset = 10;
                }

                if (LanguageSwitcher.I.GetHelper(LanguageUse.Learning).GetHexUnicodeFromChar(textInfo.characterInfo[0].character) == "0623"
                    && LanguageSwitcher.I.GetHelper(LanguageUse.Learning).GetHexUnicodeFromChar(textInfo.characterInfo[1].character) == "064E")
                {
                    newYOffset = 16;
                }

                if (LanguageSwitcher.I.GetHelper(LanguageUse.Learning).GetHexUnicodeFromChar(textInfo.characterInfo[0].character) == "0639"
                    && LanguageSwitcher.I.GetHelper(LanguageUse.Learning).GetHexUnicodeFromChar(textInfo.characterInfo[1].character) == "0650")
                {
                    newYOffset = -25;
                }

                if (newYOffset != 0)
                {
                    // Cache the vertex data of the text object as the Jitter FX is applied to the original position of the characters.
                    TMP_MeshInfo[] cachedMeshInfo = textInfo.CopyMeshInfoVertexData();

                    // Get the index of the material used by the current character.
                    int materialIndex = textInfo.characterInfo[charPosition].materialReferenceIndex;

                    // Get the index of the first vertex used by this text element.
                    int vertexIndex = textInfo.characterInfo[charPosition].vertexIndex;

                    // Get the cached vertices of the mesh used by this text element (character or sprite).
                    Vector3[] sourceVertices = cachedMeshInfo[materialIndex].vertices;

                    // Determine the center point of each character at the baseline.
                    //Vector2 charMidBasline = new Vector2((sourceVertices[vertexIndex + 0].x + sourceVertices[vertexIndex + 2].x) / 2, charInfo.baseLine);
                    // Determine the center point of each character.
                    float dy = (sourceVertices[vertexIndex + 2].y - sourceVertices[vertexIndex + 0].y);
                    Vector3 offset = new Vector3(0f, dy, 0f);

                    Vector3[] destinationVertices = textInfo.meshInfo[materialIndex].vertices;

                    destinationVertices[vertexIndex + 0] = sourceVertices[vertexIndex + 0] + offset;
                    destinationVertices[vertexIndex + 1] = sourceVertices[vertexIndex + 1] + offset;
                    destinationVertices[vertexIndex + 2] = sourceVertices[vertexIndex + 2] + offset;
                    destinationVertices[vertexIndex + 3] = sourceVertices[vertexIndex + 3] + offset;

                    for (int i = 0; i < textInfo.meshInfo.Length; i++)
                    {
                        textInfo.meshInfo[i].mesh.vertices = textInfo.meshInfo[i].vertices;
                        m_TextComponent.UpdateGeometry(textInfo.meshInfo[i].mesh, i);
                    }

                    //Debug.Log("DIACRITIC: diacritic pos fixed for " +
                    //                              ArabicAlphabetHelper.GetHexUnicodeFromChar(textInfo.characterInfo[1].character) + " by " + newYOffset);
                }


                //for (int i = 0; i < characterCount; i++) {
                //    Debug.Log("DIACRITIC: " + i
                //              //+ "index: " + textInfo.characterInfo[i].index
                //              + " char: " + textInfo.characterInfo[i].character.ToString()
                //              + " UNICODE: " + ArabicAlphabetHelper.GetHexUnicodeFromChar(textInfo.characterInfo[i].character)
                //    );
                //}
            }
        }

        public void ShowInfo()
        {
            m_TextComponent = gameObject.GetComponent<TMP_Text>();
            textInfo = m_TextComponent.textInfo;

            int characterCount = textInfo.characterCount;

            if (characterCount > 1)
            {
                for (int i = 0; i < characterCount; i++)
                {
                    //Debug.Log("CAHR " + characterCount + ": " + TMPro.TMP_TextUtilities.StringToInt(textInfo.characterInfo[characterCount].character.ToString()));
                    Debug.Log("CHAR: " + i
                              + "index: " + textInfo.characterInfo[i].index
                              + "char: " + textInfo.characterInfo[i].character.ToString()
                              + "UNICODE: " + LanguageSwitcher.I.GetHelper(LanguageUse.Learning).GetHexUnicodeFromChar(textInfo.characterInfo[i].character)
                    );
                }
                //textInfo.characterInfo[1].textElement.yOffset += yOffset;
            }
        }
    }
}
