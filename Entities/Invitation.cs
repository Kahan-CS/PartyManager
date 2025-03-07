using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

        [Required(ErrorMessage = "Please enter Guest's name")]
        public string? GuestName { get; set; }

        [Required(ErrorMessage = "Please enter Guest's email")]
        [EmailAddress(ErrorMessage = "Please enter a valid email")]
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]{2,}$", ErrorMessage = "Please enter a valid email with a proper domain")]
        public string? GuestEmail { get; set; }

        [Required]
        public InvitationStatus Status { get; set; } = InvitationStatus.InviteNotSent;

        public int PartyId { get; set; }
        public Party? Party { get; set; }

        [NotMapped]
        public string HumanReadableStatus
        {
            get
            {
                return Status switch
                {
                    InvitationStatus.InviteNotSent => "Invite Not Sent",
                    InvitationStatus.InviteSent => "Invitation Sent",
                    InvitationStatus.RespondedYes => "Responded Yes",
                    InvitationStatus.RespondedNo => "Responded No",
                    _ => Status.ToString(),
                };
            }
        }
    }

}
