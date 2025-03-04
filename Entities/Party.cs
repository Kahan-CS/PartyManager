using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PartyManager.Entities
{
    public class Party
    {
        // PK
        [Key]
        public int PartyId { get; set; }

        [Required(ErrorMessage = "Please enter a description")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Please enter the event date")]
        [DataType(DataType.DateTime)]
        public DateTime EventDate { get; set; }

        [Required(ErrorMessage = "Please enter a location")]
        public string? Location { get; set; }

        public ICollection<Invitation>? Invitations { get; }

        [NotMapped]
        public int NumberOfInvitations => Invitations?.Count ?? 0;
    }

}
