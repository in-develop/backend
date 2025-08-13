using System.ComponentModel.DataAnnotations;

namespace TeamChallenge.Models.DTOs
{
    public class CategoryCreateDto
    {
        public int Id { get; set; }

        [Required, StringLength(50)]
        public string Name { get; set; }
    }
}
