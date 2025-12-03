using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace backend.Models
{
    public class Reservation : IValidatableObject
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int GuestId { get; set; }
        public Guest? Guest { get; set; }

        [Required]
        public int RoomId { get; set; }
        public Room? Room { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime CheckInDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime CheckOutDate { get; set; }

        [Range(0, 1000000, ErrorMessage = "Total price must be non-negative.")]
        [DataType(DataType.Currency)]
        public decimal TotalPrice { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (CheckOutDate <= CheckInDate)
            {
                yield return new ValidationResult(
                    "Check-out date must be after check-in date.",
                    new[] { nameof(CheckOutDate), nameof(CheckInDate) }
                );
            }

            if (TotalPrice < 0)
            {
                yield return new ValidationResult(
                    "Total price must be non-negative.",
                    new[] { nameof(TotalPrice) }
                );
            }
        }
    }
}
