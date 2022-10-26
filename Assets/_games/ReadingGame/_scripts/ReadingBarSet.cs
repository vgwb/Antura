using UnityEngine;
using System.Collections.Generic;
using Antura.Database;
using Antura.Helpers;
using Antura.Keeper;
using Antura.LivingLetters;
using Antura.Minigames;
using Antura.Language;

namespace Antura.Minigames.ReadingGame
{
    public class ReadingBarSet : MonoBehaviour
    {
        public bool active = true;

        public ReadingBar readingBarPrefab;

        List<ReadingBar> bars = new List<ReadingBar>();

        public Camera mainCamera;

        public Transform barsStart;
        public float distanceBetweenBars = 3;
        int completedBars = 0;

        float maxBarSize = 10;
        const float MAX_BAR_SIZE = 10f;
        const float MAX_BAR_SIZE_SONG = 14f;

        ReadingBar activeBar;
        Vector3 barsStartInitialPosition;

        ReadingBarWord[] currentBarWords;
        KaraokeSong currentBarSong;

        bool playingSong = false;
        bool songStarted = false;
        bool barsCompleted = false;
        bool songInsideAWord = false;

        float lastSongTime = 0;
        IAudioSource songSource;
        System.Action onSongCompleted;

        bool showTargets = true;
        bool showArrows = true;

        void Awake()
        {
            barsStartInitialPosition = barsStart.transform.localPosition;
        }

        public ReadingBar GetActiveBar()
        {
            return activeBar;
        }

        public ReadingBar GetNextBar()
        {
            int nextId = bars.FindIndex((b) => { return b == activeBar; }) + 1;

            if (nextId >= bars.Count)
            {
                return null;
            }
            else
            {
                return bars[nextId];
            }
        }

        void SetActiveBar(ReadingBar bar)
        {
            if (activeBar != null)
            {
                activeBar.Active = false;
                activeBar.Complete();
            }

            activeBar = bar;
            if (activeBar != null)
                activeBar.Active = true;
        }

        void OnDestroy()
        {
            if (songSource != null)
                songSource.Stop();
        }

        public void Clear()
        {
            currentBarWords = null;
            SetActiveBar(null);

            // Clear past data
            foreach (var b in bars)
            {
                Destroy(b.gameObject);
            }
            bars.Clear();

            completedBars = 0;
            barsStart.transform.localPosition = barsStartInitialPosition;
        }

        public void SetData(ILivingLetterData data)
        {
            maxBarSize = MAX_BAR_SIZE;
            string text = data.TextForLivingLetter;

            var splitText = text.Split(' ');

            for (int i = 0; i < splitText.Length - 1; ++i)
                splitText[i] = splitText[i] + " ";

            currentBarWords = SetData(splitText, null, true);
            currentBarSong = null;

            //SetActiveBar(bars[0]);
        }

        public void SetData(KaraokeSong data)
        {
            maxBarSize = MAX_BAR_SIZE_SONG;
            var karaokeLines = data.lines;
            string[] words = new string[karaokeLines.Count];
            bool[] lineBreaks = new bool[karaokeLines.Count];

            for (int i = 0; i < karaokeLines.Count; ++i)
            {
                words[i] = karaokeLines[i].text;
                lineBreaks[i] = karaokeLines[i].starsWithLineBreak;
            }

            currentBarWords = SetData(words, lineBreaks, false);
            currentBarSong = data;

            for (int i = 0; i < bars.Count; ++i)
                bars[i].currentTarget = 0;
        }

        public void PlaySong(IAudioSource source, System.Action onSongCompleted)
        {
            if (playingSong)
                return;

            playingSong = true;
            songSource = source;
            this.onSongCompleted = onSongCompleted;

            for (int i = 0; i < bars.Count; ++i)
            {
                bars[i].shineWhenNearTarget = true;
            }
        }

        public void SetShowTargets(bool show)
        {
            showTargets = show;
            for (int i = 0; i < bars.Count; ++i)
            {
                bars[i].showTarget = show;
            }
        }

