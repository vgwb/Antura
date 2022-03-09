using Antura.LivingLetters;
using UnityEngine;

namespace Antura.Assessment
{
    public class SortingTicket : MonoBehaviour
    {
        public int number = -1;
        public ILivingLetterData data = null;
    }

    public static class AddTicketGoExtension
    {
        public static SortingTicket AddTicket(this Answer answ, int ticketN)
        {
            var comp = answ.gameObject.AddComponent<SortingTicket>();
            comp.data = answ.gameObject.GetComponent<StillLetterBox>().Data;
            comp.number = ticketN;
            return comp;
        }

        public static int GetTicket(this Answer answ)
        {
            return answ.gameObject.GetComponent<SortingTicket>().number;
        }
    }
}
