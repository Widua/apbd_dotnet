using System.ComponentModel.DataAnnotations;

namespace apbd_dotnet.models;

public class Reservation : IValidatableObject
    {
        public int Id { get; set; }
        public int RoomId { get; set; }

        [Required(ErrorMessage = "Nazwa organizatora jest wymagana.")]
        public string OrganizerName { get; set; }

        [Required(ErrorMessage = "Temat jest wymagany.")]
        public string Topic { get; set; }

        public DateOnly Date { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }

        [Required]
        public string Status { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (EndTime <= StartTime)
            {
                yield return new ValidationResult(
                    "Czas zakończenia musi być późniejszy niż czas rozpoczęcia.",
                    new[] { nameof(EndTime) });
            }
        }
    }