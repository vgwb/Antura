using UnityEngine;

namespace Antura.Minigames.SickLetters
{
    // Zone where we can drop wrong dots.
    // Letters contained inside the "letters" string have nothing in this area, so we can safely drop a wrong dot here.
    // TODO: Deprecated, should be removed after the use of SideLetterData is validated.
    public class SickLettersDropZone : MonoBehaviour
    {
        public string letters;
    }
}
