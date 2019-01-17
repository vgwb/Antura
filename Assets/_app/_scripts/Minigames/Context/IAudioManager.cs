using Antura.Database;
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

        /// <summary>
        /// Play sound for letter or word,
        /// if stopAllLetters is true, it will stop any previous letter sound.
        /// </summary>
        IAudioSource PlayVocabularyData(ILivingLetterData id, bool stopAllLetters = true, LetterDataSoundType soundType = LetterDataSoundType.Phoneme);

        IAudioSource PlayMusic(AudioClip clip);
        //TODO: IAudioSource PlayMusic(Music music);

        void Reset();

        // TODO: To be removed in next version
        void PlayMusic(Music music);

        void StopMusic();

        void PlayDialogue(Database.LocalizationDataId text, System.Action onCompleted = null);

        void Update();
    }
}