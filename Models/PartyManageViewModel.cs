using Microsoft.AspNetCore.Mvc.ModelBinding;
using PartyManager.Entities;

namespace PartyManager.Models
{
    public class PartyManageViewModel
    {
        // For binding the party identifier on POST.
        public int PartyId { get; set; }
        [BindNever]
        public Party? Party { get; set; }
        public List<Invitation>? Invitations { get; set; }

        // Holds data for a new invitation to be added.
        public Invitation? NewInvitation { get; set; }

        // Computed properties for invitation counts
        public int NotSentCount => Invitations?.Count(i => i.Status == InvitationStatus.InviteNotSent) ?? 0;
        public int SentCount => Invitations?.Count(i => i.Status == InvitationStatus.InviteSent) ?? 0;
        public int RespondedYesCount => Invitations?.Count(i => i.Status == InvitationStatus.RespondedYes) ?? 0;
        public int RespondedNoCount => Invitations?.Count(i => i.Status == InvitationStatus.RespondedNo) ?? 0;
    }
}
