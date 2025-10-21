using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TeamChallenge.Models.Entities
{
    public class ReviewEntity : BaseEntity
    {
        [ForeignKey("User")]
        public string UserId { get; set; }
        public UserEntity User { get; set; }
        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public ProductEntity Product { get; set; }
        [Required, Range(1, 5)]
        public int Rating { get; set; }
        [MaxLength(500)]
        public string? Comment { get; set; }
    }
}