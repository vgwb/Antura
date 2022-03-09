using System;
using Antura.LivingLetters;
using UnityEngine;

namespace Antura.Minigames
{
    /// <summary>
    /// Provides access to the MinigamesStarsWidget UI element for minigames.
    /// <seealso cref="MinigamesPopupWidget"/>
    /// </summary>
    public interface IPopupWidget
    {
        // Manual popup management
        void Show(bool reset = true);

        void SetButtonCallback(System.Action callback);
        void SetTitle(Database.LocalizationDataId text);
        void SetTitle(string text);
        void SetMessage(Database.LocalizationDataId text);
        void SetMessage(string text);
        void SetImage(Sprite image);
        void SetLetterData(ILivingLetterData data); // Modifies Text + Image
        void SetMark(bool visible, bool correct);

        void Hide();

        void ShowTimeUp(System.Action callback);

        [Obsolete("Using manual configuration", false)]
        void Show(System.Action callback, Database.LocalizationDataId text, bool markResult, LL_WordData word = null);

        [Obsolete("Using manual configuration", false)]
        void Show(System.Action callback, Database.LocalizationDataId text, LL_WordData word = null);

        [Obsolete("Using manual configuration", false)]
        void Show(System.Action callback, Sprite image);
    }
}
