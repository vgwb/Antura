using Antura.Book;
using Antura.Core;
using Antura.Profile;
using Antura.Teacher;
using Antura.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Antura.ReservedArea
{
    public class ProfilesPanel : MonoBehaviour
    {
        [Header("References")]
        public TextRender PlayerInfoText;

        public GameObject PlayerIconContainer;
        public GameObject PlayerIconPrefab;
        public GameObject ProfileCommandsContainer;
        public GameObject pleaseWaitPanel;

        private string SelectedPlayerId;

        void Start()
        {
            ResetAll();
        }

        void ResetAll()
        {
            SelectedPlayerId = "";
            RefreshPlayerIcons();
            RefreshUI();
        }

        void RefreshPlayerIcons()
        {
            foreach (Transform t in PlayerIconContainer.transform)
            {
                Destroy(t.gameObject);
            }

            List<PlayerIconData> players = AppManager.I.PlayerProfileManager.GetPlayersIconData();

            foreach (var player in players)
            {
                var newIcon = Instantiate(PlayerIconPrefab);
                newIcon.transform.SetParent(PlayerIconContainer.transform, false);
                newIcon.GetComponent<PlayerIcon>().Init(player);
                newIcon.GetComponent<UIButton>().Bt.onClick.AddListener(() => OnSelectPlayerProfile(player.Uuid));
            }
        }

        void RefreshUI()
        {
            // highlight selected profile
            ProfileCommandsContainer.SetActive(SelectedPlayerId != "");
            SetPlayerInfoText();
            foreach (Transform t in PlayerIconContainer.transform)
            {
                t.GetComponent<PlayerIcon>().Select(SelectedPlayerId);
            }
        }

        public void OnSelectPlayerProfile(string uuid)
        {
            //Debug.Log("OnSelectPlayerProfile " + uuid);
            SelectedPlayerId = SelectedPlayerId != uuid ? uuid : "";
            RefreshUI();
        }

        void SetPlayerInfoText()
        {
            if (SelectedPlayerId != "")
            {
                PlayerInfoText.text = "player id: " + SelectedPlayerId;
            }
            else
            {
                PlayerInfoText.text = "";
            }
        }

        public void OnOpenSelectedPlayerProfile()
        {
            //Debug.Log("OPEN " + SelectedPlayerId);
            AppManager.I.PlayerProfileManager.SetPlayerAsCurrentByUUID(SelectedPlayerId);
            BookManager.I.OpenBook(BookArea.Player);
        }

        public void OnDeleteSelectPlayerProfile()
        {
            GlobalUI.ShowPrompt(id: Database.LocalizationDataId.UI_AreYouSure, _onYesCallback: DoDeleteSelectPlayerProfile, _onNoCallback: DoNothing);
        }

        void DoNothing()
        {
        }

        void DoDeleteSelectPlayerProfile()
        {
            //Debug.Log("DELETE " + SelectedPlayerId);
            AppManager.I.PlayerProfileManager.DeletePlayerProfile(SelectedPlayerId);
            ResetAll();
        }

        public void OnExportSelectPlayerProfile()
        {
            if (AppManager.I.DB.ExportPlayerDb(SelectedPlayerId))
            {
                string dbPath;
                if (Application.platform == RuntimePlatform.IPhonePlayer)
                {
                    dbPath = string.Format(@"{0}/{1}", AppConfig.DbExportFolder,
                        AppConfig.GetPlayerDatabaseFilename(SelectedPlayerId));
                    GlobalUI.ShowPrompt("Get the DB from iTunes app:\n" + dbPath);
                }
                else
                {
                    // Android or Desktop
                    dbPath = string.Format(@"{0}/{1}/{2}", Application.persistentDataPath, AppConfig.DbExportFolder,
                        AppConfig.GetPlayerDatabaseFilename(SelectedPlayerId));
                    GlobalUI.ShowPrompt("The DB is here:\n" + dbPath);
                }
            }
            else
            {
                GlobalUI.ShowPrompt("Could not export the database.");
            }
        }
    }
}
