using Antura.Core;
using Antura.LivingLetters;
using UnityEngine;
using TMPro;

namespace Antura.Minigames.DancingDots
{

    public enum LivingLetterAnim {
		Nothing = 0,
		idle = 1,
		hold = 2,
		run = 3,
		walk = 4,
		ninja = 5
	}

	public class DancingDotsLivingLetter : MonoBehaviour
	{

		[Header("References")]
		public LivingLetterController letterObjectView;
		public TextMeshPro hintText;
		public TextMeshPro dotlessText;
		public GameObject fullTextGO, contentGO;

		public GameObject rainbow;
		public DancingDotsGame game;

		TextMeshPro fullText;

		public ILivingLetterData letterData { get; private set; }

		void Start()
		{

			fullText = fullTextGO.GetComponent<TextMeshPro>();
			HideRainbow();
			PlayAnimation();
		}

		public void Reset()
		{
			SetupLetter();
			SpeakLetter();
		}

		void PlayAnimation()
		{
			letterObjectView.SetState(LLAnimationStates.LL_dancing);
		}

		void OnMouseUp()
		{
			if (letterData != null)
            {
                DancingDotsConfiguration.Instance.Context.GetAudioManager().PlayVocabularyData(letterData);
			}
		}

		void GetDiacritic()
		{
			//Debug.Log("DD Get Diacritics");
			char FATHA1 = (char) 1611;
			char FATHA2 = (char) 1614;
			char DAMAH = (char) 1615;
			char KASRAH = (char) 1616;
			char SOUKON = (char) 1618;

			if (game.currentLetter.Contains(FATHA1.ToString()) ||
				game.currentLetter.Contains(FATHA2.ToString()))
			{
				game.letterDiacritic = DiacriticEnum.Fatha;
			}
			else if (game.currentLetter.Contains(DAMAH.ToString()))
			{
				game.letterDiacritic = DiacriticEnum.Dameh;
			}

			else if (game.currentLetter.Contains(KASRAH.ToString()))
			{
				game.letterDiacritic = DiacriticEnum.Kasrah;
			}

			else if (game.currentLetter.Contains(SOUKON.ToString()))
			{
				game.letterDiacritic = DiacriticEnum.Sokoun;
			}
			else
			{
				game.letterDiacritic = DiacriticEnum.None;
            }
            //Debug.Log("DD found diacritic: " + game.letterDiacritic);

            StartCoroutine(game.SetupDiacritic());

			string output = "";
			foreach (char c in game.currentLetter)
			{
				if (c != FATHA1 && c != FATHA2 && c != DAMAH && c != KASRAH && c != SOUKON)
				{
					output += c;
				}
			}
			game.currentLetter = output;
		}

		void SetupLetter()
		{
			letterData = game.questionsManager.getNewLetter();

			game.currentLetter = letterData.TextForLivingLetter;

			GetDiacritic();

            /* HACK: removed DOTS handling

			string lettersWithOneDot = "ج خ غ ف ض ب ن ز ذ ظ";
			string lettersWithTwoDots = "ة ق ي ت";
			string lettersWithThreeDots = "ث ش";

			if (lettersWithThreeDots.Contains(game.removeDiacritics(game.currentLetter)))
			{
                game.dotsCount = 3;
			}
			else if (lettersWithTwoDots.Contains(game.removeDiacritics(game.currentLetter)))
			{
                game.dotsCount = 2;
			}
			else if (lettersWithOneDot.Contains(game.removeDiacritics(game.currentLetter)))
			{
                game.dotsCount = 1;
			}
			else
			{
                game.dotsCount = 0;
			}
            */
		    game.dotsCount = 0; // HACK: forced dots count
					
			hintText.text = game.currentLetter;
			ShowText(hintText, game.dotHintAlpha);
			dotlessText.text = game.currentLetter;
			fullText.text = game.currentLetter;

            //fullTextGO.SetActive(false); // HACK: forced full letter appearing
        }

        public void HideText(TextMeshPro tmp)
		{
			tmp.color = game.SetAlpha(tmp.color,0);
		}

		public void ShowText(TextMeshPro tmp, byte alpha)
		{
			tmp.color = game.SetAlpha(tmp.color, alpha);
		}

		public void ShowRainbow()
		{
			rainbow.SetActive(true);
		}

		public void HideRainbow()
		{
			rainbow.SetActive(false);
		}

		private void SpeakLetter()
		{
			if (letterData != null && !game.isTutRound)
            {
                DancingDotsConfiguration.Instance.Context.GetAudioManager().PlayVocabularyData(letterData);
			}
		}

//		public void HideAllText()
//		{
//			hintText.color = SetAlpha(hintText.color,0);
//			dotlessText.color = SetAlpha(dotlessText.color,0);
//			fullText.color = SetAlpha(fullText.color,0);
//		}
//
//		public void ShowAllText()
//		{
//			hintText.color = SetAlpha(hintText.color,DancingDotsGameManager.instance.dotHintAlpha);
//			dotlessText.color = SetAlpha(dotlessText.color,255);
//			fullText.color = SetAlpha(fullText.color,255);
//		}

	}
}