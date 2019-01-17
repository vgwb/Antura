using Antura.Audio;
using Antura.Database;
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

        public IAudioSource PlayVocabularyData(ILivingLetterData data, bool exclusive = true, LetterDataSoundType soundType = LetterDataSoundType.Phoneme)
        {
            if (data.DataType == LivingLetterDataType.Letter) {
                return AudioManager.I.PlayLetter((data as LL_LetterData).Data, exclusive, soundType);
            } else if (data.DataType == LivingLetterDataType.Word)
            {
                return AudioManager.I.PlayWord((data as LL_WordData).Data, exclusive);
            } else if (data.DataType == LivingLetterDataType.Image) {
                return AudioManager.I.PlayWord((data as LL_ImageData).Data, exclusive);
            } else if (data.DataType == LivingLetterDataType.Phrase) {
                return AudioManager.I.PlayPhrase((data as LL_PhraseData).Data, exclusive);
            }
            return null;
        }

        public void PlayDialogue(Database.LocalizationDataId text, System.Action callback = null)
        {
            if (callback == null) {
                AudioManager.I.PlayDialogue(text.ToString());
            } else {
                AudioManager.I.PlayDialogue(text.ToString(), callback);
            }
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
            return AudioManager.I.GetAudioClip(sfx);
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