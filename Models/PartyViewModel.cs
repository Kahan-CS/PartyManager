using PartyManager.Entities;

namespace PartyManager.Models
{
    public class PartyViewModel
    {
        public Party? Party { get; set; }
        public List<Invitation>? Invitations { get; set; }
    }
}
