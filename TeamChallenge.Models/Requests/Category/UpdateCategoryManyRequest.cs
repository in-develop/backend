using System.ComponentModel.DataAnnotations;

namespace TeamChallenge.Models.Requests
{
    public class UpdateCategoryManyRequest
    {
        [Required]
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 100 characters.")]
        public string Name { get; set; }
    }
}
