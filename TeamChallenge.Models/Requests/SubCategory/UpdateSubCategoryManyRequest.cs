using System.ComponentModel.DataAnnotations;

namespace TeamChallenge.Models.Requests.SubCategory
{
    public class UpdateSubCategoryManyRequest
    {
        [Required]
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [StringLength(100, MinimumLength = 2)]
        public string Name { get; set; }

        public int? CategoryId { get; set; }
    }
}
