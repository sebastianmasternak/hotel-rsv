using System.ComponentModel.DataAnnotations;

namespace backend.Models
{
    public class RoomType
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Standard Name cannot be longer than 100 characters.")]
        public string Name { get; set; }

        [StringLength(500, ErrorMessage = "Description cannot be longer than 500 characters.")]
        public string? Description { get; set; }

        [Range(0, 100000, ErrorMessage = "Price per night must be non-negative and reasonable.")]
        [DataType(DataType.Currency)]
        public decimal PricePerNight { get; set; }
    }
}
