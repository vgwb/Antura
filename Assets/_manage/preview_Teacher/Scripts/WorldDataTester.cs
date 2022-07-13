using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Antura.Core;
using Antura.Database;
using Antura.Helpers;
using Antura.Language;
using DG.DeExtensions;
using DG.DeInspektor.Attributes;
using UnityEngine;

namespace Antura.Teacher.Test
{
    /// <summary>
    /// Helper class to test DataBase contents related to the vocabulary data assigned to the world data
    /// </summary>
    public class WorldDataTester : MonoBehaviour
    {
        public string[] LetterGroups;

        private DatabaseManager _databaseManager;
        private VocabularyHelper _vocabularyHelper;
        private List<PlaySessionData> _playSessionDatas;
        private List<LetterData> _letterDatas;
        private List<WordData> _wordDatas;


        IEnumerator Start()
        {
            var edition = FindObjectOfType<Database.Management.EditorContentHolder>().InputContent;
            AppManager.I.AppSettingsManager.SetLearningContentID(edition.ContentID);
            yield return AppManager.I.ReloadEdition();

            _databaseManager = new DatabaseManager(edition, edition.LearningLanguage);
            _vocabularyHelper = new VocabularyHelper(_databaseManager);

            _playSessionDatas = _databaseManager.GetAllPlaySessionData();
            _letterDatas = _databaseManager.GetAllLetterData();
            _wordDatas = _databaseManager.GetAllWordData();
            //_phraseDatas = _databaseManager.GetAllPhraseData();
        }

        // Find all words that only have letters that appear in the Groups
        // Each Group can be used by the words in the subsequent Group
        [DeMethodButton("Words with only letters in Groups")]
        public void DoCheckWordsWithOnlyLetters()
        {
            string s;
            var query = "";
            var report = "";
            report += ($"Report - Analyzing {LetterGroups.Length} groups");
            List<LetterData> previousLetters = new List<LetterData>();
            List<WordData> previousWords = new List<WordData>();
            var wordToGroup = new Dictionary<string, string>();
            for (var iGroup = 0; iGroup < LetterGroups.Length; iGroup++)
            {
                var letterGroup = LetterGroups[iGroup];

                query += $",{letterGroup}";
                var desiredLetters = new List<LetterData>();
                foreach (string l in query.Split(","))
                {
                    if (l.IsNullOrEmpty()) continue;
                    var data = _databaseManager.GetLetterDataById(l.Trim());
                    desiredLetters.Add(data);
                }
                desiredLetters.AddRange(_databaseManager.GetAllLetterData().Where(x => x.Kind == LetterDataKind.SpecialChar));

                /*
                var str = $"Group {(iGroup+1)} letters:\n";
                foreach (var l in desiredLetters)
                {
                    if (previousLetters.Contains(l)) continue;
                    str += $"{l}\n";
                }
                report += "\n" + str;*/

                var correctWords = new List<WordData>();
                var wrongWords = new List<WordData>();
                var letterCount = new Dictionary<LetterData, int>();
                foreach (var wordData in _wordDatas)
                {
                    var lettersInWord = _vocabularyHelper.GetLettersInWord(wordData);
                    lettersInWord = lettersInWord.ConvertAll(ld => _vocabularyHelper.ConvertToLetterWithForcedForm(ld, LetterForm.Isolated));
                    bool isCorrect = true;
                    foreach (var letterInWord in lettersInWord)
                    {
                        if (desiredLetters.All(x => !x.IsSameLetterAs(letterInWord, LetterEqualityStrictness.Letter)))
                        {
                            //Debug.LogError($"Word {wordData.Id} has letter {letterInWord} we do not want.");
                            isCorrect = false;
                        }
                    }

                    if (isCorrect) correctWords.Add(wordData);
                    else wrongWords.Add(wordData);

                    if (isCorrect)
                    {
                        // Check letter count
                        foreach (var desiredLetter in desiredLetters)
                        {
                            if (lettersInWord.Any(x => x.IsSameLetterAs(desiredLetter, LetterEqualityStrictness.Letter)))
                            {
                                //Debug.LogError($"Word {wordData.Id} has letter {desiredLetter} we want.");
                                if (!letterCount.ContainsKey(desiredLetter)) letterCount[desiredLetter] = 0;
                                letterCount[desiredLetter]++;
                            }
                        }
                    }
                }

                s = $"Group {(iGroup+1)} Letters:\n";
                foreach (var desiredLetter in desiredLetters)
                {
                    if (previousLetters.Contains(desiredLetter)) continue;
                    s += $"{desiredLetter.Id} ({desiredLetter.Isolated}) ({(letterCount.ContainsKey(desiredLetter) ? letterCount[desiredLetter].ToString() : "0")})\n";

                    if (!letterCount.ContainsKey(desiredLetter))
                    {
                        Debug.LogError($"Letter <b>{desiredLetter}</b> found ZERO times");
                    }
                    else if (letterCount[desiredLetter] < 3)
                    {
                        Debug.LogError($"Letter <b>{desiredLetter}</b> found only {letterCount[desiredLetter]} times");
                    }
                }
                report += "\n" + s;

                int totCorrectWords = correctWords.Count;
                correctWords = correctWords.Where(x => !previousWords.Contains(x)).ToList();
                s = $"Words for group {iGroup+1}: <b>{correctWords.Count}</b> (tot {totCorrectWords}/{_wordDatas.Count}):\n";
                int nWithDrawings = correctWords.Count(x => x.HasDrawing());
                s += $"(With drawings: {nWithDrawings}/{correctWords.Count}):\n";
                foreach (var correctWord in correctWords)
                {
                    wordToGroup[correctWord.Id] = $"WordGroup{iGroup+1}";
                    s += $"{correctWord.Id} ({correctWord.Text})";
                    if (correctWord.HasDrawing()) s += " D";
                    s += "\n";
                }
                report += "\n" + s;

                previousLetters.AddRange(desiredLetters);
                previousWords.AddRange(correctWords);
            }

            s = "\nUNUSED WORDS:\n";
            foreach (var word in _vocabularyHelper.GetAllWords(new WordFilters()))
            {
                if (previousWords.Contains(word)) continue;
                s += $"{word.Id} ({word.Text} {_vocabularyHelper.GetLettersInWord(word).Select(x => x.Isolated).ToJoinedString()})\n";
            }
            report += "\n" + s;

            s = "\nUNUSED LETTERS:\n";
            foreach (var letter in _vocabularyHelper.GetAllLetters(new LetterFilters()))
            {
                if (previousLetters.Contains(letter)) continue;
                s += $"{letter.Id} ({letter.Isolated})\n";
            }
            report += "\n" + s;
            Debug.Log(report);

            // Report word data with their play session link
            s = "PLAY SESSION LINKS:\n";
            foreach (var word in _vocabularyHelper.GetAllWords(new WordFilters()))
            {
                if (wordToGroup.TryGetValue(word.Id, out var group))
                {
                    s += $"{word.Id} ({word.Text}), {group}\n";
                }
                else
                {
                    s += $"{word.Id} ({word.Text}), no group\n";
                }
            }
            Debug.Log(s);
        }

    }
}
