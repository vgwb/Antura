using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Antura.Discover
{
    public static class QuestSubjectsUtility
    {
        public static List<SubjectCount> ComputeSubjectsBreakdown(QuestData quest)
        {
            var totals = new Dictionary<Subject, int>();
            if (quest == null)
                return new List<SubjectCount>();

            var topicList = quest.Topics ?? new List<TopicData>();
            foreach (var t in topicList)
            {
                if (t == null)
                    continue;
                // Core card subjects (2x)
                if (t.CoreCard != null)
                    AddCardSubjects(totals, t.CoreCard, 2);
                // Connected cards subjects (1x)
                if (t.Connections != null)
                {
                    foreach (var cn in t.Connections)
                    {
                        var cc = cn != null ? cn.ConnectedCard : null;
                        if (cc != null)
                            AddCardSubjects(totals, cc, 1);
                    }
                }
            }

            return totals.Select(kv => new SubjectCount(kv.Key, kv.Value))
                         .OrderByDescending(sc => sc.Count)
                         .ThenBy(sc => sc.Subject.ToString(), StringComparer.Ordinal)
                         .ToList();
        }

        public static string BuildSummaryText(IEnumerable<SubjectCount> counts, string separator = ", ")
        {
            if (counts == null)
                return string.Empty;
            var list = counts.ToList();
            if (list.Count == 0)
                return string.Empty;
            return string.Join(separator, list.Select(sc => sc.ToString()));
        }

        public static string BuildSummaryTextSimple(IEnumerable<SubjectCount> counts, string separator = ", ")
        {
            if (counts == null)
                return string.Empty;
            var list = counts.ToList();
            if (list.Count == 0)
                return string.Empty;
            return string.Join(separator, list.Take(3).Select(sc => sc.Subject.ToString()));
        }

        private static void AddCardSubjects(Dictionary<Subject, int> totals, CardData card, int weight)
        {
            if (card == null)
                return;
            var subs = card.Subjects;
            if (subs == null || subs.Count == 0)
                return;
            foreach (var s in subs)
            {
                if (!totals.ContainsKey(s))
                    totals[s] = 0;
                totals[s] += weight;
            }
        }
    }
}
