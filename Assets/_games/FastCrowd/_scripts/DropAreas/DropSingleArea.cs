using Antura.Helpers;
using Antura.LivingLetters;
using Antura.UI;
using UnityEngine;
using TMPro;

namespace Antura.Minigames.FastCrowd
{
    public class DropSingleArea : MonoBehaviour
    {
        public TMP_FontAsset normalFont;
        public TMP_FontAsset numbersFont;

        public TMP_Text LetterLable;
        public TextMeshPro DrawText;
        public ILivingLetterData Data;
        public DropContainer DropContain;

        Vector3 enabledPos, disabledPos;

        #region Api
        public void Init(ILivingLetterData _data, DropContainer _dropContainer, bool asImage)
        {
            DropContain = _dropContainer;
            Data = _data;
            if (asImage && !(_data is LL_LetterData)) {
                LetterLable.text = string.Empty;
                DrawText.gameObject.SetActive(true);

                DrawText.text = Data.DrawingCharForLivingLetter;
            } else {
                LetterLable.font = normalFont;
                LetterLable.GetComponent<TextRender>().SetLetterData(Data);
                DrawText.gameObject.SetActive(false);
            }

            AreaState = State.disabled;
        }

        public void Init(ILivingLetterData _data, int text, DropContainer _dropContainer)
        {
            DropContain = _dropContainer;
            Data = _data;

            LetterLable.font = numbersFont;
            LetterLable.text = GenericHelper.ReverseText(text.ToString());
            DrawText.gameObject.SetActive(false);

            AreaState = State.disabled;
        }

        public void Init(ILivingLetterData _data, string text, DropContainer _dropContainer)
        {
            DropContain = _dropContainer;
            Data = _data;

            LetterLable.font = normalFont;
            LetterLable.text = GenericHelper.ReverseText(text);
            DrawText.gameObject.SetActive(false);

            AreaState = State.disabled;
        }

        /// <summary>
        /// 
        /// </summary>
        public void SetDisbled()
        {
            AreaState = State.disabled;
        }

        /// <summary>
        /// 
        /// </summary>
        public void SetEnabled()
        {
            AreaState = State.enabled;
        }

        /// <summary>
        /// Set Matching state.
        /// </summary>
        public void SetMatching()
        {
            AreaState = State.matching;
        }

        /// <summary>
        /// Set Matching state.
        /// </summary>
        public void SetMatchingWrong()
        {
            AreaState = State.matching_wrong;
        }

        /// <summary>
        /// Automatically return to state pre matching.
        /// </summary>
        public void DeactivateMatching()
        {

            if (GetComponent<Collider>().enabled) {
                AreaState = State.enabled;
            } else {
                AreaState = State.disabled;
            }
        }
        #endregion

        /// <summary>
        /// Stete of drop Area.
        /// </summary>
        public State AreaState
        {
            get { return areaState; }
            protected set {
                if (areaState != value) {
                    areaState = value;
                    areaStateChanged();
                } else {
                    areaState = value;
                }

            }
        }
        private State areaState;


        /// <summary>
        /// Effects to state change.
        /// </summary>
        void areaStateChanged()
        {
            switch (AreaState) {
                case State.enabled:
                    GetComponent<Collider>().enabled = true;
                    GetComponent<MeshRenderer>().materials[0].color = Color.yellow;
                    break;
                case State.disabled:
                    GetComponent<Collider>().enabled = false;
                    GetComponent<MeshRenderer>().materials[0].color = Color.gray;
                    break;
                case State.matching:
                    // Matching preview right
                    GetComponent<MeshRenderer>().materials[0].color = Color.green;
                    break;
                case State.matching_wrong:
                    // Matching preview wrong
                    GetComponent<MeshRenderer>().materials[0].color = new Color(248, 0, 0);
                    break;
            }
        }


        public enum State
        {
            isnull,
            disabled,
            enabled,
            matching,
            matching_wrong,
        }
    }
}
