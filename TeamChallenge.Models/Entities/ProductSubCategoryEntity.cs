using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace TeamChallenge.Models.Entities
{
    public class ProductSubCategoryEntity : BaseEntity
    {
        [ForeignKey("Product")]
        [Required]
        public int ProductId { get; set; }
        public ProductEntity Product { get; set; }

        [ForeignKey("SubCategory")]
        public int SubCategoryId { get; set; }
        public SubCategoryEntity SubCategory { get; set; }
    }
}
