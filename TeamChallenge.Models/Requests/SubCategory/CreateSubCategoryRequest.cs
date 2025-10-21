using System.ComponentModel.DataAnnotations;

namespace TeamChallenge.Models.Requests.SubCategory
{
    public class CreateSubCategoryRequest
    {
        [Required]
        public string Name { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "A valid CategoryId is required.")]
        public int CategoryId { get; set; }
        //public List<int> ProductIds { get; set; } = new List<int>();
    }
}
