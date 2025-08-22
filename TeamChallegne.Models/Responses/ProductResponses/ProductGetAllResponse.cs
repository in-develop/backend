using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamChallenge.Models.Entities;

namespace TeamChallenge.Models.Responses.ProductResponses
{
    public class ProductGetAllResponse : BaseDataListResponse<ProductEntity>
    {
        public ProductGetAllResponse(IEnumerable<ProductEntity> data) : base(data)
        {
        }
    }
}
