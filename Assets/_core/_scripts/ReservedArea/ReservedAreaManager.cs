using Antura.Core;
using Antura.Database;
using Antura.Debugging;
using Antura.UI;
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Demigiant.DemiTools.DeUnityExtended;
using DG.DeInspektor.Attributes;

namespace Antura.ReservedArea
{
    public class ReservedAreaManager : MonoBehaviour
    {

        [Header("References")]
        public CheckIcon AnalyticsCheckIcon;
        public CheckIcon NotificationsCheckIcon;
        [DeEmptyAlert]
        public CheckIcon ClassroomModeCheckIcon;
        [DeEmptyAlert]
        [SerializeField] ClassroomPanel classroomPanel;
        [DeEmptyAlert]
        [SerializeField] GameObject oldUI, newUI;

        void Awake()
        {
            oldUI.SetActive(false);
            newUI.SetActive(true);
        }

        IEnumerator Start()
        {
            AnalyticsCheckIcon.Set(AppManager.I.AppSettingsManager.Settings.ShareAnalyticsEnabled);
            NotificationsCheckIcon.Set(AppManager.I.AppSettingsManager.Settings.NotificationsEnabled);
            ClassroomModeCheckIcon.Set(AppManager.I.AppSettingsManager.Settings.ClassRoomMode > 0);
            yield return null;

            // If "no classroom" open popup to choose classroom, otherwise select current classroom
            int currClassroomIndex = AppManager.I.AppSettings.ClassRoomMode;
            if (currClassroomIndex >= 0)
                classroomPanel.OpenClass(currClassroomIndex);
            else
                classroomPanel.OpenSelectClassroomPopup(false);
        }

        #region Buttons

        public void OnOpenUrlWebsite()
        {
            Application.OpenURL(AppConfig.UrlWebsite);
        }

        public void OnOpenUrlPrivacy()
        {
            Application.OpenURL(AppConfig.UrlPrivacy);
        }

        public void OnOpenCommunityTelegram()
        {
            Application.OpenURL(AppConfig.UrlCommunityTelegram);
        }

        public void OnOpenCommunityFacebook()
        {
            Application.OpenURL(AppConfig.UrlCommunityFacebook);
        }

        public void OnOpenCommunityTwitter()
        {
            Application.OpenURL(AppConfig.UrlCommunityTwitter);
        }

        public void OnOpenCommunityInstagram()
        {
            Application.OpenURL(AppConfig.UrlCommunityInstagram);
        }

        public void OnOpenInstallInstructions()
        {
            if (AppManager.I.AppEdition.editionID == AppEditionID.LearnEnglish_Ceibal)
            {
                var pdfViewerPrefab = Resources.Load("Pdf/CeibalPDFViewer") as GameObject;
                var pdfViewer = Instantiate(pdfViewerPrefab);
                pdfViewer.transform.SetParent(GameObject.Find("[GlobalUI]").transform, false);
            }
            else
            {
                GlobalUI.ShowPrompt(LocalizationDataId.UI_Prompt_AndroidInstallPDF);
                OpenPDF(AppConfig.PdfAndroidInstall);
            }
        }

        private int clickCounter = 0;

        public void OnClickEnableDebugPanel()
        {
            clickCounter++;
            if (clickCounter >= 3)
            {
                if (!DebugManager.I.DebugPanelEnabled)
                {
                    DebugManager.I.EnableDebugPanel();
                }
            }
        }

        #endregion

        public void OnBtnShareData()
        {
            GlobalUI.ShowPrompt(LocalizationDataId.UI_PromptOnlineAnalytics,
            () =>
            {
                AppManager.I.AppSettingsManager.EnableShareAnalytics(true);
                AnalyticsCheckIcon.Set(true);
            }, () =>
            {
                AppManager.I.AppSettingsManager.EnableShareAnalytics(false);
                AnalyticsCheckIcon.Set(false);
            });
        }

