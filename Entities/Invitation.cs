using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PartyManager.Entities
{
    // enum representing the various status of an invitation.
    public enum InvitationStatus
    {
        InviteNotSent,
        InviteSent,
        RespondedYes,
        RespondedNo
    }


    /// <summary>
    /// Entity representing a particular invitation (of a particular party)
    /// </summary>
    /// 
    public class Invitation
    {
        // Primary Key for the Invitation entity
        [Key]
        public int InvitationId { get; set; }

        // Guest's name with validation
        [Required(ErrorMessage = "Please enter Guest's name")]
        public string? GuestName { get; set; }

        // Guest's email with validation
        [Required(ErrorMessage = "Please enter Guest's email")]
        [EmailAddress(ErrorMessage = "Please enter a valid email")]
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]{2,}$", ErrorMessage = "Please enter a valid email with a proper domain")]
        public string? GuestEmail { get; set; }

        // Status of the invitation with a default value
        [Required]
        public InvitationStatus Status { get; set; } = InvitationStatus.InviteNotSent;

        // Foreign key to the Party entity
        public int PartyId { get; set; }
        // Navigation property to the Party entity
        public Party? Party { get; set; }

        // Human-readable status of the invitation, not mapped to the database
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
