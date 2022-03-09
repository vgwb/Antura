using Antura.Audio;
using Antura.Database;
using Antura.Keeper;
using Antura.LivingLetters;
using UnityEngine;

namespace Antura.Minigames
{
    public class MinigamesAudioManager : IAudioManager
    {
        public bool MusicEnabled
        {
            get { return AudioManager.I.MusicEnabled; }
            set { AudioManager.I.MusicEnabled = value; }
        }

        public IAudioSource PlayVocabularyData(ILivingLetterData data, bool exclusive = true, LetterDataSoundType soundType = LetterDataSoundType.Phoneme,
            System.Action callback = null, KeeperMode keeperMode = KeeperMode.LearningNoSubtitles,
            bool autoClose = true, bool isKeeper = false)
        {
            return KeeperManager.I.PlayVocabularyData(data, exclusive, isKeeper: isKeeper, _callback: callback, autoClose: autoClose, keeperMode: keeperMode, soundType: soundType);
        }

        public void PlayDialogue(LocalizationDataId text, System.Action callback = null, KeeperMode keeperMode = KeeperMode.Default, bool isKeeper = true)
        {
            KeeperManager.I.PlayDialogue(text.ToString(), _callback: callback, keeperMode: keeperMode, isKeeper: isKeeper);
        }

        public void PlayMusic(Music music)
        {
            AudioManager.I.PlayMusic(music);
        }

        public void StopMusic()
        {
            AudioManager.I.StopMusic();
        }

        public IAudioSource PlaySound(Sfx sfx)
        {
            return AudioManager.I.PlaySound(sfx);
        }

        public void StopAllSfx()
        {
            AudioManager.I.StopSfxGroup();
        }

        public AudioClip GetAudioClip(Sfx sfx)
        {
            return AudioManager.I.GetSfxAudioClip(sfx);
        }

        public void Reset()
        {
            StopMusic();
            AudioManager.I.StopVocabularyGroup();
            AudioManager.I.ClearCache();
            AudioManager.I.StopDialogue(true);
        }

        public IAudioSource PlayMusic(AudioClip clip)
        {
            return AudioManager.I.PlayMusic(clip);
        }

        public IAudioSource PlaySound(AudioClip clip)
        {
            return AudioManager.I.PlaySound(clip);
        }

        public void Update()
        {
        }
    }
}
