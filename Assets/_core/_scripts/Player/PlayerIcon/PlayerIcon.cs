using Antura.Core;
using Antura.Database;
using Antura.UI;
using DG.DeExtensions;
using DG.DeInspektor.Attributes;
using TMPro;
using System;
using System.Collections.Generic;
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
        public bool NonInteractable;
        [DeComment("Level is auto-hidden if the player has the hat", style = DeCommentStyle.WrapNextLine, marginBottom = -1)]
        public bool HideLevel;
        public bool HideName;
        public bool HideBackground;
        public bool HideHighlight;
        public float LevelLabelHatShift = 40;
        public Color SelectedPlayerNameColor = Color.white;
        public Color SelectedTfNameColor = Color.white;

        [Header("References")]
        public Image Background;
        public Sprite EndgameHat;
        public Sprite EndgameHatWStars;
        public GameObject Highlight;
        public GameObject HighlightImage;
        public Image HatImage;
        public Image FaceImg, HairImg;
        public TextRender LevelLabel;
        public TMP_Text TfName;
        public Image PlayerNameBox;
        public string Uuid { get; private set; }

        public UIButton UIButton
        {
            get
            {
                if (fooUIButton == null)
                {
                    fooUIButton = this.GetComponent<UIButton>();
                }
                return fooUIButton;
            }
        }

        Color defTfNameColor;
        Color defPlayerNameBoxColor;
        UIButton fooUIButton;
        RectTransform levelLabelRT;
        Vector2 orLevelLabelPosition;
        bool isDemoUser;
        Color defFaceColor, defHairColor;

        #region Unity

        void Awake()
        {
            defTfNameColor = TfName.color;
            defPlayerNameBoxColor = PlayerNameBox.color;

            PlayerNameBox.gameObject.SetActive(!HideName);
            Background.gameObject.SetActive(!HideBackground);
            Highlight.SetActive(!HideHighlight);

            if (NonInteractable)
            {
                this.GetComponent<Button>().enabled = false;
                UIButton.enabled = false;
                Image[] raycastBlockers = this.GetComponentsInChildren<Image>(true);
                foreach (Image img in raycastBlockers)
                    img.raycastTarget = false;
            }
        }

        void Start()
        {
            if (!AutoInit)
                return;

            if (AppManager.I.PlayerProfileManager.CurrentPlayer != null)
            {
                Init(AppManager.I.PlayerProfileManager.CurrentPlayer.GetPlayerPreview());
            }
        }

        #endregion

        #region Public

        public void Init(PlayerProfilePreview playerIconData)
        {
            if (levelLabelRT == null)
            {
                levelLabelRT = LevelLabel.GetComponent<RectTransform>();
                orLevelLabelPosition = levelLabelRT.anchoredPosition;
            }
            Uuid = playerIconData.Uuid;
            //Debug.Log("playerIconData " + playerIconData.Uuid + " " + playerIconData.Gender + " " + playerIconData.AvatarId + " " + playerIconData.Tint + " " + playerIconData.IsDemoUser + " > " + playerIconData.HasFinishedTheGame + "/" + playerIconData.HasFinishedTheGameWithAllStars);
            // EndgameState endgameState = playerIconData.HasFinishedTheGameWithAllStars
            //     ? EndgameState.FinishedWAllStars
            //     : playerIconData.HasFinishedTheGame
            //         ? EndgameState.Finished
            //         : EndgameState.Unfinished;
            SetAppearance(playerIconData, EndgameState.Unfinished);
        }

        [DeMethodButton("DEBUG: Select", mode = DeButtonMode.PlayModeOnly)]
        public void Select(string _uuid)
        {
            bool isOn = Uuid == _uuid;
            UIButton.Toggle(isOn);
            if (isOn)
            {
                TfName.color = SelectedTfNameColor;
                PlayerNameBox.color = SelectedPlayerNameColor;
                PlayerNameBox.color.SetAlpha(1f);
                if (FaceImg != null)
                    FaceImg.color = isDemoUser ? FaceImg.color.SetAlpha(1f) : defFaceColor;
                if (HairImg != null)
                    HairImg.color = defHairColor;
                if (HatImage != null)
                    HatImage.color = HatImage.color.SetAlpha(1f);

            }
            else
            {
                TfName.color = defTfNameColor;
                PlayerNameBox.color = defPlayerNameBoxColor;
                PlayerNameBox.SetAlpha(0.65f);
                if (FaceImg != null)
                    FaceImg.color = isDemoUser ? FaceImg.color.SetAlpha(0.5f) : FaceImg.color.ChangeSaturation(0.35f);
                if (HairImg != null)
                    HairImg.color = HairImg.color.ChangeSaturation(0.35f);
                if (HatImage != null)
                    HatImage.color = HatImage.color.SetAlpha(0.5f);
            }
        }

        [DeMethodButton("DEBUG: Deselect", mode = DeButtonMode.PlayModeOnly)]
        public void Deselect()
        {
            UIButton.Toggle(false);
        }

        #endregion

        void SetAppearance(PlayerProfilePreview playerIconData, EndgameState endgameState)
        {
            //            if (playerIconData.Gender == PlayerGender.None) {
            //                Debug.LogWarning("Player gender set to NONE");
            //            }
            isDemoUser = playerIconData.IsDemoUser;
            Color color = isDemoUser ? new Color(0.4117647f, 0.9254903f, 1f, 1f) : playerIconData.BgColor;
            //            UIButton.Ico = FaceImg;   // forced icon
            //            UIButton.ChangeDefaultColors(color, color.SetAlpha(0.5f));

            if (isDemoUser)
            {
                FaceImg.sprite = Resources.Load<Sprite>($"{AppConfig.RESOURCES_DIR_AVATARS}god");
                HairImg.sprite = null;
            }
            else
            {
                if (playerIconData.IsOldAvatar)
                {
                    color = PlayerTintConverter.ToColor(playerIconData.Tint);
                    FaceImg.sprite = Resources.Load<Sprite>(AppConfig.RESOURCES_DIR_AVATARS + (playerIconData.Gender == PlayerGender.Undefined ? "M" : playerIconData.Gender.ToString()) + playerIconData.AvatarId);
                }
                else
                {
                    FaceImg.sprite = Resources.Load<Sprite>($"{AppConfig.RESOURCES_DIR_AVATARS}AvatarV2_{(playerIconData.NewAvatarId + 1)}_face");
                    HairImg.sprite = Resources.Load<Sprite>($"{AppConfig.RESOURCES_DIR_AVATARS}AvatarV2_{(playerIconData.NewAvatarId + 1)}_hair");
                }
            }

            UIButton.ChangeDefaultColors(color, color.ChangeSaturation(0.35f));

            defFaceColor = FaceImg.color = (isDemoUser || playerIconData.IsOldAvatar) ? Color.white : playerIconData.SkinColor;
            if (HairImg.sprite != null)
                defHairColor = HairImg.color = playerIconData.HairColor;
            HairImg.gameObject.SetActive(!isDemoUser && !playerIconData.IsOldAvatar);
            bool hasHat = endgameState != EndgameState.Unfinished;
            HatImage.gameObject.SetActive(hasHat);
            HatImage.color = HatImage.color.SetAlpha(1);
            levelLabelRT.anchoredPosition = hasHat ? orLevelLabelPosition + new Vector2(0, LevelLabelHatShift) : orLevelLabelPosition;

            switch (endgameState)
            {
                case EndgameState.Finished:
                    HatImage.sprite = EndgameHat;
                    break;
                case EndgameState.FinishedWAllStars:
                    HatImage.sprite = EndgameHatWStars;
                    break;
            }
            // if (HideLevel || hasHat)
            // {
            //     LevelLabel.text = "";
            // }
            // else
            // {
            //     // LevelLabel.text = $"{playerIconData.MaxJourneyPosition.Stage}-{playerIconData.MaxJourneyPosition.LearningBlock}";
            //     // TODO
            //     LevelLabel.text = "";
            // }
            LevelLabel.text = playerIconData.PlayerName;

            TfName.text = playerIconData.PlayerName.IsNullOrEmpty() ? "-" : playerIconData.PlayerName.ToUpper();

            // Debug.Log("hasMaxStarsInCurrentPlaySessions: " + hasMaxStarsInCurrentPlaySessions);
            //HighlightImage.SetActive(playerIconData.HasMaxStarsInCurrentPlaySessions);

            //            if (playerIconData.Gender == PlayerGender.None) {
            //                Debug.LogWarning("Player gender set to NONE");
            //            }
            //            Color color = isDemoUser ? new Color(0.4117647f, 0.9254903f, 1f, 1f) : PlayerTintConverter.ToColor(playerIconData.Tint);
            //            UIButton.Ico = IconImage;   // forced icon
            //            UIButton.ChangeDefaultColors(color, color.SetAlpha(0.5f));
            //            UIButton.Ico.sprite = isDemoUser
            //                ? Resources.Load<Sprite>(AppConfig.RESOURCES_DIR_AVATARS + "god")
            //                : Resources.Load<Sprite>(AppConfig.RESOURCES_DIR_AVATARS + (playerIconData.Gender == PlayerGender.None ? "M" : playerIconData.Gender.ToString()) +
            //                                         playerIconData.AvatarId);
            //            bool hasHat = endgameState != EndgameState.Unfinished;
            //            HatImage.gameObject.SetActive(hasHat);
            //            levelLabelRT.anchoredPosition = hasHat ? orLevelLabelPosition + new Vector2(0, LevelLabelHatShift) : orLevelLabelPosition;
            //
            //            switch (endgameState) {
            //                case EndgameState.Finished:
            //                    HatImage.sprite = EndgameHat;
            //                    break;
            //                case EndgameState.FinishedWAllStars:
            //                    HatImage.sprite = EndgameHatWStars;
            //                    break;
            //            }
            //            if (HideLevel || hasHat) {
            //                LevelLabel.text = "";
            //            } else {
            //                LevelLabel.text = playerIconData.MaxJourneyPosition.Stage.ToString() + "-" + playerIconData.MaxJourneyPosition.LearningBlock.ToString();
            //            }
            //
            //            // Debug.Log("hasMaxStarsInCurrentPlaySessions: " + hasMaxStarsInCurrentPlaySessions);
            //            HighlightImage.SetActive(playerIconData.HasMaxStarsInCurrentPlaySessions);
        }

        [DeMethodButton("DEBUG: Randomize Appearance", mode = DeButtonMode.PlayModeOnly)]
        void RandomizeAppearance()
        {
            float rnd0 = UnityEngine.Random.value;
            float rnd1 = UnityEngine.Random.value;
            float rnd2 = UnityEngine.Random.value;
            float rnd3 = UnityEngine.Random.value;
            var rndPlayerIconData = new PlayerProfilePreview(Uuid = "",
                                                        "",
                                                        0,
                                                       UnityEngine.Random.Range(0, 7),
                                                       PlayerGender.M,
                                                       PlayerTint.Blue,
                                                       UnityEngine.Random.ColorHSV(),
                                                       UnityEngine.Random.ColorHSV(),
                                                       UnityEngine.Random.ColorHSV(),
                                                       rnd3 <= 0.5f,
                                                       AppEditionID.LearnEnglish,
                                                       "TEST",
                                                       new PetData()
                                                       );
            SetAppearance(rndPlayerIconData,
                rnd2 < 0.33f ? EndgameState.Unfinished : rnd2 < 0.66f ? EndgameState.Finished : EndgameState.FinishedWAllStars
            );
        }
    }
}
