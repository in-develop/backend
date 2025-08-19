using System.ComponentModel.DataAnnotations;

namespace TeamChallenge.Models.DTOs.Category
{
    public class CategoryCreateDto : BaseDTO
    {
        [Required, StringLength(50, MinimumLength = 3)]
        public string Name { get; set; }
    }
}
