using PartyManager.Entities;

namespace PartyManager.Models
{
    public class PartyModifyViewModel
    {
        public Party Party { get; set; }
        public List<Invitation> Invitations { get; set; }
        // Holds data for a new invitation to be added.
        public Invitation NewInvitation { get; set; }
    }
}
