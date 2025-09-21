using System.ComponentModel.DataAnnotations;

namespace TeamChallenge.Models.Requests.SubCategory
{
    public class CreateSubCategoryManyRequest
    {
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(100, MinimumLength = 2)]
        public string Name { get; set; }

        [Required(ErrorMessage = "CategoryId is required.")]
        public int CategoryId { get; set; }
    }
}