        public void SetShowArrows(bool show)
        {
            showArrows = show;
            for (int i = 0; i < bars.Count; ++i)
            {
                bars[i].showArrows = show;
            }
        }

        ReadingBarWord[] SetData(string[] words, bool[] forceLineBreaks, bool addOffsets)
        {
            Clear();

            int wordsCount = words.Length;
            ReadingBarWord[] barWords = new ReadingBarWord[wordsCount];
            List<int> barStarts = new List<int>();

            barStarts.Add(0);

            barWords[0].start = 0;
            barWords[0].word = words[0];
            barWords[0].barId = 0;

            var currentReadingBar = GameObject.Instantiate(readingBarPrefab);
            currentReadingBar.transform.SetParent(barsStart);
            currentReadingBar.transform.localPosition = Vector3.zero;
            currentReadingBar.showArrows = showArrows;
            currentReadingBar.showTarget = showTargets;
            currentReadingBar.text.SetText(words[0], LanguageUse.Learning);
            currentReadingBar.Id = 0;
            if (!addOffsets)
            {
                currentReadingBar.startOffset = 0;
                currentReadingBar.endOffset = 0;
            }

            bars.Add(currentReadingBar);
            float lastBarSize = barWords[0].end = currentReadingBar.text.TMPText.GetPreferredValues().x;

            for (int i = 1; i < wordsCount; ++i)
            {
                var word = words[i];
                barWords[i].word = word;

                var previousText = currentReadingBar.text.text;
                currentReadingBar.text.SetText(previousText + word, LanguageUse.Learning);
                float currentBarSize = currentReadingBar.text.TMPText.GetPreferredValues().x;
                barWords[i].start = lastBarSize;
                barWords[i].end = currentBarSize;
                barWords[i].barId = barWords[i - 1].barId;

                // Evaluate split
                if (currentBarSize >= maxBarSize || (forceLineBreaks != null && forceLineBreaks[i]))
                {
                    currentReadingBar.text.SetText(previousText, LanguageUse.Learning);

                    currentReadingBar = GameObject.Instantiate(readingBarPrefab);
                    currentReadingBar.showArrows = showArrows;
                    currentReadingBar.showTarget = showTargets;
                    currentReadingBar.transform.SetParent(barsStart);
                    currentReadingBar.transform.localPosition = Vector3.down * (bars.Count % 2) * distanceBetweenBars;
                    currentReadingBar.text.SetText(word, LanguageUse.Learning);
                    currentReadingBar.Id = barWords[i].barId + 1;
                    if (!addOffsets)
                    {
                        currentReadingBar.startOffset = 0;
                        currentReadingBar.endOffset = 0;
                    }

                    barWords[i].start = 0;
                    barWords[i].barId = currentReadingBar.Id;
                    currentBarSize = barWords[i].end = currentReadingBar.text.TMPText.GetPreferredValues().x;
                    bars.Add(currentReadingBar);
                    barStarts.Add(i);
                }

                lastBarSize = currentBarSize;
            }
            barStarts.Add(wordsCount);

            // Normalize
            for (int i = 0; i < barStarts.Count - 1; ++i)
            {
                int start = barStarts[i];
                int end = barStarts[i + 1] - 1;

                float barSize = barWords[end].end;

                for (int b = start; b <= end; ++b)
                {
                    barWords[b].start /= barSize;
                    barWords[b].end /= barSize;
                }
            }

            return barWords;
        }

        public bool SwitchToNextBar()
        {
            if (activeBar != null)
            {
                activeBar.Complete();
                ++completedBars;
            }

            int nextId = bars.FindIndex((b) => { return b == activeBar; }) + 1;

            if (nextId >= bars.Count)
            {
                // Completed!
                SetActiveBar(null);
                return true;
            }
            else
            {
                // Switch to next
                SetActiveBar(bars[nextId]);

            }

            return false;
        }

        public ReadingBar PickGlass(Camera main, Vector2 lastPointerPosition)
        {
            if (!active || activeBar == null)
                return null;

            var barCollider = activeBar.glass.GetComponentInChildren<Collider>();

            RaycastHit hitInfo;
            if (barCollider.Raycast(mainCamera.ScreenPointToRay(lastPointerPosition), out hitInfo, 1000))
            {
                return activeBar;
            }
            return null;
        }

