using System.ComponentModel.DataAnnotations;

namespace TeamChallenge.Models.DTOs.Category
{
    public class CategoryCreateDto
    {
        [Required, StringLength(50)]
        public string Name { get; set; }
    }
}
