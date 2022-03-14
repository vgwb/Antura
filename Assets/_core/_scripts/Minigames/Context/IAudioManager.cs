using Antura.Database;
using Antura.Keeper;
using Antura.LivingLetters;
using UnityEngine;

namespace Antura.Minigames
{
    /// <summary>
    /// Provides generic audio access to the core and to minigames.
    /// <seealso cref="MinigamesAudioManager"/>
    /// </summary>
    public interface IAudioManager
    {
        bool MusicEnabled { get; set; }

        IAudioSource PlaySound(Sfx sfx);
        IAudioSource PlaySound(AudioClip clip);
        void StopAllSfx();

        IAudioSource PlayMusic(AudioClip clip);
        //TODO: IAudioSource PlayMusic(Music music);

        void Reset();

        // TODO: To be removed in next version
        void PlayMusic(Music music);

        void StopMusic();

        /// <summary>
        /// Play sound for letter or word,
        /// if exclusive is true, it will stop any previous letter sound.
        /// </summary>
        IAudioSource PlayVocabularyData(ILivingLetterData data, bool exclusive = true,
            LetterDataSoundType soundType = LetterDataSoundType.Phoneme,
            System.Action callback = null, KeeperMode keeperMode = KeeperMode.LearningNoSubtitles,
            bool autoClose = true, bool isKeeper = false);

        void PlayDialogue(LocalizationDataId text, System.Action onCompleted = null, KeeperMode keeperMode = KeeperMode.Default, bool isKeeper = true);

        void Update();
    }
}
