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
        [CustomValidation(typeof(Party), nameof(ValidateFutureDate))]
        public DateTime EventDate { get; set; } = DateTime.Today;

        [Required(ErrorMessage = "Please enter a location")]
        public string? Location { get; set; }

        public virtual ICollection<Invitation> Invitations { get; set; } = new List<Invitation>();

        [NotMapped]
        public int NumberOfInvitations => Invitations?.Count ?? 0;

        public static ValidationResult ValidateFutureDate(DateTime date, ValidationContext context)
        {
            return date >= DateTime.Today
                ? ValidationResult.Success
                : new ValidationResult("Event date must be in the future.");
        }
    }

}
