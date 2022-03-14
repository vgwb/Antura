#if UNITY_EDITOR
using System.Collections;
using Antura.Core;
using Antura.Database;
using Antura.Database.Management;
using Antura.Language;
using Antura.Profile;
using Antura.Teacher;
using Antura.UI;
using UnityEngine;

public class CustomSceneManager : MonoBehaviour
{
    public LanguageCode langCode;

    [HideInInspector]
    public DatabaseLoader dbLoader;
    public DatabaseManager dbManager;
    public TeacherAI teacherAI;
    public PlayerProfile playerProfile;
    public VocabularyHelper vocabularyHelper;
    public ScoreHelper scoreHelper;

    void Start()
    {
        GlobalUI.I.gameObject.SetActive(false);
        /*
                dbManager = new DatabaseManager(true, langCode);
                vocabularyHelper = new VocabularyHelper(dbManager);
                scoreHelper = new ScoreHelper(dbManager);
                teacherAI = new TeacherAI(dbManager, vocabularyHelper, scoreHelper);
        */
        //yield return AppManager.I.ReloadEdition();
        //Debug.LogError("Loaded custom scene");
    }


}
#endif
