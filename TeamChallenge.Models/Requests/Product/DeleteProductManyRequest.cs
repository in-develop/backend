using System.ComponentModel.DataAnnotations;

namespace TeamChallenge.Models.Requests.Product
{
    public class DeleteProductManyRequest
    {
        [Required, MinLength(1, ErrorMessage = "You must provide at least one Id.")]
        public List<int> Ids { get; set; }
    }
}
