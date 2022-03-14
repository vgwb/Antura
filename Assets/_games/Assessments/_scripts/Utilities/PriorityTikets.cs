using Kore.Utils;

namespace Antura.Assessment
{
    public class Ticket : IPoolable
    {
        public void Reset()
        {

        }
    }

    public class PriorityTikets
    {
        MiniPool<Ticket> ticketPool = new MiniPool<Ticket>(2);
        Ticket lowPriorityTicket = null;
        Ticket highPriorityTicket = null;

        public Ticket LockLowPriority()
        {
            var ticket = ticketPool.Acquire();

            if (lowPriorityTicket == null && highPriorityTicket == null)
            {
                lowPriorityTicket = ticket;
            }

            return ticket;
        }

        public bool IsLowPriorityTicketValid(Ticket ticket)
        {
            return lowPriorityTicket == ticket && highPriorityTicket == null;
        }

        public void UnlockLowPriorityTicket(Ticket ticket)
        {
            if (lowPriorityTicket == ticket)
            {
                lowPriorityTicket = null;
            }

            ticketPool.Release(ticket);
        }

        public Ticket LockHighPriority()
        {
            var ticket = ticketPool.Acquire();

            if (highPriorityTicket == null)
            {
                highPriorityTicket = ticket;
            }
            return ticket;
        }

        public bool IsHighPriorityTicketValid(Ticket ticket)
        {
            return highPriorityTicket == ticket;
        }

        public void UnlockHighPriorityTicket(Ticket ticket)
        {
            if (highPriorityTicket == ticket)
            {
                highPriorityTicket = null;
            }

            ticketPool.Release(ticket);
        }
    }
}
