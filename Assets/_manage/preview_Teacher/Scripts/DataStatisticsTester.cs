using System;
using System.Collections;
using System.Collections.Generic;
using DG.DeInspektor.Attributes;
using UnityEngine;
using Antura.Audio;
using Antura.Database;
using Antura.Core;
using Antura.Extensions;
using Antura.Helpers;
using Antura.UI;

namespace Antura.Teacher.Test
{
    /// <summary>
    /// Helper class to test DataBase contents, useful to pinpoint critical data.
    /// </summary>
    public class DataStatisticsTester : MonoBehaviour
    {
        private VocabularyHelper _vocabularyHelper;
        private DatabaseManager _databaseManager;
        private List<PlaySessionData> _playSessionDatas;
        private List<LetterData> _letterDatas;
        private List<WordData> _wordDatas;
        private List<PhraseData> _phraseDatas;

        //private LetterFilters _letterFilters;
        private WordFilters _wordFilters;

        IEnumerator Start()
        {
            _databaseManager = AppManager.I.DB;
            _vocabularyHelper = AppManager.I.VocabularyHelper;

            _playSessionDatas = _databaseManager.GetAllPlaySessionData();
            _letterDatas = _databaseManager.GetAllLetterData();
            _letterDatas.RemoveAll(x => x.Kind == LetterDataKind.SpecialChar);
            _wordDatas = _databaseManager.GetAllWordData();
            _phraseDatas = _databaseManager.GetAllPhraseData();

            //_letterFilters = new LetterFilters();
            _wordFilters = new WordFilters();
            yield break;
        }


        [DeMethodButton("Letters Frequency")]
        public void DoLettersFrequency()
        {
            int threshold = 3;

            DoStatsList("Frequency of letters in words", _letterDatas,
                data => _vocabularyHelper.GetWordsWithLetter(_wordFilters, data, LetterEqualityStrictness.Letter).Count < threshold,
                data => _vocabularyHelper.GetWordsWithLetter(_wordFilters, data, LetterEqualityStrictness.Letter).Count.ToString());
        }


        /*
        [DeMethodButton("Letter Audio PHONEME")]
        public void DoLetterAudioPhoneme()
        {
            DoStatsList("Letters with audio PHONEME", _letterDatas,
                        data => AudioManager.I.GetAudioClip(data, LetterDataSoundType.Phoneme) == null,
                        data => AudioManager.I.GetAudioClip(data, LetterDataSoundType.Phoneme) == null ? "NO" : "ok");
        }

        [DeMethodButton("Letter Audio NAME")]
        public void DoLetterAudioName()
        {
            DoStatsList("Letters with audio LETTERNAME", _letterDatas,
                                    data => AudioManager.I.GetAudioClip(data, LetterDataSoundType.Name) == null,
                                    data => AudioManager.I.GetAudioClip(data, LetterDataSoundType.Name) == null ? "NO" : "ok");
        }

        [DeMethodButton("Word Audio")]
        public void DoWordAudio()
        {
            DoStatsList("Words with audio", _wordDatas,
                data => AudioManager.I.GetAudioClip(data) == null,
                data => AudioManager.I.GetAudioClip(data) == null ? "NO" : "ok");
        }

        [DeMethodButton("Phrase Audio")]
        public void DoPhraseAudio()
        {
            DoStatsList("Phrases with audio", _phraseDatas,
                data => AudioManager.I.GetAudioClip(data) == null,
                data => AudioManager.I.GetAudioClip(data) == null ? "NO" : "ok");
        }
        */

        // Find all letters that have no words in the same PS, or words that have no letters in the same PS
        [DeMethodButton("Data matching in PS")]
        public void DoCheckLettersAndWordsWithoutMatchingsInPS()
        {
            string final_s = "Word & Letters matching in PS";

            List<WordData> observedWords = new List<WordData>();
            List<LetterData> observedLetters = new List<LetterData>();

            final_s += "\n\n Words without matching letters in their PS:";
            foreach (var playSessionData in _playSessionDatas) {
                // Get the letters & words in this PS
                var contents = AppManager.I.Teacher.VocabularyAi.GetContentsUpToJourneyPosition(playSessionData.GetJourneyPosition());
                var letters = contents.GetHashSet<LetterData>();
                //var letterIds = letters.ToList().ConvertAll(x => x.Id);
                var words = contents.GetHashSet<WordData>();
                //var wordIds = words.ToList().ConvertAll(x => x.Id);

                // Check whether there are words with letters that are not in the PS
                bool somethingWrong = false;
                string ps_s = "\n\nPS " + playSessionData.GetJourneyPosition();
                foreach (var word in words) {
                    if (observedWords.Contains(word)) continue;

                    if (!_vocabularyHelper.WordContainsAnyLetter(word, letters)) {
                        observedWords.Add(word);
                        ps_s += "\n" + word.Id + " has no matching letters!";
                        somethingWrong = true;
                    }
                }
                foreach (var letter in letters) {
                    if (observedLetters.Contains(letter)) continue;

                    if (!_vocabularyHelper.AnyWordContainsLetter(letter, words)) {
                        observedLetters.Add(letter);
                        ps_s += "\n" + letter.Id + " has no matching words!";
                        somethingWrong = true;
                    }
                }
                if (somethingWrong)
                    final_s += ps_s;
            }

            Debug.Log(final_s);
        }

