using PartyManager.Entities;
using System.ComponentModel.DataAnnotations;

namespace PartyManager.Models
{
    public class InvitationResponseViewModel
    {
        public int InvitationId { get; set; }

        public string? GuestName { get; set; }

        public string? PartyDescription { get; set; }

        [Required(ErrorMessage = "Please select an option.")]
        public InvitationStatus Status { get; set; }
    }
}
