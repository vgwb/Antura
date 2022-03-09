using System.Collections.Generic;

namespace Antura.UI
{
    /// <summary>
    /// Shows the number of current and max lives available in a minigame.
    /// </summary>
    public class MinigamesUILives : ABSMinigamesUIComponent
    {
        public MinigamesUISingleLife LifePrefab;

        public int MaxLives { get; private set; }
        public int CurrLives { get; private set; }
        private readonly List<MinigamesUISingleLife> lives = new List<MinigamesUISingleLife>();

        #region Unity

        void Awake()
        {
            LifePrefab.gameObject.SetActive(false);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Sets the max lives. Call this before calling any other method.
        /// </summary>
        /// <param name="_maxLives">Max lives</param>
        public void Setup(int _maxLives)
        {
            MaxLives = CurrLives = _maxLives;

            foreach (MinigamesUISingleLife life in lives)
                Destroy(life.gameObject);
            lives.Clear();

            for (int i = 0; i < MaxLives; ++i)
            {
                MinigamesUISingleLife life = (MinigamesUISingleLife)Instantiate(LifePrefab, LifePrefab.transform.parent, false);
                life.gameObject.SetActive(true);
                lives.Add(life);
            }

            IsSetup = true;
        }

        /// <summary>
        /// Sets the current lives to the given value and returns the value (representing the lives with a full heart).
        /// </summary>
        /// <param name="_to">Lives to set as active (with full heart)</param>
        /// <param name="_canExceedMax">If TRUE and the new value is higher than <see cref="MaxLives"/>,
        /// new life objects are created and <see cref="MaxLives"/> is modified accordingly.</param>
        public int SetCurrLives(int _to, bool _canExceedMax = false)
        {
            if (!Validate("MinigamesUILives"))
            { return 0; }

            if (!_canExceedMax && _to > MaxLives)
            {
                _to = MaxLives;
            }
            if (CurrLives == _to)
            {
                return CurrLives;
            }

            if (_to < 0)
            {
                _to = 0;
            }
            CurrLives = _to;
            for (int i = 0; i < lives.Count; i++)
            {
                MinigamesUISingleLife life = lives[i];
                if (i < _to)
                {
                    life.Gain();
                }
                else
                {
                    life.Lose();
                }
            }
            _to -= lives.Count;
            while (_to > 0)
            {
                _to--;
                MinigamesUISingleLife life = (MinigamesUISingleLife)Instantiate(LifePrefab, LifePrefab.transform.parent, false);
                life.gameObject.SetActive(true);
                lives.Add(life);
            }
            int actualMaxLives = lives.Count;
            if (MaxLives < actualMaxLives)
            {
                MaxLives = actualMaxLives;
            }
            return _to;
        }

        /// <summary>
        /// Adds one life and retuns the current active ones (the ones with a full heart).
        /// If the new value is higher than <see cref="MaxLives"/>,
        /// new life objects are created and <see cref="MaxLives"/> is modified accordingly.
        /// </summary>
        /// <param name="_canExceedMax">If TRUE and the new value is higher than <see cref="MaxLives"/>,
        /// new life objects are created and <see cref="MaxLives"/> is modified accordingly.</param>
        public int GainALife(bool _canExceedMax = false)
        {
            return SetCurrLives(CurrLives + 1, _canExceedMax);
        }

        /// <summary>Removes one active life and retuns the current active ones (the ones with a full heart)</summary>
        public int LoseALife()
        {
            if (!Validate("MinigamesUILives"))
                return 0;

            return CurrLives == 0 ? 0 : SetCurrLives(CurrLives - 1);
        }

        /// <summary>Makes all life objects active (with a full heart)</summary>
        public int ResetToMax()
        {
            return SetCurrLives(MaxLives);
        }

        #endregion
    }
}
