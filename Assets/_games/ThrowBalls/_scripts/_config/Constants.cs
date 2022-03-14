using UnityEngine;

namespace Antura.Minigames.ThrowBalls
{
    public class Constants
    {
        public const string CORRECT_LETTER_TAG = "CorrectLetter";
        public const string WRONG_LETTER_TAG = "WrongLetter";

        public const string TAG_POKEBALL = "Pokeball";
        public const string TAG_RAIL = "Rail";

        public static readonly Vector3 GRAVITY = new Vector3(0, -150f, 0);

        // This exists since in some Update methods, we need to divide by gravity.
        // Division is much more expensive than multiplication, so do the division once, store it here, and reference it whenever needed:
        public static readonly Vector3 GRAVITY_INVERSE = new Vector3(0, 1f / -150f, 0);

        public static int INVISIBLE_LAYER = 11;
        public static int DEFAULT_LAYER = 0;
    }
}
