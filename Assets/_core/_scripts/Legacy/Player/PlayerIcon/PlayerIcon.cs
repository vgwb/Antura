#if FALSE
using Antura.Core;
using Antura.UI;
using DG.DeExtensions;
using DG.DeInspektor.Attributes;
using UnityEngine;
using UnityEngine.UI;

namespace Antura.Profile
{
    [RequireComponent(typeof(UIButton))]
    public class PlayerIcon : MonoBehaviour
    {
        enum EndgameState
        {
            Unfinished,
            Finished,
            FinishedWAllStars
        }

        [Header("Settings")]
        [Tooltip("If TRUE automatically initializes to the current player")]
        public bool AutoInit;
        [DeComment("Level is auto-hidden if the player has the hat", style = DeCommentStyle.WrapNextLine, marginBottom = -1)]
        public bool HideLevel;
        public float LevelLabelHatShift = 40;

        [Header("References")]
        public Image Background;
        public Sprite EndgameHat;
        public Sprite EndgameHatWStars;
        public GameObject HighlightImage;
        public Image HatImage;
        public Image IconImage;
        public TextRender LevelLabel;
        public string Uuid { get; private set; }

        public UIButton UIButton
        {
            get {
                if (fooUIButton == null) {
                    fooUIButton = this.GetComponent<UIButton>();
                }
                return fooUIButton;
            }
        }

        UIButton fooUIButton;
        RectTransform levelLabelRT;
        Vector2 orLevelLabelPosition;

#region Unity

        void Start()
        {
            if (!AutoInit) { return; }

            if (AppManager.I.PlayerProfileManager.CurrentPlayer != null) {
                Init(AppManager.I.PlayerProfileManager.CurrentPlayer.GetPlayerIconData());
            }
        }

#endregion

#region Public

        public void Init(PlayerIconData playerIconData)
        {
            if (levelLabelRT == null) {
                levelLabelRT = LevelLabel.GetComponent<RectTransform>();
                orLevelLabelPosition = levelLabelRT.anchoredPosition;
            }
            Uuid = playerIconData.Uuid;
            //Debug.Log("playerIconData " + playerIconData.Uuid + " " + playerIconData.Gender + " " + playerIconData.AvatarId + " " + playerIconData.Tint + " " + playerIconData.IsDemoUser + " > " + playerIconData.HasFinishedTheGame + "/" + playerIconData.HasFinishedTheGameWithAllStars);
            EndgameState endgameState = playerIconData.HasFinishedTheGameWithAllStars
                ? EndgameState.FinishedWAllStars
                : playerIconData.HasFinishedTheGame
                    ? EndgameState.Finished
                    : EndgameState.Unfinished;
            SetAppearance(playerIconData, endgameState);
        }

        [DeMethodButton("DEBUG: Select", mode = DeButtonMode.PlayModeOnly)]
        public void Select(string _uuid)
        {
            UIButton.Toggle(Uuid == _uuid);
        }

        [DeMethodButton("DEBUG: Deselect", mode = DeButtonMode.PlayModeOnly)]
        public void Deselect()
        {
            UIButton.Toggle(false);
        }

#endregion

        void SetAppearance(PlayerIconData playerIconData, EndgameState endgameState)
        {
            if (playerIconData.Gender == PlayerGender.None) {
                Debug.LogWarning("Player gender set to NONE");
            }
            Color color = playerIconData.IsDemoUser ? new Color(0.4117647f, 0.9254903f, 1f, 1f) : PlayerTintConverter.ToColor(playerIconData.Tint);
            UIButton.Ico = IconImage;   // forced icon
            UIButton.ChangeDefaultColors(color, color.SetAlpha(0.5f));
            UIButton.Ico.sprite = playerIconData.IsDemoUser
                ? Resources.Load<Sprite>(AppConfig.RESOURCES_DIR_AVATARS + "god")
                : Resources.Load<Sprite>(AppConfig.RESOURCES_DIR_AVATARS + (playerIconData.Gender == PlayerGender.None ? "M" : playerIconData.Gender.ToString()) +
                                         playerIconData.AvatarId);
            bool hasHat = endgameState != EndgameState.Unfinished;
            HatImage.gameObject.SetActive(hasHat);
            levelLabelRT.anchoredPosition = hasHat ? orLevelLabelPosition + new Vector2(0, LevelLabelHatShift) : orLevelLabelPosition;

            switch (endgameState) {
                case EndgameState.Finished:
                    HatImage.sprite = EndgameHat;
                    break;
                case EndgameState.FinishedWAllStars:
                    HatImage.sprite = EndgameHatWStars;
                    break;
            }
            if (HideLevel || hasHat) {
                LevelLabel.text = "";
            } else {
                LevelLabel.text = playerIconData.MaxJourneyPosition.Stage.ToString() + "-" + playerIconData.MaxJourneyPosition.LearningBlock.ToString();
            }

            // Debug.Log("hasMaxStarsInCurrentPlaySessions: " + hasMaxStarsInCurrentPlaySessions);
            HighlightImage.SetActive(playerIconData.HasMaxStarsInCurrentPlaySessions);
        }

        [DeMethodButton("DEBUG: Randomize Appearance", mode = DeButtonMode.PlayModeOnly)]
        void RandomizeAppearance()
        {
            float rnd0 = UnityEngine.Random.value;
            float rnd1 = UnityEngine.Random.value;
            float rnd2 = UnityEngine.Random.value;
            float rnd3 = UnityEngine.Random.value;
            var rndPlayerIconData = new PlayerIconData(Uuid = "",
                                                       UnityEngine.Random.Range(1, 5),
                                                       rnd0 <= 0.5f ? PlayerGender.F : PlayerGender.M,
                                                       (PlayerTint)UnityEngine.Random.Range(1, 8),
                                                      rnd1 <= 0.2f,
                                                      rnd3 <= 0.5f,
                                                      rnd3 <= 0.5f,
                                                      rnd3 <= 0.5f,
                                                       new JourneyPosition(UnityEngine.Random.Range(1, 6), UnityEngine.Random.Range(1, 15), 1));
            SetAppearance(rndPlayerIconData,
                rnd2 < 0.33f ? EndgameState.Unfinished : rnd2 < 0.66f ? EndgameState.Finished : EndgameState.FinishedWAllStars
            );
        }
    }
}
#endif