        // Find all words that have letters that do not appear in the journey
        [DeMethodButton("Words with undiscovered letters in PS")]
        public void DoCheckWordsWithUndiscoveredLettersInPS()
        {
            string final_s = "Words with undiscovered letters in PS:";

            List<WordData> observedWords = new List<WordData>();

            foreach (var playSessionData in _playSessionDatas) {
                // Get all letters & words in this PS
                var contents = AppManager.I.Teacher.VocabularyAi.GetContentsUpToJourneyPosition(playSessionData.GetJourneyPosition());
                var journeyLetters = contents.GetHashSet<LetterData>();
                var journeyWords = contents.GetHashSet<WordData>();

                // Check whether there are words with letters that are not in the PS
                bool somethingWrong = false;
                string ps_s = "\n\nPS " + playSessionData.GetJourneyPosition();
                foreach (var word in journeyWords) {
                    if (observedWords.Contains(word)) continue;

                    var lettersInWord = _vocabularyHelper.GetLettersInWord(word);
                    foreach (var letterInWord in lettersInWord) {
                        if (!journeyLetters.Contains(letterInWord)) {
                            ps_s += "\n" + word.Id + " has undiscovered letter " + letterInWord.Id + "!";
                            somethingWrong = true;
                        }
                    }
                    observedWords.Add(word);
                }
                if (somethingWrong)
                    final_s += ps_s;
            }

            Debug.Log(final_s);
        }

        // Check number of letters, words, and phrases in each PS
        [DeMethodButton("Data frequency in PS")]
        public void DoCheckDataFrequencyByPS()
        {
            string final_s = "Data frequency in PS";

            Dictionary<LetterData, int> observedLetters = new Dictionary<LetterData, int>();
            Dictionary<WordData, int> observedWords = new Dictionary<WordData, int>();
            Dictionary<PhraseData, int> observedPhrases = new Dictionary<PhraseData, int>();

            foreach (var d in AppManager.I.DB.GetAllLetterData()) observedLetters[d] = 0;
            foreach (var d in AppManager.I.DB.GetAllWordData()) observedWords[d] = 0;
            foreach (var d in AppManager.I.DB.GetAllPhraseData()) observedPhrases[d] = 0;

            foreach (var playSessionData in _playSessionDatas) {
                // Get the data in this PS
                var contents = AppManager.I.Teacher.VocabularyAi.GetContentsUpToJourneyPosition(playSessionData.GetJourneyPosition());
                var letters = contents.GetHashSet<LetterData>();
                var words = contents.GetHashSet<WordData>();
                var phrases = contents.GetHashSet<PhraseData>();

                // Count the data entries
                foreach (var d in words)
                    observedWords[d]++;
                foreach (var d in letters)
                    observedLetters[d]++;
                foreach (var d in phrases)
                    observedPhrases[d]++;
            }

            final_s += "\n\n Letters:";
            foreach (var d in AppManager.I.DB.GetAllLetterData()) if (observedLetters[d] == 0) final_s += "\n" + d.Id + ": " + observedLetters[d];

            final_s += "\n\n Words:";
            foreach (var d in AppManager.I.DB.GetAllWordData()) if (observedWords[d] == 0) final_s += "\n" + d.Id + ": " + observedWords[d];

            final_s += "\n\n Phrases:";
            foreach (var d in AppManager.I.DB.GetAllPhraseData()) if (observedPhrases[d] == 0) final_s += "\n" + d.Id + ": " + observedPhrases[d];

            Debug.Log(final_s);
        }

        // Print letters and words with these letters
        [DeMethodButton("Print Letters and words they appear in")]
        public void DoPrintLettersAndWords()
        {
            DoStatsList("Letters & words", _letterDatas,
                data => false,
                data => {
                    string s = "";
                    var words = _vocabularyHelper.GetWordsWithLetter(_wordFilters, data, LetterEqualityStrictness.Letter);
                    foreach (var word in words) {
                        s += $"{word.Id} ({word.Text}), ";
                    }
                    return s;
                });
        }



        #region Internals

        void DoStatsList<T>(string title, List<T> dataList, Predicate<T> problematicCheck, Func<T, string> valueFunc)
        {
            var problematicEntries = new List<string>();

            string data_s = "";
            foreach (var data in dataList) {
                bool isProblematic = problematicCheck(data);

                string entryS = string.Format("{0}: \t{1}", data, valueFunc(data));
                if (isProblematic) {
                    data_s += "\n" + entryS;
                    problematicEntries.Add(data.ToString());
                } else {
                    data_s += "\n" + entryS;
                }
            }

            string final_s = "---- " + title;
            if (problematicEntries.Count == 0) {
                final_s += "\nAll is fine!\n";
            } else {
                final_s += "\nProblematic: (" + problematicEntries.Count + ") \n";
                foreach (var entry in problematicEntries)
                    final_s += "\n" + entry;
            }

            final_s += data_s;
            PrintReport(final_s);
        }

        void PrintReport(string s)
        {
            Debug.Log(s);
        }

        #endregion

    }

}