        public bool GetFollowingDistance(out float distance)
        {
            distance = 0;

            if (activeBar == null || !songInsideAWord)
                return false;

            distance = activeBar.GetDistanceFromTarget();
            return true;
        }

        void Update()
        {
            for (int i = 0; i < bars.Count; ++i)
            {
                var bar = bars[i];

                int completedPairId = (completedBars / 2) * 2;

                bool show = (i >= completedPairId) && (i < completedPairId + 2);

                bar.Show(show);
            }

            bool isMenuOpen = UI.PauseMenu.I.IsMenuOpen;
            songInsideAWord = false;
            if (playingSong)
            {
                if (songSource != null && songSource.IsPlaying)
                {
                    float currentTime = songSource.Position;
                    lastSongTime = currentTime;

                    if (isMenuOpen)
                    {
                        songSource.Pause();
                        return;
                    }

                    var songWords = currentBarSong.lines;

                    var songStart = songWords[0].start;
                    if (currentTime > songStart - 2)
                    {
                        if (!songStarted && activeBar == null && bars.Count > 0)
                            SetActiveBar(bars[0]);

                        songStarted = true;
                    }

                    if (songStarted)
                    {
                        for (int i = 0; i < songWords.Count; ++i)
                        {
                            if (i >= currentBarWords.Length)
                            {
                                Debug.LogWarning("Cannot find current bar words.");
                            }
                            var currentSongWord = songWords[i];
                            var currentBarWord = currentBarWords[i];

                            var timeStart = currentSongWord.start;
                            var timeEnd = currentSongWord.end;

                            // Move to currentBarWord
                            while (activeBar != null && activeBar.Id < currentBarWord.barId)
                                SwitchToNextBar();

                            if (currentTime < timeStart)
                            {
                                if (activeBar != null && activeBar.Id == currentBarWord.barId)
                                    activeBar.currentTarget = 0;
                                break;
                            }
                            else if (currentTime > timeEnd)
                            {
                                barsCompleted = (i == songWords.Count - 1);

                                if (activeBar != null && activeBar.Id == currentBarWord.barId)
                                {
                                    if (i == 3 || i == 7) // only at specific rows   // TODO: find a better way to stop the music halfway
                                    {
                                        ((ReadingGameGame)(ReadingGameGame.I)).hiddenText.gameObject.SetActive(false);
                                        bars[0].transform.parent.gameObject.SetActive(false);
                                        //KeeperManager.I.PlayDialogue(LocalizationDataId.Song_alphabet_SingWithMe, false, keeperMode:KeeperMode.SubtitlesOnly);
                                    }
                                }
                            }
                            else
                            {
                                // Show bar
                                if (!bars[0].transform.parent.gameObject.activeInHierarchy)
                                {
                                    ((ReadingGameGame)(ReadingGameGame.I)).hiddenText.gameObject.SetActive(true);
                                    bars[0].transform.parent.gameObject.SetActive(true);
                                    KeeperManager.I.CloseSubtitles();
                                }

                                float tInWord = (currentTime - timeStart) / (timeEnd - timeStart);

                                float t = Mathf.Lerp(currentBarWord.start, currentBarWord.end, tInWord);
                                if (activeBar != null)
                                    activeBar.currentTarget = t;
                                songInsideAWord = true;

                                break;
                            }
                        }
                    }

                    if (barsCompleted)
                    {
                        SetActiveBar(null);
                    }
                }
                else if (barsCompleted)
                {
                    songStarted = false;
                    playingSong = false;
                    songSource = null;

                    if (onSongCompleted != null)
                        onSongCompleted();
                }
                else
                {
                    if (songSource != null && !songSource.IsPlaying && !isMenuOpen)
                    {
                        songSource.Stop();
                        songSource.Play();
                        songSource.Position = lastSongTime;
                    }
                }
            }
        }
    }
}
