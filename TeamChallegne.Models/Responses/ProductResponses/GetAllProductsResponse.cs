using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamChallenge.Models.Entities;

namespace TeamChallenge.Models.Responses
{
    public class GetAllProductsResponse : BaseDataListResponse<ProductEntity>
    {
        public GetAllProductsResponse(IEnumerable<ProductEntity> data) : base(data)
        {
        }
    }
}