        public void OnBtnNotifications()
        {
            GlobalUI.ShowPrompt(LocalizationDataId.UI_PromptNotifications,
            () =>
            {
                AppManager.I.AppSettingsManager.EnableNotifications(true);
                NotificationsCheckIcon.Set(true);
            }, () =>
            {
                AppManager.I.AppSettingsManager.EnableNotifications(false);
                NotificationsCheckIcon.Set(false);
            });
        }

        public void OnBtnClassroomMode()
        {
            // // NEW open classroom selector
            // // Stub data
            // List<string> classroomIDs = GetStubClassroomIDs();
            // GlobalPopups.OpenSelector("Choose classroom", classroomIDs, x => {
            //     classroomPanel.Open(classroomIDs[x], classroomPanel.TestGenerateStubProfiles());
            // });

            // OLD SYSTEM where classroom mode button was a toggle
            // if (AppManager.I.AppSettingsManager.Settings.ClassRoomMode > 0)
            // {
            //     AppManager.I.AppSettingsManager.SetClassroomMode(0);
            //     ClassroomModeCheckIcon.Set(false);
            // }
            // else
            // {
            //     AppManager.I.AppSettingsManager.SetClassroomMode(1);
            //     ClassroomModeCheckIcon.Set(true);
            // }
        }

        public void OnOpenDonate()
        {
            Application.OpenURL(AppConfig.UrlDonate);
        }

        #region RATE

        public void OnOpenRateApp()
        {
            GlobalUI.ShowPrompt(LocalizationDataId.UI_Prompt_rate, DoOpenRateApp, DoNothing);
        }

        void DoOpenRateApp()
        {
            Debug.Log("On DEVICE it will open the app page on the proper store");
            if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                Application.OpenURL(AppConfig.UrlStoreiOSApple);
                // IOSNativeUtility.RedirectToAppStoreRatingPage();
            }
            else if (Application.platform == RuntimePlatform.Android)
            {
                Application.OpenURL(AppConfig.UrlStoreAndroidGoogle);
                // AndroidNativeUtility.OpenAppRatingPage("");
            }
            //GlobalUI.ShowPrompt("", "Rate app");
        }

        #endregion

        #region SUPPORT FORM

        public void OnOpenSupportForm()
        {
            GlobalUI.ShowPrompt(LocalizationDataId.UI_Prompt_bugreport, DoOpenSupportForm, DoNothing);
        }

        void DoOpenSupportForm()
        {
            AppManager.I.OpenSupportForm();
        }

        #endregion

        public void OnOpenRecomment()
        {
            // GlobalUI.ShowPrompt("", "How to Recommend Antura");
        }

        void DoNothing()
        {
        }

        public void OpenPDF(string filename)
        {
            string destPath;
            var pdfTemp = Resources.Load("Pdf/" + filename, typeof(TextAsset)) as TextAsset;
            destPath = Application.persistentDataPath + "/" + filename;

            File.WriteAllBytes(destPath, pdfTemp.bytes);
            Debug.Log("Copied " + pdfTemp.name + " to " + destPath + " , File size : " + pdfTemp.bytes.Length);
            Application.OpenURL(destPath);
        }

        /// <summary>
        /// exports all databases found in
        /// </summary>
        public void OnExportDatabasesJoined()
        {
            if (AppManager.I.DB.ExportPlayersJoinedDb(out string errorString))
            {
                string dbPath = DBService.GetDatabaseFilePath(AppConfig.GetJoinedDatabaseFilename(), AppConfig.DbJoinedFolder);
                GlobalUI.ShowPrompt("The joined DB is here:\n" + dbPath);
            }
            else
            {
                GlobalUI.ShowPrompt("Could not export the joined database.");
            }
        }

        /// <summary>
        /// Imports a set of database
        /// </summary>
        public void OnImportDatabases()
        {
            AppManager.I.PlayerProfileManager.ImportAllPlayerProfiles();
            AppManager.I.NavigationManager.ReloadScene();
        }
    }
}
