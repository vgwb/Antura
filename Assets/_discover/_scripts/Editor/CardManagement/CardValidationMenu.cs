using System.Text;
using UnityEditor;

namespace Antura.Discover.Editor
{
    public static class CardValidationMenu
    {
        public static void ValidateCards()
        {
            var rpt = CardValidationUtility.ValidateAllCards(logEachIssue: true);
            var sb = new StringBuilder();
            sb.AppendLine(rpt.ToString());
            EditorUtility.DisplayDialog("Card Validation", sb.ToString(), "OK");
        }

        public static void SyncCardQuestLinks()
        {
            var logs = new System.Collections.Generic.List<string>();
            int changes = CardValidationUtility.SyncCardQuestLinks(applyChanges: true, logs: logs, verbose: true);
            var msg = $"Applied changes: {changes}\n\n" + string.Join("\n", logs);
            EditorUtility.DisplayDialog("Sync Card-Quest Links", msg, "OK");
        }

        public static void CleanNonReciprocal()
        {
            var logs = new System.Collections.Generic.List<string>();
            int changes = CardValidationUtility.UnlinkNonReciprocalCardQuestLinks(applyChanges: true, logs: logs, verbose: true);
            var msg = $"Applied changes: {changes}\n\n" + string.Join("\n", logs);
            EditorUtility.DisplayDialog("Clean Card-Quest Links", msg, "OK");
        }
    }
}
