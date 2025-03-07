using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PartyManager.Entities
{
    /// <summary>
    /// Entity representing a specific party/event
    /// </summary>
    public class Party
    {
        // PK
        [Key]
        public int PartyId { get; set; }

        // Description of a party/event: Comprises of name basically
        [Required(ErrorMessage = "Please enter a description")]
        public string? Description { get; set; }

        // Event date property with custom validations
        [Required(ErrorMessage = "Please enter the event date")]
        [DataType(DataType.DateTime)]
        [CustomValidation(typeof(Party), nameof(ValidateFutureDate))]
        public DateTime EventDate { get; set; } = DateTime.Today;

        // Location for the Party/event
        [Required(ErrorMessage = "Please enter a location")]
        public string? Location { get; set; }

        // List of Invitations for the party/event
        public virtual ICollection<Invitation> Invitations { get; set; } = new List<Invitation>();

        // Unmapped property to display the # of invites.
        [NotMapped]
        public int NumberOfInvitations => Invitations?.Count ?? 0;

        // Custom Validation for Event date: it has to be today or in future.
        public static ValidationResult? ValidateFutureDate(DateTime date, ValidationContext context)
        {
            return date >= DateTime.Today
                ? ValidationResult.Success
                : new ValidationResult("Event date must be in the future.");
        }
    }

}
