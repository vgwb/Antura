using Antura.Core;
using UnityEngine;
using UnityEditor;

namespace Antura.Editor
{
    public class InfoView : EditorWindow
    {

        [MenuItem("Antura/Info/GitHub repository", false, 300)]
        static void OpenGitHub()
        {
            Application.OpenURL(AppConfig.UrlGithubRepository);
        }

        [MenuItem("Antura/Info/Translations Sheet", false, 300)]
        static void OpenLocalizationGoogleSheet()
        {
            Application.OpenURL(AppConfig.UrlTranslationsSheet);
        }

        [MenuItem("Antura/Info/GitHub Project", false, 300)]
        static void OpenGitHubProject()
        {
            Application.OpenURL(AppConfig.UrlGithubProject);
        }

        [MenuItem("Antura/Info/antura.org", false, 300)]
        static void OpenAnturaWebsite()
        {
            Application.OpenURL(AppConfig.UrlWebsite);
        }
    }
}
