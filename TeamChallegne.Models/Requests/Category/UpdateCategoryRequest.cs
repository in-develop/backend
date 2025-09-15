using System.ComponentModel.DataAnnotations;

namespace TeamChallenge.Models.Requests.Category
{
    public class UpdateCategoryRequest
    {
<<<<<<< HEAD
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 100 characters.")]
=======
        [Required, StringLength(50, MinimumLength = 3)]
>>>>>>> master
        public string Name { get; set; }

        public List<int> SubCategoryIds { get; set; } = new List<int>();
    }
}
