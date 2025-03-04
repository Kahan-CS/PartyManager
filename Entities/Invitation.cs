using System.ComponentModel.DataAnnotations;

namespace PartyManager.Entities
{
    public enum InvitationStatus
    {
        InviteNotSent,
        InviteSent,
        RespondedYes,
        RespondedNo
    }


    public class Invitation
    {
        // PK
        [Key]
        public int InvitationId { get; set; }

        [Required]
        public string? GuestName { get; set; }

        [Required]
        [EmailAddress]
        public string? GuestEmail { get; set; }

        [Required]
        public InvitationStatus Status { get; set; } = InvitationStatus.InviteNotSent;

        public int PartyId { get; set; }
        public Party? Party { get; set; }
    }

}
